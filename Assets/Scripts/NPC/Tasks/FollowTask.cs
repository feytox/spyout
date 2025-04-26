#nullable enable
using System.Linq;
using UnityEngine;
using Utils;

/// <summary>
/// Задача следования за целью с поиском пути
/// </summary>
public class FollowTask : NPCTask
{
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
        if (UpdatePath())
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
        
        var currentPos = _grid!.WorldToCell(NPC.transform.position);
        var newTargetPos = _grid!.WorldToCell(_target.Position);
        if (IsTargetReached(currentPos, newTargetPos))
            return true;

        if (newTargetPos == _targetPos)
            return false;
        
        _targetPos = newTargetPos;
        _currentPath.Trim(LockedPathPoints);
        RefreshPath(currentPos);
        return false;
    }

    private void RefreshPath(Vector2Int currentPos)
    {
        var start = _currentPath.TryPeekLast(out var lastPathPoint)
            ? _grid!.WorldToCell(lastPathPoint)
            : currentPos;

        var deltaPath = _grid!.FindPath(NPC.gameObject, start, _targetPos!.Value)
            .Select(_grid.CellToNormalWorld)
            .ToArray();

        if (deltaPath.Length != 0)
        {
            _currentPath.EnqueueRange(deltaPath);
            return;
        }

        _currentPath.Clear();
        _targetPos = null;
    }

    private static bool IsTargetReached(Vector2Int currentPos, Vector2Int targetPos)
    {
        return (currentPos - targetPos).sqrMagnitude <= NPCController.TargetMinimumSqrDistance;
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