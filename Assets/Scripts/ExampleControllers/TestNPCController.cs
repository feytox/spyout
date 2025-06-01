using System;
using System.Linq;
using UnityEngine;
using Utils;

[Obsolete]
[RequireComponent(typeof(Rigidbody2D))]
public class TestNpcController : MonoBehaviour, IWalker
{
    public GameObject target;
    public float MovementSpeed = 4f;

    private new Rigidbody2D _rigidbody;
    private int _currentTarget;
    private Vector2[] _pathToTarget;
    private Cooldown _walkCooldown;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _pathToTarget = GridController.FindPath(this, _rigidbody.transform.position, target.transform.position)
            .Select(cellPos => cellPos.ToCellCenter())
            .ToArray();
        _walkCooldown = new Cooldown(5);
    }

    private void FixedUpdate()
    {
        if (!_walkCooldown.IsExpired)
            return;

        if (_currentTarget >= _pathToTarget.Length)
        {
            _rigidbody.linearVelocity = Vector2.zero;
            return;
        }

        var current = _pathToTarget[_currentTarget];
        var moveVec = current - _rigidbody.position;
        if (moveVec.sqrMagnitude <= 0.1)
        {
            _currentTarget++;
            return;
        }

        _rigidbody.linearVelocity = moveVec.normalized * MovementSpeed;
    }

    public IDoorPermission DoorPermission => null;
}