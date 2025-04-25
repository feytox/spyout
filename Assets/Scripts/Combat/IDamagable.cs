public interface IDamageable
{
    public void Damage(float amount);

    public bool CanTakeDamage(IDamageable attacker);
}

public static class DamageableExtensions
{
    private const float AttackSqrRange = 0.75f * 0.75f;
    
    public static bool TryAttack<T>(this T attacker, T target, float amount) where T : IDamageable, IPositionProvider
    {
        if (!attacker.IsInAttackRange(target))
            return false;

        if (!target.CanTakeDamage(attacker))
            return false;
        
        target.Damage(amount);
        return true;
    }

    public static bool IsInAttackRange<T>(this T attacker, T target) where T : IDamageable, IPositionProvider
    {
        return (attacker.Position - target.Position).sqrMagnitude <= AttackSqrRange;
    }
}