using System;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 100;
    [SerializeField] private float _currentHealth = 100;
    
    public event Action<float> OnHealthChange;
    
    public float MaxHealth => _maxHealth;
    public bool IsDead => _currentHealth <= 0;
    public bool IsMaxHealth => Mathf.Approximately(_currentHealth, _maxHealth);
    public float Health
    {
        get => _currentHealth;
        set
        {
            _currentHealth = value;
            OnHealthChange?.Invoke(_currentHealth);
        }
    }

    public bool Damage(float amount)
    {
        Health = Mathf.Max(_currentHealth - amount, 0);
        return _currentHealth > 0;
    }

    public void Heal(float amount)
    {
        Health = Mathf.Clamp(_currentHealth + amount, 0, _maxHealth);
    }
}