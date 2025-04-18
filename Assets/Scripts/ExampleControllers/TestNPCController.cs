using System;
using System.Linq;
using UnityEngine;

[Obsolete]
[RequireComponent(typeof(Rigidbody2D))]
public class TestNPCController : MonoBehaviour
{
    public GameObject target;
    public float MovementSpeed = 4f;

    private new Rigidbody2D rigidbody;
    private int currentTarget = 0;
    private Vector2[] pathToTarget;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        pathToTarget = GridController
            .FindPath(gameObject, rigidbody.transform.position, target.transform.position)
            .Select(cellPos => cellPos.ToCenterPos())
            .ToArray();
    }

    void FixedUpdate()
    {
        if (currentTarget >= pathToTarget.Length)
        {
            rigidbody.linearVelocity = Vector2.zero;
            return;
        }

        var current = pathToTarget[currentTarget];
        var moveVec = current - rigidbody.position;
        if (moveVec.sqrMagnitude <= 0.1)
        {
            currentTarget++;
            return;
        }

        rigidbody.linearVelocity = moveVec.normalized * MovementSpeed;
    }
}
