using System;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    public float Health = 100;
    public float MaxHealth = 100;

    // event parameters can be extended for new features

    public event Action<float> OnDamageTaken;
    public event Action<float> OnHealed;
    public event Action OnDeath;

    public bool IsDead => Health == 0;

    public void TakeDamage(float amount)
    {
        Health = Mathf.Clamp(Health - amount, 0, MaxHealth);
        OnDamageTaken?.Invoke(amount);
        if (IsDead)
            OnDeath?.Invoke();
    }

    public void Heal(float amount)
    {
        Health = Mathf.Clamp(Health + amount, 0, MaxHealth);
        OnHealed?.Invoke(amount);
    }
}
