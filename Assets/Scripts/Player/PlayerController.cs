using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(PlayerInputController))]
[RequireComponent(typeof(PlayerInventoryController))]
public class PlayerController : MonoBehaviour, IDamageable, IPositionProvider
{
    [SerializeField] private float _movementSpeed = 120f;

    private PlayerAnimController _animController;
    private PlayerInputController _inputs;
    private Rigidbody2D _body;
    private PlayerInventoryController _playerInventory;

    public static PlayerInputController Inputs => GetInstance()._inputs;
    public static Inventory Inventory => GetInstance()._playerInventory.Inventory;
    public static GameObject GameObject => GetInstance().gameObject;
    
    public Vector2 Position => transform.position;

    void Awake()
    {
        Debug.Assert(
            _singleton == null,
            $"{gameObject.name} tried to awake {nameof(PlayerController)} second time!"
        );
        _singleton = this;

        _inputs = GetComponent<PlayerInputController>();
        _body = GetComponent<Rigidbody2D>();
        _playerInventory = GetComponent<PlayerInventoryController>();
        _animController = GetComponentInChildren<PlayerAnimController>();
    }

    private void FixedUpdate()
    {
        _body.AddForce(_inputs.Movement * _movementSpeed, ForceMode2D.Force);
        _animController?.UpdateMovementAnimation(_inputs.Movement); // TODO: refactor
    }
    
    public void Damage(float amount)
    {
        Debug.Log($"Player taken {amount} damage");
    }

    public bool CanTakeDamage(IDamageable attacker) => true;

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