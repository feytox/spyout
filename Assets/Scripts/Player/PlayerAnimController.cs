using UnityEngine;

// TODO улучшить переход анимаций
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class PlayerAnimController : MonoBehaviour
{
    private static readonly int walk = Animator.StringToHash(WalkParam);
    private const string WalkParam = "walk";
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    public void UpdateMovementAnimation(Vector2 movementInput)
    {
        spriteRenderer.flipX = movementInput.x < 0.0f;
        var walkValue = movementInput == Vector2.zero ? 0f : 1f;
        animator.SetFloat(walk, walkValue);
    }
}