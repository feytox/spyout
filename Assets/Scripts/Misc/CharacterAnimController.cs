using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class CharacterAnimController : MonoBehaviour
{
    public const float DissolveTime = 1f;
    private const float FlashTime = 0.25f;

    private static readonly int IsWalking = Animator.StringToHash("isWalking");
    private static readonly int Attack = Animator.StringToHash("attack");
    private static readonly int FlashAmount = Shader.PropertyToID("_FlashAmount");
    private static readonly int Fade = Shader.PropertyToID("_Fade");
    
    [SerializeField]
    private AnimationCurve _flashCurve;
    [SerializeField]
    private AnimationCurve _dissolveCurve;
    
    protected Animator Animator;
    private SpriteRenderer _spriteRenderer;
    private Material _material;
    private float _currentFlashTime;
    private float _currentDissolveTime;
    
    private void Awake()
    {
        Animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _material = _spriteRenderer.material;
    }

    void Update()
    {
        UpdateFlash();
        UpdateDissolve();
    }

    private void UpdateFlash()
    {
        if (_currentFlashTime <= 0)
            return;

        _currentFlashTime = Mathf.Max(0, _currentFlashTime - Time.deltaTime);
        var progress = _flashCurve.Evaluate(FlashTime - _currentFlashTime);
        _material.SetFloat(FlashAmount, progress);
    }

    private void UpdateDissolve()
    {
        if (_currentDissolveTime <= 0)
            return;

        _currentDissolveTime = Mathf.Max(0, _currentDissolveTime - Time.deltaTime);
        var progress = _dissolveCurve.Evaluate(DissolveTime - _currentDissolveTime);
        _material.SetFloat(Fade, progress);
    }

    public void UpdateMovementAnimation(Vector2 movementInput)
    {
        _spriteRenderer.flipX = movementInput.x < 0.0f;
        var isWalking = movementInput != Vector2.zero;
        Animator.SetBool(IsWalking, isWalking);
    }

    public void TriggerAttack(Vector2 attackVec)
    {
        _spriteRenderer.flipX = attackVec.x < 0.0f;
        Animator.SetTrigger(Attack);
    }

    public void OnDamage() => _currentFlashTime = FlashTime;

    public virtual void OnDeath() => _currentDissolveTime = DissolveTime;
}