using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(Slider))]
public abstract class HealthBarComponent : MonoBehaviour
{
    protected abstract HealthController HealthController { get; }
    private Slider _slider;

    private void Start()
    {
        _slider = GetComponent<Slider>();
        _slider.maxValue = HealthController.MaxHealth;
        _slider.value = HealthController.MaxHealth;
        HealthController.OnHealthChange += OnHealthChange;
    }

    private void OnHealthChange(float health)
    {
        _slider.value = health;
    }
}