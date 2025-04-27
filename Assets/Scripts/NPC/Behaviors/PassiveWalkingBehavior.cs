using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PassiveWalkingBehavior : NPCBehavior
{
    [SerializeField] private WaypointsController _waypoints;
    
    public override bool Reloadable => true;
    
    public override IEnumerable<NPCTask> CreateTasks(TaskData taskData)
    {
        return new PassiveWalkTask(taskData, _waypoints.Points.Value).Yield();
    }
}