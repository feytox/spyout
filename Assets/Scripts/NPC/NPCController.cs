#nullable enable
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(NpcInventoryController))]
public class NpcController : MonoBehaviour, ICharacter, IWalker
{
    private const float TargetMinimumSqrDistance = 0.01f;
    private const float DeathTime = 0.7f;

    [SerializeField] private float _movementSpeed = 4f;
    [SerializeField] private float _attackDamage = 10f;
    [SerializeField] private float _attackRadius = 1.25f;
    [SerializeField] private float _attackCooldownSeconds = 1f;
    [SerializeField] private int _maxPathLength = 50;

    public Rigidbody2D? Body { get; private set; }
    public HealthController? Health { get; private set; }
    public CharacterSoundController? Sounds { get; private set; }
    public IDoorPermission? DoorPermission { get; private set; }

    public Vector2 Position => transform.position;
    public float CurrentDamage => _attackDamage;
    public float AttackRadius => _attackRadius;
    public float AttackCooldown => _attackCooldownSeconds;
    public int MaxPathLength => _maxPathLength;

    private NpcAnimController? _animController;
    private DropOnDeathComponent? _dropOnDeath;

    private void Awake()
    {
        Body = GetComponent<Rigidbody2D>();
        _animController = GetComponentInChildren<NpcAnimController>();
        Health = GetComponentInChildren<HealthController>();
        GetComponent<InventoryController>();
        Sounds = GetComponentInChildren<CharacterSoundController>();
        DoorPermission = GetComponent<IDoorPermission>();
        _dropOnDeath = GetComponentInChildren<DropOnDeathComponent>();
    }

    private void FixedUpdate()
    {
        _animController?.UpdateMovementAnimation(Body!.linearVelocity);
        Sounds?.UpdateIdleSound();
    }

    #region ICharacter

    public void OnTargetAttacked<T>(T target) where T : IDamageable, IPositionProvider
    {
        _animController?.TriggerAttack(target.Position - Position);
        Sounds?.PlaySound(CharacterSoundType.Attack);
    }

    public void OnDeath<T>(T attacker) where T : IDamageable, IPositionProvider
    {
        _animController?.OnDeath();
        Sounds?.PlaySound(CharacterSoundType.Death);
        _dropOnDeath?.OnDeath(0.9f * DeathTime);
        Destroy(gameObject, DeathTime);
    }

    public void OnDamage<T>(T attacker) where T : IDamageable, IPositionProvider
    {
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
        Sounds?.UpdateMovement(Position, moveVec);
    }

    public bool IsTargetReached(IPositionProvider target, float sqrPrecision = TargetMinimumSqrDistance)
    {
        return (target.Position - Position).sqrMagnitude <= sqrPrecision;
    }

    #endregion
}