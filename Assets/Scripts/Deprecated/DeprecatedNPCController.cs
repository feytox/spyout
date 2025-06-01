using System;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

[Obsolete]
public class DeprecatedNpcController : MonoBehaviour
{
    public float moveSpeed = 4;
    public float moveMaxCooldown = 5; // in seconds // TODO: remove when obsolete
    public float movementDuration = 2; // in seconds
    
    private Rigidbody2D _rb;
    private Cooldown _cooldown;
    private Cooldown _moveCooldown;
    private Vector2 _moveVec;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _cooldown = new Cooldown(Random.value * moveMaxCooldown);
        _moveCooldown = new Cooldown(movementDuration);
        
        _cooldown.Reset();
        _moveCooldown.Reset();
    }

    private void FixedUpdate()
    {
        if (!_moveCooldown.IsExpired)
            _rb.linearVelocity = _moveVec * moveSpeed;
        
        if (!_cooldown.IsExpired)
            return;

        _moveVec = Random.insideUnitCircle;
        _cooldown.SetDuration(Random.value * moveMaxCooldown);
        _cooldown.Reset();
        _moveCooldown.ResetIfExpired();
    }
}
