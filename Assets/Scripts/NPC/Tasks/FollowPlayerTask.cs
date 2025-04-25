#nullable enable
using System.Linq;
using UnityEngine;
using Utils;

public class FollowPlayerTask : NPCTask
{
    private const int LockedPathPoints = 1;

    private readonly Cooldown _targetUpdateCooldown = new(0.2f);
    private readonly OverflowBuffer<Vector2> _currentPath = new();
    
    private GridController? _grid;
    private PlayerController? _player;
    private Vector2Int? _targetPos;

    public FollowPlayerTask(TaskData taskData) : base(taskData)
    {
    }

    protected override void OnTaskStart()
    {
        _grid = GridController.GetInstance();
        _player = PlayerController.GetInstance();
    }

    public override bool Step()
    {
        UpdatePath();
        MoveByPath();
        return false;
    }
    
    // TODO: это стоит закинуть в async
    // ReSharper disable Unity.PerformanceAnalysis
    private void UpdatePath()
    {
        if (RefreshTargetPosition()) 
            return;

        var start = _currentPath.TryPeekLast(out var lastPathPoint)
            ? _grid!.WorldToCell(lastPathPoint)
            : _grid!.WorldToCell(NPC.transform.position);

        var deltaPath = _grid.FindPath(NPC.gameObject, start, _targetPos!.Value)
            .Select(_grid.CellToNormalWorld)
            .ToArray();

        if (deltaPath.Length != 0)
        {
            _currentPath.EnqueueRange(deltaPath);
            return;
        }

        _currentPath.Trim(0);
        _targetPos = null;
    }

    private bool RefreshTargetPosition()
    {
        if (_targetPos is not null && !_targetUpdateCooldown.ResetIfExpired())
            return true;

        if (_targetPos is null)
            _targetUpdateCooldown.Reset();

        var newTargetPos = _grid!.WorldToCell(_player!.Position);
        if (newTargetPos == _targetPos)
            return true;

        _targetPos = newTargetPos;
        _currentPath.Trim(LockedPathPoints);
        return false;
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