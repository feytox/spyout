using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PassiveWalkingBehavior : NpcBehavior
{
    [SerializeField] private WaypointsController _waypoints;

    public override bool Reloadable => true;

    public override IEnumerable<NpcTask> CreateTasks(TaskData taskData)
    {
        return new PassiveWalkTask(taskData, _waypoints.Points.Value).Yield();
    }
}