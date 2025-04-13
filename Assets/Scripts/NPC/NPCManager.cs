using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class NPCManager : MonoBehaviour
{
    public float MovementSpeed = 0.7f;
    public float Health = 100f;
    private new Rigidbody2D rigidbody;
    private new Collider2D collider;

    [SerializeField]
    private Vector2 SpriteOffset;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }

    private void MoveTo(Vector2 goal)
    {
        // Pathfinding implementation
    }

    // May be changed later to be more flexible or used as a base
    private void MoveInDirection(Vector2 direction)
    {
        rigidbody.AddForce(direction * MovementSpeed, ForceMode2D.Force);
    }
}
