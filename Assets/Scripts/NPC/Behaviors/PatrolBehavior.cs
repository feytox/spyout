using System.Collections.Generic;
using UnityEngine;

public class PatrolBehavior : NPCBehavior
{
    [SerializeField] private WaypointsController _waypoints;

    public override bool Reloadable => true;

    public override IEnumerable<NPCTask> CreateTasks(TaskData taskData)
    {
        return new NPCTask[] { 
            PatrolTask<PlayerController>.OfPlayer(taskData, _waypoints.Points.Value), 
            AttackTask<PlayerController>.OfPlayer(taskData) 
        };
    }
}