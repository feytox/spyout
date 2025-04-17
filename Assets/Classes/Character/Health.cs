#nullable enable
using System;
using UnityEngine;

namespace Classes.Character
{
    public class Health : MonoBehaviour
    {
        public float CurrentHealth { get; private set; } = 100;
        public float MaxHealth { get; private set; } = 100;

        public event Action? OnDeath;
        public event Action<float>? OnHealthChanged;

        public bool IsDead => CurrentHealth == 0;

        public void TakeDamage(float amount)
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth - amount, 0, MaxHealth);
            OnHealthChanged?.Invoke(CurrentHealth);
            if (IsDead)
                OnDeath?.Invoke();
        }

        public void Heal(float amount)
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, MaxHealth);
            OnHealthChanged?.Invoke(amount);
        }

        public void SetMaxHealth(float value)
        {
            Debug.Assert(value > 0, "Max health must be greater than 0");
            MaxHealth = value;
            CurrentHealth = Mathf.Min(CurrentHealth, MaxHealth);
        }
    }
}
