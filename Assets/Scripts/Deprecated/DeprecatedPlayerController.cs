using System;
using UnityEngine;

[Obsolete]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class DeprecatedPlayerController : MonoBehaviour
{
    public float MovementSpeed = 3f;
    public Transform BodyTransform { get; private set; }

    private Vector2 _movement = Vector2.zero;
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        BodyTransform = transform;
    }

    private void Start()
    {
        var playerManager = GetComponentInParent<DeprecatedPlayerManager>();
        Debug.Assert(playerManager != null);

        playerManager.MoveAction.performed += ctx => _movement = ctx.ReadValue<Vector2>(); // Already normalized
        playerManager.MoveAction.canceled += _ => _movement = Vector2.zero;
    }

    private void FixedUpdate()
    {
        // it would be hell in 3D
        // rigidbody.linearDamping = 0f;
        _rigidbody.AddForce(_movement * MovementSpeed, ForceMode2D.Force);
    }
}
