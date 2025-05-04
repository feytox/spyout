#nullable enable
using System.Linq;
using UnityEngine;
using Utils;

/// <summary>
/// Задача следования за целью с поиском пути
/// </summary>
public class FollowTask : NPCTask
{
    // TODO: move const to somewhere else
    private const int MaxPathLength = 15;
    private const int LockedPathPoints = 1;
    private const float TargetUpdateCooldown = 0.2f;

    private readonly Cooldown _targetUpdateCooldown = new(TargetUpdateCooldown);
    private readonly OverflowBuffer<Vector2> _currentPath = new();
    private readonly IPositionProvider _target;
    
    private GridController? _grid;
    private Vector2Int? _targetPos;

    public FollowTask(TaskData taskData, IPositionProvider target) : base(taskData)
    {
        _target = target;
    }

    public static FollowTask OfPlayer(TaskData taskData)
    {
        return new FollowTask(taskData, PlayerController.GetInstance());
    }

    protected override void OnTaskStart()
    {
        _currentPath.Clear();
        _grid = GridController.GetInstance();
    }

    public override bool Step()
    {
        if (_target.IsDead || UpdatePath())
            return true;
        
        MoveByPath();
        return false;
    }
    
    // TODO: это стоит закинуть в async
    // ReSharper disable Unity.PerformanceAnalysis
    private bool UpdatePath()
    {
        if (_targetPos is null)
            _targetUpdateCooldown.Reset();
        else if (!_targetUpdateCooldown.ResetIfExpired())
            return false;
        
        if (NPC.IsTargetReached(_target))
            return true;
        
        var newTargetPos = _grid!.WorldToCell(_target.Position);
        if (newTargetPos == _targetPos)
            return _currentPath.Count == 0;
        
        _targetPos = newTargetPos;
        _currentPath.Trim(LockedPathPoints);
        return RefreshPath(_grid!.WorldToCell(NPC.transform.position));
    }

    private bool RefreshPath(Vector2Int currentPos)
    {
        var start = _currentPath.TryPeekLast(out var lastPathPoint)
            ? _grid!.WorldToCell(lastPathPoint)
            : currentPos;

        var deltaPath = _grid!.FindPath(NPC.gameObject, start, _targetPos!.Value, MaxPathLength)
            .Select(_grid.CellToNormalWorld)
            .ToArray();

        if (deltaPath.Length != 0)
        {
            _currentPath.EnqueueRange(deltaPath);
            return false;
        }

        _currentPath.Clear();
        _targetPos = null;
        return true;
    }

    private void MoveByPath()
    {
        while (true)
        {
            if (!_currentPath.TryPeek(out var currentTarget))
                return;

            if (!NPC.MoveToTarget(currentTarget))
                break;
            
            _currentPath.Dequeue();
        }
    }

    public override NPCTask? CreateNextTask(TaskData taskData) => null;
}