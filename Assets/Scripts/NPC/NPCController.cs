#nullable enable
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(NPCInventoryController))]
public class NPCController : MonoBehaviour, ICharacter
{
    private const float TargetMinimumSqrDistance = 0.2f;

    [SerializeField] private float _movementSpeed = 4f;

    public Rigidbody2D? Body { get; private set; }
    public InventoryController? Inventory { get; private set; }
    public Vector2 Position => transform.position;

    private NPCAnimController? _animController;
    private HealthController? _healthController;

    void Start()
    {
        Body = GetComponent<Rigidbody2D>();
        _animController = GetComponentInChildren<NPCAnimController>();
        _healthController = GetComponentInChildren<HealthController>();
        Inventory = GetComponent<InventoryController>();
    }

    #region ICharacter

    public void OnTargetAttacked<T>(T target) where T : IDamageable, IPositionProvider
    {
        _animController?.TriggerAttack(target.Position - Position);
    }

    HealthController? ICharacter.Health => _healthController;

    public void OnDeath<T>(T attacker) where T : IDamageable, IPositionProvider
    {
        _animController?.OnDeath();
    }

    public void OnDamage<T>(T attacker) where T : IDamageable, IPositionProvider
    {
        Debug.Log("NPC Damage :D");
        this.ApplyKnockback(attacker);
        _animController?.OnDamage();
    }

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
            return true;

        MoveInDirection(moveVec.normalized);
        return false;
    }

    /// <summary>
    /// Перемещает NPC в указанном направлении.
    /// </summary>
    /// <param name="moveVec">Вектор направления движения.</param>
    private void MoveInDirection(Vector2 moveVec)
    {
        Body!.linearVelocity = moveVec * _movementSpeed;
        _animController?.UpdateMovementAnimation(moveVec);
    }

    public bool IsTargetReached(IPositionProvider target, float sqrPrecision = TargetMinimumSqrDistance)
    {
        return (target.Position - Position).sqrMagnitude <= sqrPrecision;
    }

    #endregion
}