using JetBrains.Annotations;
using UnityEngine;

/// <inheritdoc cref="WaypointWalkTask"/>
/// <remarks>Перемещается по точкам циклично</remarks>
public class CycleWaypointWalkTask : WaypointWalkTask
{
    private readonly Vector2Int[] _waypoints;
    
    public CycleWaypointWalkTask([NotNull] TaskData taskData, [NotNull] Vector2Int[] waypoints) : base(taskData, waypoints)
    {
        _waypoints = waypoints;
    }

    public override NPCTask CreateNextTask(TaskData taskData)
    {
        return new CycleWaypointWalkTask(taskData, _waypoints);
    }
}