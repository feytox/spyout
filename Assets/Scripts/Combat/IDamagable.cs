public interface IDamageable
{
    public void Damage(float amount);

    public bool CanTakeDamage(IDamageable attacker);
}

public static class DamageableExtensions
{
    private const float AttackSqrRange = 0.75f * 0.75f;

    public static bool TryAttack<T1, T2>(this T1 attacker, T2 target, float amount) 
        where T1 : IDamageable, IPositionProvider
        where T2 : IDamageable, IPositionProvider
    {
        if (!attacker.IsInAttackRange(target))
            return false;

        if (!target.CanTakeDamage(attacker))
            return false;

        target.Damage(amount);
        return true;
    }

    public static bool IsInAttackRange<T1, T2>(this T1 attacker, T2 target) 
        where T1 : IDamageable, IPositionProvider
        where T2 : IDamageable, IPositionProvider
    {
        return (attacker.Position - target.Position).sqrMagnitude <= AttackSqrRange;
    }
}