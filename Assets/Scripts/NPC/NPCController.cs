using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(NPCTaskManager))]
public class NPCController : MonoBehaviour
{   
    [SerializeField] private float _movementSpeed = 120f;
    
    public Vector2? CurrentTarget { get; set; }
    
    private Rigidbody2D _body;

    void Start()
    {
        _body = GetComponent<Rigidbody2D>();
    }

    public void MoveInDirection(Vector2 moveVec)
    {
        // _body.AddForce(moveVec * _movementSpeed, ForceMode2D.Force);
        _body.linearVelocity = moveVec * (_movementSpeed * Time.fixedDeltaTime);
    }
}