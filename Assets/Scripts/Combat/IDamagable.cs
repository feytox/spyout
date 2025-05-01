public interface IDamageable
{
    /// <summary>
    /// Наносит урон объекту.
    /// </summary>
    /// <remarks>Лучше использовать <see cref="DamageableExtensions.TryAttack"/></remarks>
    /// <param name="attacker">Атакующий</param>
    /// <param name="amount">Величина урона</param>
    public void Damage<T>(T attacker, float amount) where T : IDamageable, IPositionProvider;
    
    /// <summary>
    /// Вызывается после совершения атаки по цели
    /// </summary>
    public void OnTargetAttacked<T>(T attacker) where T : IDamageable, IPositionProvider;
    
    /// <summary>
    /// Проверяет, может ли объект получить урон от указанного атакующего без учёта расстояния.
    /// </summary>
    /// <param name="attacker">Атакующий объект.</param>
    /// <returns>True, если объект может получить урон, иначе false.</returns>
    public bool CanTakeDamage(IDamageable attacker);
}

public static class DamageableExtensions
{
    private const float AttackSqrRange = 1.25f * 1.25f;

    /// <summary>
    /// Пытается нанести урон цели, если она находится в радиусе атаки и может получить урон.
    /// </summary>
    /// <typeparam name="T1">Тип атакующего, реализующий <see cref="IDamageable"/> и <see cref="IPositionProvider"/>.</typeparam>
    /// <typeparam name="T2">Тип цели, реализующий <see cref="IDamageable"/> и <see cref="IPositionProvider"/>.</typeparam>
    /// <param name="attacker">Атакующий объект.</param>
    /// <param name="target">Цель атаки.</param>
    /// <param name="amount">Величина урона.</param>
    /// <returns>True, если атака успешна, иначе false.</returns>
    public static bool TryAttack<T1, T2>(this T1 attacker, T2 target, float amount)
        where T1 : IDamageable, IPositionProvider
        where T2 : IDamageable, IPositionProvider
    {
        if (!attacker.IsInAttackRange(target))
            return false;

        if (!target.CanTakeDamage(attacker))
            return false;

        target.Damage(attacker, amount);
        attacker.OnTargetAttacked(target);
        return true;
    }

    /// <summary>
    /// Проверяет, находится ли цель в радиусе атаки атакующего.
    /// </summary>
    /// <typeparam name="T1">Тип атакующего, реализующий <see cref="IDamageable"/> и <see cref="IPositionProvider"/>.</typeparam>
    /// <typeparam name="T2">Тип цели, реализующий <see cref="IDamageable"/> и <see cref="IPositionProvider"/>.</typeparam>
    /// <param name="attacker">Атакующий объект.</param>
    /// <param name="target">Цель атаки.</param>
    /// <returns>True, если цель в радиусе атаки, иначе false.</returns>
    public static bool IsInAttackRange<T1, T2>(this T1 attacker, T2 target)
        where T1 : IDamageable, IPositionProvider
        where T2 : IDamageable, IPositionProvider
    {
        return (attacker.Position - target.Position).sqrMagnitude <= AttackSqrRange;
    }
}