#nullable enable
using UnityEngine;
using Utils;

/// <summary>
/// Задача для NPC, реализующая патрулирование цели по заданным точкам (waypoints).
/// </summary>
public class PatrolTask<T> : WaypointWalkTask where T : IDamageable, IPositionProvider
{
    private const float TargetSqrDistance = 25f;
    private const float TargetCheckCooldown = 1f;

    private readonly Cooldown _targetCheckCooldown = new(TargetCheckCooldown);
    private readonly T _target;

    private PatrolTask(TaskData taskData, T target, Vector2Int[] waypoints)
        : base(taskData, waypoints)
    {
        _target = target;
    }

    public static PatrolTask<PlayerController> OfPlayer(TaskData taskData, Vector2Int[] waypoints)
    {
        var player = PlayerController.GetInstance();
        return new PatrolTask<PlayerController>(taskData, player, waypoints);
    }

    // TODO: здесь есть "не баг, а фича" - нпс могут чувствовать игрока через стены. Возможно, стоит убрать
    public override bool Step()
    {
        if (_targetCheckCooldown.ResetIfExpired() && Npc.IsTargetReached(_target, TargetSqrDistance))
            return true;

        return base.Step();
    }

    public override NpcTask? CreateNextTask(TaskData taskData) => null;
}