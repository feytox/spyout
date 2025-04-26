#nullable enable
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Задача перемещения по заданному порядку точек (waypoints) с использованием поиска пути.
/// </summary>
public class WaypointWalkTask : NPCTask
{
    private readonly Queue<Vector2Int> _waypoints;
    private readonly Queue<Vector2> _path = new();
    private GridController? _grid;

    protected WaypointWalkTask(TaskData taskData, Vector2Int[] waypoints) : base(taskData)
    {
        _waypoints = new Queue<Vector2Int>(waypoints);
    }

    protected override void OnTaskStart()
    {
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
        if (_path.Count > 0)
            return false;

        var currentPos = _grid!.WorldToCell(NPC.transform.position);
        if (!TryGetNextWaypoint(out var target))
            return true;

        var targetPath = _grid.FindPath(NPC.gameObject, currentPos, target)
            .Select(_grid.CellToNormalWorld);

        foreach (var pos in targetPath)
            _path.Enqueue(pos);

        return false;
    }

    private bool TryGetNextWaypoint(out Vector2Int nextWaypoint)
    {
        if (_waypoints.Count == 0)
        {
            nextWaypoint = default;
            return false;
        }

        nextWaypoint = _waypoints.Dequeue();
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
            
            _path.Dequeue();
        }
    }
    
    public override NPCTask? CreateNextTask(TaskData taskData) => null;
}