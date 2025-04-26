using JetBrains.Annotations;
using Utils;

/// <summary>
/// Задача атаки цели до тех пор, пока цель в зоне поражения
/// </summary>
/// <typeparam name="T"></typeparam>
public class AttackTask<T> : NPCTask where T : IDamageable, IPositionProvider
{
    // TODO: move from const to somewhere else
    private const float DamageAmount = 10;
    private const float AttackCooldown = 1f;
    private const float RangeCheckCooldown = 0.3f;
    
    private readonly T _target;
    private readonly Cooldown _attackCooldown = new(AttackCooldown);
    private readonly Cooldown _rangeCooldown = new(RangeCheckCooldown);
    
    public AttackTask([NotNull] TaskData taskData, T target) : base(taskData)
    {
        _target = target;
    }

    public static AttackTask<PlayerController> OfPlayer(TaskData taskData)
    {
        return new AttackTask<PlayerController>(taskData, PlayerController.GetInstance());
    }

    public override bool Step()
    {
        if (_rangeCooldown.ResetIfExpired() && (_target == null || !NPC.IsInAttackRange(_target)))
            return true;
        
        if (!_attackCooldown.ResetIfExpired())
            return false;
        
        return _target == null || !NPC.TryAttack(_target, DamageAmount);
    }

    public override NPCTask CreateNextTask(TaskData taskData) => null;
}