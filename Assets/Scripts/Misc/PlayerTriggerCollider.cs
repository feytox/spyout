using System;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider2D))]
public class PlayerTriggerCollider : MonoBehaviour
{
    public event Action OnPlayerEnter;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerController _))
            OnPlayerEnter?.Invoke();
    }
}