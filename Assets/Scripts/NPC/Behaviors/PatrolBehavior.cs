using System.Collections.Generic;
using UnityEngine;

public class PatrolBehavior : NpcBehavior
{
    [SerializeField] private WaypointsController _waypoints;

    public override bool Reloadable => true;

    public override IEnumerable<NpcTask> CreateTasks(TaskData taskData)
    {
        return new NpcTask[]
        {
            PatrolTask<PlayerController>.OfPlayer(taskData, _waypoints.Points.Value),
            AttackTask<PlayerController>.OfPlayer(taskData, true)
        };
    }
}