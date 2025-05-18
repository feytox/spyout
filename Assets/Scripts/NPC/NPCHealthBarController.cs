using Unity.VisualScripting;
using UnityEngine;

public class NPCHealthBarController : MonoBehaviour
{
    [SerializeField] private NPCController _npc;
    [SerializeField] private AnimationCurve _flashCurve;
    [SerializeField] private float _flashTime = 0.25f;
    [SerializeField] private Gradient _backGroundGradient;
    [SerializeField] private Gradient _fillGradient;
    [SerializeField] private Gradient _borderGradient;
    [SerializeField] private SpriteRenderer _background;
    [SerializeField] private SpriteRenderer _fill;
    [SerializeField] private SpriteRenderer _border;

    private float _maxFillWidth;
    private float _maxHealth;
    private float _currentFlashTime;

    void Start()
    {
        _maxFillWidth = _fill.size.x;
        _npc.Health!.OnHealthChange += OnHealthChange;
        _maxHealth = _npc.Health.MaxHealth;
    }

    void Update()
    {
        if (_currentFlashTime <= 0)
        {
            ToggleSprites(false);
            return;
        }

        _currentFlashTime = Mathf.Max(0, _currentFlashTime - Time.deltaTime);
        var alpha = _flashCurve.Evaluate(_flashTime - _currentFlashTime);
        ApplyAlpha(_background, alpha);
        ApplyAlpha(_fill, alpha);
        ApplyAlpha(_border, alpha);
    }


    private void OnHealthChange(float health)
    {
        var progress = health / _maxHealth;
        _fill.size = _fill.size.WithX(_maxFillWidth * progress);
        _currentFlashTime = _flashTime;
        ToggleSprites(true);
        ApplyGradient(_background, _backGroundGradient, progress);
        ApplyGradient(_fill, _fillGradient, progress);
        ApplyGradient(_border, _borderGradient, progress);
    }

    // TODO: move to external class
    private void ApplyGradient(SpriteRenderer sprite, Gradient gradient, float progress)
    {
        sprite.color = gradient.Evaluate(progress);
    }

    private void ApplyAlpha(SpriteRenderer sprite, float alpha)
    {
        sprite.color = sprite.color.WithAlpha(alpha);
    }

    private void ToggleSprites(bool value)
    {
        if (!value && _currentFlashTime <= -100)
            return;

        _background.enabled = value;
        _fill.enabled = value;
        _border.enabled = value;
        if (!value)
            _currentFlashTime = -1000;
    }
}