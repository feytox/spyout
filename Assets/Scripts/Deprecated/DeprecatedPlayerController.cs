using System;
using UnityEngine;

[Obsolete]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class DeprecatedPlayerController : MonoBehaviour
{
    public float MovementSpeed = 3f;
    public Transform BodyTransform { get; private set; }

    private Vector2 movement = Vector2.zero;
    private new Rigidbody2D rigidbody;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        BodyTransform = transform;
    }

    void Start()
    {
        var playerManager = GetComponentInParent<DeprecatedPlayerManager>();
        Debug.Assert(playerManager != null);

        playerManager.MoveAction.performed += ctx => movement = ctx.ReadValue<Vector2>(); // Already normalized
        playerManager.MoveAction.canceled += ctx => movement = Vector2.zero;
    }

    void FixedUpdate()
    {
        // it would be hell in 3D
        // rigidbody.linearDamping = 0f;
        rigidbody.AddForce(movement * MovementSpeed, ForceMode2D.Force);
    }
}
