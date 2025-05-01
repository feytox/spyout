using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class NPCAnimController : MonoBehaviour
{
    private static readonly int IsWalking = Animator.StringToHash("isWalking");
    private static readonly int Attack = Animator.StringToHash("attack");
    private static readonly int IsDead = Animator.StringToHash("isDead");
    
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    public void UpdateMovementAnimation(Vector2 movementInput)
    {
        _spriteRenderer.flipX = movementInput.x < 0.0f;
        var isWalking = movementInput != Vector2.zero;
        _animator.SetBool(IsWalking, isWalking);
    }

    public void TriggerAttack() => _animator.SetTrigger(Attack);

    public void OnDeath() => _animator.SetBool(IsDead, true);
}