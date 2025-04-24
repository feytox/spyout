using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

public class FollowPlayerTask : NPCTask
{
    private const float TargetMinimumSqrDistance = 0.25f;
    
    private readonly Cooldown _targetUpdateCooldown = new(0.25f);
    private Queue<Vector2> _currentPath = new();
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
        var start = GridController.WorldToGridCell(NPC.transform.position); 
        var deltaPath = GridController
            .FindPath(NPC.gameObject, start, newTargetPos)
            .Select(GridController.CellToWorld)
            .Select(pos => pos + new Vector2(0.5f, 0.5f));

        _currentPath = new Queue<Vector2>(deltaPath);
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