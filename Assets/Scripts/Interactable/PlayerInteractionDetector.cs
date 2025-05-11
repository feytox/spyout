using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider2D))]
public class PlayerInteractionDetector : MonoBehaviour
{
    private readonly HashSet<IInteractable> _interactablesInRange = new();
    
    void Start()
    {
        PlayerController.Inputs.Interact.Subscribe(0, _ => OnInteract());
    }

    private bool OnInteract()
    {
        var pos = transform.position;
        var interactable = _interactablesInRange.MinBy(interactable => (interactable.Position - pos).sqrMagnitude);
        if (interactable is null)
            return false;

        interactable.Interact();
        return true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out IInteractable interactable) || !interactable.CanInteract()) 
            return;
        
        _interactablesInRange.Add(interactable);
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