#nullable enable
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

/// <summary>
/// Задача перемещения по заданному порядку точек (waypoints) с использованием поиска пути.
/// </summary>
public class WaypointWalkTask : NPCTask
{
    private readonly Queue<Vector2Int> _waypoints;
    private readonly Queue<Vector2> _path = new();
    private GridController? _grid;
    private Task? _pathUpdateTask;

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

    protected virtual bool CanWalkNext() => true;
    
    private bool UpdatePath()
    {
        if (_path.Count > 0 || !CanWalkNext())
            return false;

        if (_pathUpdateTask is not null && !_pathUpdateTask.IsCompleted)
            return false;

        var currentPos = _grid!.WorldToCell(NPC.transform.position);
        if (!TryGetNextWaypoint(out var target))
            return true;
        
        _pathUpdateTask = CreatePathUpdateTask(currentPos, target);
        return false;
    }
    
    // TODO: refactor
    // ReSharper disable Unity.PerformanceAnalysis
    private Task CreatePathUpdateTask(Vector2Int currentPos, Vector2Int target)
    {
        return Task.Run(() => _grid!
                .FindPathOrClosest(NPC, currentPos, target, NPC.MaxPathLength)
                .Select(_grid.CellToNormalWorld), NPC.destroyCancellationToken)
            .ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                    foreach (var pos in task.Result)
                        _path.Enqueue(pos);
                else if (task.Exception is not null)
                    Debug.LogException(task.Exception);
            }, TaskScheduler.FromCurrentSynchronizationContext());
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