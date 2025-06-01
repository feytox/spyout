using JetBrains.Annotations;
using UnityEngine;

/// <summary>
/// Задача для NPC, реализующая пассивное перемещение по заданным точкам (waypoints).
/// Отличается от <see cref="WaypointWalkTask"/> тем, что NPC делает случайную паузу.
/// </summary>
public class PassiveWalkTask : WaypointWalkTask
{
    private const float MinWaitTime = 1f;
    private const float MaxWaitTime = 4f;

    private readonly Vector2Int[] _waypoints;
    private float? _cooldownTime;

    public PassiveWalkTask([NotNull] TaskData taskData, [NotNull] Vector2Int[] waypoints) : base(taskData, waypoints)
    {
        _waypoints = waypoints;
    }

    protected override bool CanWalkNext()
    {
        if (_cooldownTime is null)
        {
            _cooldownTime = Time.time + Random.Range(MinWaitTime, MaxWaitTime);
            return false;
        }

        if (Time.time < _cooldownTime)
            return false;

        _cooldownTime = null;
        return true;
    }

    public override NpcTask CreateNextTask(TaskData taskData)
    {
        return new PassiveWalkTask(taskData, _waypoints);
    }
}