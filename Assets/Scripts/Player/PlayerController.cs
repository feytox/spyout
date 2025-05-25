using System;
using UnityEngine;
using Utils;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(PlayerInputController))]
[RequireComponent(typeof(PlayerInventoryController))]
public class PlayerController : MonoBehaviour, ICharacter
{
    [SerializeField] private float _movementSpeed = 120f;
    [SerializeField] private float _attackRadius = 1f;
    [SerializeField] private LayerMask _attackLayer;
    [SerializeField] private float _attackCooldownSeconds = 0.25f;
    [SerializeField] private DeathMenuController _deathMenu;
    
    public event Action OnDamageTaken;

    public static PlayerInputController Inputs => GetInstance()._inputs;
    public InventoryController Inventory => _playerInventory;
    public PlayerInteractionDetector InteractionDetector { get; private set; }
    public CharacterSoundController Sounds { get; private set; }
    public Collider2D Collider { get; private set; }

    public Rigidbody2D Body { get; private set; }
    public Vector2 Position => transform.position;
    public float CurrentDamage => (Inventory.ActiveItem?.Item.Damage).GetValueOrDefault();
    public float AttackRadius => _attackRadius;
    public float AttackCooldown => _attackCooldownSeconds;

    private PlayerAnimController _animController;
    private PlayerInputController _inputs;
    private PlayerInventoryController _playerInventory;
    private HealthController _healthController;
    private Cooldown _attackCooldown;
    
    void Awake()
    {
        Debug.Assert(
            _singleton == null,
            $"{gameObject.name} tried to awake {nameof(PlayerController)} second time!"
        );
        _singleton = this;

        Body = GetComponent<Rigidbody2D>();
        _inputs = GetComponent<PlayerInputController>();
        _playerInventory = GetComponent<PlayerInventoryController>();
        _animController = GetComponentInChildren<PlayerAnimController>();
        _healthController = GetComponentInChildren<HealthController>();
        InteractionDetector = GetComponentInChildren<PlayerInteractionDetector>();
        Sounds = GetComponentInChildren<CharacterSoundController>();
        Collider = GetComponent<Collider2D>();

        if (_animController is not null)
            _inputs.MovementUpdate += _animController.UpdateMovementAnimation;
        
        _attackCooldown = new Cooldown(_attackCooldownSeconds);
        _inputs.Attack += () => AttackInRange();
    }

    void FixedUpdate()
    {
        Body.AddForce(_inputs.Movement * _movementSpeed, ForceMode2D.Force);
        Sounds?.UpdateMovement(Position, _inputs.Movement);
        Sounds?.UpdateIdleSound();
    }

    public bool AttackInRange()
    {
        if ((this as ICharacter).CurrentDamage <= 0 || !_attackCooldown.ResetIfExpired())
            return false;

        var targetCollider = Physics2D.OverlapCircle(Position, _attackRadius, _attackLayer);
        if (targetCollider is null)
        {
            _animController?.TriggerAttack(_inputs.Movement);
            var missSound = Inventory.ActiveItem?.Item.AttackMissSound;
            if (missSound is not null)
                Sounds?.PlayRandomSound(missSound);
            
            return false;
        }

        var target = targetCollider.gameObject.GetComponent<ICharacter>();
        return target is not null && this.TryAttack(target);
    }

    #region ICharacter

    HealthController ICharacter.Health => _healthController;

    public void OnTargetAttacked<T>(T target) where T : IDamageable, IPositionProvider
    {
        _animController?.TriggerAttack(target.Position - Position);
        var attackSound = Inventory.ActiveItem?.Item.AttackSound;
        if (attackSound is not null)
            Sounds?.PlayRandomSound(attackSound);
    }

    public void OnDeath<T>(T attacker) where T : IDamageable, IPositionProvider
    {
        _inputs.DisablePlayer();
        Sounds.PlaySound(CharacterSoundType.Death);
        _animController?.OnDeath();
        _deathMenu?.TriggerDeath();
    }

    public void OnDamage<T>(T attacker) where T : IDamageable, IPositionProvider
    {
        this.ApplyKnockback(attacker);
        _animController?.OnDamage();
        OnDamageTaken?.Invoke();
    }

    #endregion

    #region Singleton

    private static PlayerController _singleton;

    public static PlayerController GetInstance()
    {
        Debug.Assert(
            _singleton != null,
            $"Tried to access {nameof(PlayerController)} before it was initialized!"
        );
        return _singleton;
    }

    #endregion
}