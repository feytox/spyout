using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
public class NPCController : MonoBehaviour, IDamageable, IPositionProvider
{   
    public const float TargetMinimumSqrDistance = 0.2f;
    private NPCAnimController _animController;
    [SerializeField] private float _movementSpeed = 4f;
    public Vector2 Position => transform.position;
    private Rigidbody2D _body;

    void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _animController = GetComponentInChildren<NPCAnimController>();
    }

    void FixedUpdate()
    {
        _animController?.UpdateMovementAnimation(_body.linearVelocity);
    }
    
    #region IDamageable

    public void Damage(float amount)
    {
        Debug.Log($"Taken {amount} damage");
    }
    
    public bool CanTakeDamage(IDamageable attacker) => true;

    #endregion
    
    #region Movement

    /// <summary>
    /// Перемещает NPC к указанной цели с заданной точностью.
    /// </summary>
    /// <param name="target">Целевая позиция.</param>
    /// <returns>Возвращает true, если цель достигнута, иначе false.</returns>
    public bool MoveToTarget(Vector2 target)
    {
        var moveVec = target - (Vector2)transform.position;
        if (moveVec.sqrMagnitude <= TargetMinimumSqrDistance)
        {
            return true;
        }
        
        MoveInDirection(moveVec.normalized);
        return false;
    }
    
    /// <summary>
    /// Перемещает NPC в указанном направлении.
    /// </summary>
    /// <param name="moveVec">Вектор направления движения.</param>
    private void MoveInDirection(Vector2 moveVec)
    {
        _body.linearVelocity = moveVec * _movementSpeed;  
    } 

    #endregion
}