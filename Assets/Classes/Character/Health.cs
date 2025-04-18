#nullable enable
using System;
using UnityEngine;

namespace Classes.Character
{
    [DisallowMultipleComponent]
    public class Health : MonoBehaviour
    {
        [SerializeField]
        private float _currentHealth = 100; // default
        public float CurrentHealth => _currentHealth;

        [SerializeField]
        private float _maxHealth = 100; // default
        public float MaxHealth
        {
            get => _maxHealth;
            set
            {
                Debug.Assert(value > 0, "Max health must be greater than 0");
                _maxHealth = value;
                _currentHealth = Mathf.Min(_currentHealth, _maxHealth);
                OnHealthChanged?.Invoke(_currentHealth);
            }
        }

        public event Action? OnDeath;
        public event Action<float>? OnHealthChanged;

        public bool IsDead => _currentHealth == 0;

        public void TakeDamage(float amount)
        {
            _currentHealth = Mathf.Clamp(_currentHealth - amount, 0, _maxHealth);
            OnHealthChanged?.Invoke(_currentHealth);
            if (IsDead)
                OnDeath?.Invoke();
        }

        public void Heal(float amount)
        {
            _currentHealth = Mathf.Clamp(_currentHealth + amount, 0, _maxHealth);
            OnHealthChanged?.Invoke(amount);
        }
    }
}
