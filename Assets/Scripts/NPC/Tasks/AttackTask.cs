using JetBrains.Annotations;
using Utils;

public class AttackTask<T> : FollowTask<T> where T : IDamageable, IPositionProvider
{
    private readonly Cooldown _attackCooldown = new(0);


    private AttackTask([NotNull] TaskData taskData, T target, bool endWhenNoTarget)
        : base(taskData, target, endWhenNoTarget)
    {
    }

    public new static AttackTask<PlayerController> OfPlayer(TaskData taskData, bool endWhenNoTarget)
    {
        return new AttackTask<PlayerController>(taskData, PlayerController.GetInstance(), endWhenNoTarget);
    }

    protected override void OnTaskStart()
    {
        base.OnTaskStart();
        _attackCooldown.SetDuration(Npc.AttackCooldown);
        _attackCooldown.Reset();
    }

    public override bool Step()
    {
        var followCompleted = base.Step();
        if (NoTarget)
            return followCompleted;

        var attackCompleted = AttackStep();
        return followCompleted && attackCompleted;
    }

    private bool AttackStep()
    {
        if (!_attackCooldown.ResetIfExpired())
            return false;

        return Target == null || !Npc.TryAttack(Target);
    }
}