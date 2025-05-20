using System;
using System.Linq;
using UnityEngine;
using Utils;

[Obsolete]
[RequireComponent(typeof(Rigidbody2D))]
public class TestNPCController : MonoBehaviour, IWalker
{
    public GameObject target;
    public float MovementSpeed = 4f;

    private new Rigidbody2D rigidbody;
    private int currentTarget;
    private Vector2[] pathToTarget;
    private Cooldown walkCooldown;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        pathToTarget = GridController.FindPath(this, rigidbody.transform.position, target.transform.position)
            .Select(cellPos => cellPos.ToCellCenter())
            .ToArray();
        walkCooldown = new Cooldown(5);
    }

    private void FixedUpdate()
    {
        if (!walkCooldown.IsExpired)
            return;
        
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

    public IDoorPermission DoorPermission => null;
}