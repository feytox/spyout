using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 4;
    
    private Rigidbody2D rb;
    private Vector2 movement;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = movement * moveSpeed;
    }

    private void OnMove(InputValue inputValue)
    {
        movement = inputValue.Get<Vector2>();
    }
}
