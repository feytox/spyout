using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(Slider))]
public abstract class HealthBarComponent : MonoBehaviour
{
    protected abstract HealthController HealthController { get; }
    private Slider _slider;
    
    void Start()
    {
        _slider = GetComponent<Slider>();
        _slider.maxValue = HealthController.MaxHealth;
        _slider.value = HealthController.MaxHealth;
        HealthController.OnHealthChange += OnHealthChange;
        OnStart();
    }

    protected virtual void OnStart()
    {
    }

    protected virtual void OnHealthChange(float health)
    {
        _slider.value = health;
    }
}