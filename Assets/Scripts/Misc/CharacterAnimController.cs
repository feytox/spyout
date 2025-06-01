using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class CharacterAnimController : MonoBehaviour
{
    protected static readonly int Fade = Shader.PropertyToID("_Fade");
    private static readonly int IsWalking = Animator.StringToHash("isWalking");
    private static readonly int Attack = Animator.StringToHash("attack");
    private static readonly int FlashAmount = Shader.PropertyToID("_FlashAmount");
    private const float MovementEpsilon = 0.005f;

    [SerializeField] private AnimationCurve _flashCurve;
    [SerializeField] private float _flashTime = 0.25f;
    [SerializeField] private AnimationCurve _dissolveCurve;
    [SerializeField] private float _dissolveTime = 1f;

    protected Animator Animator;
    protected Material Material;
    private SpriteRenderer _spriteRenderer;
    private float _currentFlashTime;
    private float _currentDissolveTime;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Material = _spriteRenderer.material;
    }

    private void Update()
    {
        UpdateFlash();
        UpdateDissolve();
    }

    private void UpdateFlash()
    {
        if (_currentFlashTime <= 0)
            return;

        _currentFlashTime = Mathf.Max(0, _currentFlashTime - Time.deltaTime);
        var progress = _flashCurve.Evaluate(_flashTime - _currentFlashTime);
        Material.SetFloat(FlashAmount, progress);
    }

    private void UpdateDissolve()
    {
        if (_currentDissolveTime <= 0)
            return;

        _currentDissolveTime = Mathf.Max(0, _currentDissolveTime - Time.deltaTime);
        var progress = _dissolveCurve.Evaluate(_dissolveTime - _currentDissolveTime);
        Material.SetFloat(Fade, progress);
    }

    public void UpdateMovementAnimation(Vector2 movementInput)
    {
        _spriteRenderer.flipX = movementInput.x < 0.0f;
        var isWalking = movementInput.sqrMagnitude > MovementEpsilon;
        Animator.SetBool(IsWalking, isWalking);
    }

    public void TriggerAttack(Vector2 attackVec)
    {
        _spriteRenderer.flipX = attackVec.x < 0.0f;
        Animator.SetTrigger(Attack);
    }

    public void OnDamage() => _currentFlashTime = _flashTime;

    public virtual void OnDeath() => _currentDissolveTime = _dissolveTime;
}