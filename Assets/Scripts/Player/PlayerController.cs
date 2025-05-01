using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(PlayerInputController))]
[RequireComponent(typeof(PlayerInventoryController))]
public class PlayerController : MonoBehaviour, ICharacter
{
    [SerializeField] private float _movementSpeed = 120f;

    private PlayerAnimController _animController;
    private PlayerInputController _inputs;
    private PlayerInventoryController _playerInventory;
    private HealthController _healthController;

    public static PlayerInputController Inputs => GetInstance()._inputs;
    public static Inventory Inventory => GetInstance()._playerInventory.Inventory;

    public Rigidbody2D Body { get; private set; }
    public Vector2 Position => transform.position;

    void Awake()
    {
        Debug.Assert(
            _singleton == null,
            $"{gameObject.name} tried to awake {nameof(PlayerController)} second time!"
        );
        _singleton = this;

        _inputs = GetComponent<PlayerInputController>();
        Body = GetComponent<Rigidbody2D>();
        _playerInventory = GetComponent<PlayerInventoryController>();
        _animController = GetComponentInChildren<PlayerAnimController>();
        _healthController = GetComponentInChildren<HealthController>();

        if (_animController is not null)
            _inputs.MovementUpdate += _animController.UpdateMovementAnimation;
    }

    private void FixedUpdate()
    {
        Body.AddForce(_inputs.Movement * _movementSpeed, ForceMode2D.Force);
    }

    #region ICharacter

    HealthController ICharacter.HealthController => _healthController;

    public void OnTargetAttacked<T>(T attacker) where T : IDamageable, IPositionProvider
    {
        _animController?.TriggerAttack();
    }

    public void OnDeath<T>(T attacker) where T : IDamageable, IPositionProvider
    {
        // TODO: add smth after death
        _inputs.enabled = false;
    }

    public void OnDamage<T>(T attacker) where T : IDamageable, IPositionProvider
    {
        // TODO: add damage animation
        this.ApplyKnockback(attacker);
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