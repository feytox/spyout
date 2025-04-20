using UnityEngine;

// TODO улучшить переход анимаций
public class PlayerAnimController : MonoBehaviour
{
    private Animator _animator;
    private const string WalkParam = "walk";
    
    void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    
    public void UpdateMovementAnimation(Vector2 movementInput)
    {
        var walkValue = (movementInput == Vector2.zero) ? 0f : 1f;
        _animator.SetFloat(WalkParam, walkValue);
    }
}