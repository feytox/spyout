using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class CharacterAnimController : MonoBehaviour
{
    private const float FlashTime = 0.25f;
    
    private static readonly int IsWalking = Animator.StringToHash("isWalking");
    private static readonly int Attack = Animator.StringToHash("attack");
    private static readonly int FlashAmount = Shader.PropertyToID("_FlashAmount");
    
    [SerializeField]
    private AnimationCurve _flashCurve;
    
    protected Animator Animator;
    private SpriteRenderer _spriteRenderer;
    private Material _material;
    private float _currentFlashTime;
    
    private void Awake()
    {
        Animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _material = _spriteRenderer.material;
    }

    void Update()
    {
        if (_currentFlashTime <= 0)
            return;

        _currentFlashTime = Mathf.Max(0, _currentFlashTime - Time.deltaTime);
        var progress = _flashCurve.Evaluate(FlashTime - _currentFlashTime);
        _material.SetFloat(FlashAmount, progress);
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
}