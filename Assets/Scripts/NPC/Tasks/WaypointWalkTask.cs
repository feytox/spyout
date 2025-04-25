using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public abstract class WaypointWalkTask : NPCTask
{
    private readonly HashSet<Vector2Int> _waypoints;
    private readonly Queue<Vector2> _path;

    protected WaypointWalkTask([NotNull] TaskData taskData, Vector2Int[] waypoints) : base(taskData)
    {
        _waypoints = waypoints.ToHashSet();
    }

    public override bool Step()
    {
        if (UpdatePath())
            return true;

        MoveByPath();
        return false;
    }

    private bool UpdatePath()
    {
        if (_path.Count > 0)
            return false;

        var currentPos = GridController.WorldToGridCell(NPC.transform.position);
        if (!TryGetNextWaypoint(currentPos, out var target))
            return true;

        var targetPath = GridController.FindPath(NPC.gameObject, currentPos, target)
            .Select(GridController.CellToNormalWorld);

        foreach (var pos in targetPath)
            _path.Enqueue(pos);

        return false;
    }

    private bool TryGetNextWaypoint(Vector2Int currentPos, out Vector2Int nextWaypoint)
    {
        if (_waypoints.Count == 0)
        {
            nextWaypoint = default;
            return false;
        }

        nextWaypoint = _waypoints.MinBy(waypoint => (waypoint - currentPos).sqrMagnitude);
        _waypoints.Remove(nextWaypoint);
        return true;
    }

    private void MoveByPath()
    {
        while (true)
        {
            if (!_path.TryPeek(out var currentTarget))
                return;

            if (!NPC.MoveToTarget(currentTarget))
                break;
        }
    }
}