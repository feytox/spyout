using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(Slider))]
public class PlayerHealthBarController : MonoBehaviour
{
    void Start()
    {
        var slider = GetComponent<Slider>();
        var healthController = (PlayerController.GetInstance() as ICharacter).HealthController;
        
        slider.maxValue = healthController.MaxHealth;
        slider.value = healthController.MaxHealth;
        healthController.OnHealthChange += health => slider.value = health;
    }
}