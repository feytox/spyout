using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CycleWalkingBehavior : NPCBehavior
{
    [SerializeField] private WaypointsController _waypoints;

    public override bool Reloadable => false;

    public override IEnumerable<NPCTask> CreateTasks(TaskData taskData)
    {
        var points = _waypoints.Points.Select(GridController.GetInstance().WorldToCell).ToArray();
        return new[] { new CycleWaypointWalkTask(taskData, points) };
    }
}