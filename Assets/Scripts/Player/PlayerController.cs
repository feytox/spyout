using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(PlayerInputController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 120f;

    private PlayerAnimController _animController;
    private PlayerInputController _inputs;
    private Rigidbody2D _body;

    public static PlayerInputController Inputs => GetInstance()._inputs;

    void Awake()
    {
        Debug.Assert(
            _singleton == null,
            $"{gameObject.name} tried to awake {nameof(PlayerController)} second time!"
        );
        _singleton = this;

        _inputs = GetComponent<PlayerInputController>();
        _body = GetComponent<Rigidbody2D>();
        _animController = transform.GetChild(0).GetComponent<PlayerAnimController>();
    }

    private void FixedUpdate()
    {
        _body.AddForce(_inputs.Movement * _movementSpeed, ForceMode2D.Force);
        _animController.UpdateMovementAnimation(_inputs.Movement);
    }

    #region Singleton

    private static PlayerController _singleton;

    private static PlayerController GetInstance()
    {
        Debug.Assert(
            _singleton != null,
            $"Tried to access {nameof(PlayerController)} before it was initialized!"
        );
        return _singleton;
    }

    #endregion
}