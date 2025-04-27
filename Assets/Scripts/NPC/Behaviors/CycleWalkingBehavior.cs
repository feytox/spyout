using System.Collections.Generic;
using UnityEngine;

public class CycleWalkingBehavior : NPCBehavior
{
    [SerializeField] private WaypointsController _waypoints;

    public override bool Reloadable => true;

    public override IEnumerable<NPCTask> CreateTasks(TaskData taskData)
    {
        return new[] { new CycleWaypointWalkTask(taskData, _waypoints.Points.Value) };
    }
}