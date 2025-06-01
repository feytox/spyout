using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(CanvasGroup))]
public class DamageFlashController : MonoBehaviour
{
    [SerializeField] private AnimationCurve _flashCurve;
    [SerializeField] private float _flashTime;

    private CanvasGroup _canvasGroup;
    private float _currentFlashTime;

    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        PlayerController.GetInstance().OnDamageTaken += OnDamage;
    }

    private void Update()
    {
        if (_currentFlashTime <= 0)
            return;

        _currentFlashTime = Mathf.Max(0, _currentFlashTime - Time.deltaTime);
        _canvasGroup.alpha = _flashCurve.Evaluate(_flashTime - _currentFlashTime);
    }

    private void OnDamage() => _currentFlashTime = _flashTime;
}