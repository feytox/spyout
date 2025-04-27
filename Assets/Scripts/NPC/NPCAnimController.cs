using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class NPCAnimController : MonoBehaviour
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
        var walkValue = !Mathf.Approximately(movementInput.sqrMagnitude, 0.0f);
        animator.SetBool(walk, walkValue);
    }
}