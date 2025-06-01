using System.Collections.Generic;
using UnityEngine;
using Utils;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider2D))]
public class PlayerInteractionDetector : MonoBehaviour
{
    [SerializeField] private float _checkInteractionCooldown = 0.2f;

    private readonly HashSet<IInteractable> _interactablesInRange = new();
    private Cooldown _checkCooldown;

    public void ClearInteractables()
    {
        foreach (var interactable in _interactablesInRange) 
            interactable.OnInteractionExit();
        
        _interactablesInRange.Clear();
    }
    
    private void Start()
    {
        PlayerController.Inputs.Interact.Subscribe(0, _ => OnInteract());
        _checkCooldown = new Cooldown(_checkInteractionCooldown);
    }

    private void FixedUpdate()
    {
        if (!_checkCooldown.ResetIfExpired())
            return;

        if (_interactablesInRange.Count == 0)
            return;

        foreach (var interactable in _interactablesInRange)
            if (interactable.CanInteract())
                interactable.OnInteractionEnter();
            else
                interactable.OnInteractionExit();
    }

    private bool OnInteract()
    {
        var pos = transform.position;
        var interactable = _interactablesInRange.MinBy(interactable => (interactable.Position - pos).sqrMagnitude);
        if (interactable is null || !interactable.CanInteract())
            return false;

        interactable.Interact();
        return true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out IInteractable interactable))
            return;

        _interactablesInRange.Add(interactable);
        if (interactable.CanInteract())
            interactable.OnInteractionEnter();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.TryGetComponent(out IInteractable interactable))
            return;

        _interactablesInRange.Remove(interactable);
        interactable.OnInteractionExit();
    }
}