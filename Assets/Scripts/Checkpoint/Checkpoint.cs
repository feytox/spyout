using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Waypoint _point;
    [SerializeField] private PlayerTriggerCollider _collider;

    public event Action OnActivate;
    
    public Vector3 Position => _point.Position;
    
    private bool _activated;

    void Start() => _collider.OnPlayerEnter += TryActivate;

    private void TryActivate()
    {
        if (_activated)
            return;

        _activated = true;
        OnActivate?.Invoke();
    }
}