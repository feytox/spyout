using System.Linq;
using UnityEngine;
using Utils;

public class FollowPlayerTask : NPCTask
{
    private const float TargetMinimumSqrDistance = 0.2f;
    private const int LockedPathPoints = 1;

    private readonly Cooldown _targetUpdateCooldown = new(0.2f);
    private readonly OverflowBuffer<Vector2> _currentPath = new();
    private Vector2Int? _targetPos;

    public FollowPlayerTask(TaskData taskData) : base(taskData)
    {
    }

    public override bool Step()
    {
        UpdatePath();
        MoveByPath();
        return false;
    }

    private void UpdatePath()
    {
        if (_targetPos is not null && !_targetUpdateCooldown.ResetIfExpired())
            return;

        if (_targetPos is null)
            _targetUpdateCooldown.Reset();

        var newTargetPos = GridController.WorldToGridCell(PlayerController.Position);
        if (newTargetPos == _targetPos)
            return;

        _targetPos = newTargetPos;
        _currentPath.Trim(LockedPathPoints);

        var start = _currentPath.TryPeekLast(out var lastPathPoint)
            ? GridController.WorldToGridCell(lastPathPoint)
            : GridController.WorldToGridCell(NPC.transform.position);

        var deltaPath = GridController.FindPath(NPC.gameObject, start, newTargetPos)
            .Select(GridController.CellToWorld)
            .Select(VectorsExtensions.ToCellCenter)
            .ToArray();
        
        if (deltaPath.Length == 0)
        {
            _currentPath.Trim(0);
            _targetPos = null;
            return;
        }

        _currentPath.EnqueueRange(deltaPath);
    }

    private void MoveByPath()
    {
        while (true)
        {
            if (!_currentPath.TryPeek(out var currentTarget))
                return;

            var moveVec = currentTarget - (Vector2)NPC.transform.position;
            if (moveVec.sqrMagnitude <= TargetMinimumSqrDistance)
            {
                _currentPath.Dequeue();
                continue;
            }

            NPC.MoveInDirection(moveVec.normalized);
            break;
        }
    }

    public override NPCTask CreateNextTask(TaskData taskData) => null;
}