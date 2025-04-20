using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
public class NPCController : MonoBehaviour
{   
    [SerializeField] private float _movementSpeed = 120f;

    private const float TargetMinimumSqrDistance = 0.25f;
    
    public Vector2? CurrentTarget { get; set; }
    
    private Rigidbody2D _body;

    void Start()
    {
        _body = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (CurrentTarget is null)
            return;

        var moveDirection = CurrentTarget.Value - (Vector2)transform.position;
        if (moveDirection.sqrMagnitude < TargetMinimumSqrDistance)
        {
            CurrentTarget = null;
            return;
        }
        
        _body.AddForce(moveDirection.normalized * _movementSpeed, ForceMode2D.Force);
    }
}