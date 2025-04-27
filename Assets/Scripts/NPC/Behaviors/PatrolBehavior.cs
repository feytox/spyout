#nullable enable
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatrolBehavior : NPCBehavior
{
    [SerializeField] private WaypointsController? _waypoints;

    private Vector2Int[]? _waypointsPoses;

    public override bool Reloadable => true;

    public override IEnumerable<NPCTask> CreateTasks(TaskData taskData)
    {
        _waypointsPoses ??= _waypoints!.Points.Select(GridController.GetInstance().WorldToCell).ToArray();
        return new NPCTask[] { 
            PatrolTask<PlayerController>.OfPlayer(taskData, _waypointsPoses), 
            AttackTask<PlayerController>.OfPlayer(taskData) 
        };
    }
}