using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    
    [Header("Key Bindings")]
    [SerializeField] private KeyCode upKey = KeyCode.W;
    [SerializeField] private KeyCode downKey = KeyCode.S;
    [SerializeField] private KeyCode leftKey = KeyCode.A;
    [SerializeField] private KeyCode rightKey = KeyCode.D; 
    [SerializeField] private KeyCode elseUpKey = KeyCode.UpArrow;
    [SerializeField] private KeyCode elseDownKey = KeyCode.DownArrow;
    [SerializeField] private KeyCode elseLeftKey = KeyCode.LeftArrow;
    [SerializeField] private KeyCode elseRightKey = KeyCode.RightArrow;

    private Rigidbody2D rb;
    private Vector2 movementInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;
    }

    private void Update()
    {
        movementInput = Vector2.zero;
        
        if (Input.GetKey(upKey) || Input.GetKey(elseUpKey)) movementInput.y += 1f;
        if (Input.GetKey(downKey) || Input.GetKey(elseDownKey)) movementInput.y -= 1f;
        if (Input.GetKey(leftKey) || Input.GetKey(elseLeftKey)) movementInput.x -= 1f;
        if (Input.GetKey(rightKey) || Input.GetKey(elseRightKey)) movementInput.x += 1f;

        if (movementInput.magnitude > 1f)
        {
            movementInput.Normalize();
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = movementInput * moveSpeed;
    }
}