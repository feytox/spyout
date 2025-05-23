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

    public bool Damage(float amount)
    {
        _currentHealth = Mathf.Max(_currentHealth - amount, 0);
        OnHealthChange?.Invoke(_currentHealth);
        return _currentHealth > 0;
    }

    public void Heal(float amount)
    {
        _currentHealth = Mathf.Clamp(_currentHealth + amount, 0, _maxHealth);
        OnHealthChange?.Invoke(_currentHealth);
    }
}