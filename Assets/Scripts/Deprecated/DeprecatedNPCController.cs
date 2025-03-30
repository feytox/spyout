using System;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;
using Random = UnityEngine.Random;

public class DeprecatedNPCController : MonoBehaviour
{
    public float moveSpeed = 4;
    public float moveMaxCooldown = 5; // in seconds // TODO: remove when obsolete
    public float movementDuration = 2; // in seconds
    
    private Rigidbody2D rb;
    private Cooldown cooldown;
    private Cooldown moveCooldown;
    private Vector2 moveVec;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cooldown = new Cooldown(Random.value * moveMaxCooldown);
        moveCooldown = new Cooldown(movementDuration);
        
        cooldown.Reset();
        moveCooldown.Reset();
    }

    void FixedUpdate()
    {
        if (!moveCooldown.IsExpired)
            rb.linearVelocity = moveVec * moveSpeed;
        
        if (!cooldown.IsExpired)
            return;

        moveVec = Random.insideUnitCircle;
        cooldown.SetDuration(Random.value * moveMaxCooldown);
        cooldown.Reset();
        moveCooldown.ResetIfExpired();
    }
}
