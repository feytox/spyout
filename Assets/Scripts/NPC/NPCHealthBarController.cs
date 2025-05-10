using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class NPCHealthBarController : HealthBarComponent
{
    [SerializeField] private NPCController _npc;
    [SerializeField] private AnimationCurve _flashCurve;
    [SerializeField] private float _flashTime = 0.25f;
    [SerializeField] private Gradient _backGroundGradient;
    [SerializeField] private Gradient _fillGradient;
    [SerializeField] private Gradient _borderGradient;
    [SerializeField] private Image _background;
    [SerializeField] private Image _fill;
    [SerializeField] private Image _border;

    private CanvasGroup _canvasGroup;
    private float _currentFlashTime;
    
    protected override HealthController HealthController => _npc.Health;

    protected override void OnStart()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        if (_currentFlashTime <= 0)
            return;

        _currentFlashTime = Mathf.Max(0, _currentFlashTime - Time.deltaTime);
        _canvasGroup.alpha = _flashCurve.Evaluate(_flashTime - _currentFlashTime);
    }

    protected override void OnHealthChange(float health)
    {
        base.OnHealthChange(health);
        _currentFlashTime = _flashTime;
        ApplyGradient(_background, _backGroundGradient, health);
        ApplyGradient(_fill, _fillGradient, health);
        ApplyGradient(_border, _borderGradient, health);
    }
    
    // TODO: move to external class
    private void ApplyGradient(Image image, Gradient gradient, float health)
    {
        var progress = health / HealthController.MaxHealth;
        image.color = gradient.Evaluate(progress);
    }
}