using UnityEngine;

public interface ICharacter : IDamageable, IPositionProvider
{   
    public Rigidbody2D Body { get; }
    public HealthController Health { get; }
    public InventoryController Inventory { get; }
    
    public void OnDeath<T>(T attacker) where T : IDamageable, IPositionProvider;

    public void OnDamage<T>(T attacker) where T : IDamageable, IPositionProvider;

    void IDamageable.Damage<T>(T attacker, float amount)
    {
        var isDead = !Health.Damage(amount);
        OnDamage(attacker);
        if (isDead)
            OnDeath(attacker);
    }

    float IDamageable.CurrentDamage => (Inventory.ActiveItem?.Item.Damage).GetValueOrDefault();
    bool IPositionProvider.IsDead => Health.IsDead;
    bool IDamageable.CanTakeDamage(IDamageable attacker) => !IsDead;
}

public static class CharacterExtensions
{
    private const float KnockbackModifier = 5f;
    
    public static void ApplyKnockback<T>(this ICharacter character, T attacker) where T : IDamageable, IPositionProvider
    {
        var attackVec = character.Position - attacker.Position;
        attackVec.Normalize();
        
        character.Body.AddForce(attackVec * KnockbackModifier, ForceMode2D.Impulse);
    }
}