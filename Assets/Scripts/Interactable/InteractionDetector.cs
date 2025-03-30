using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector : MonoBehaviour
{
    private HashSet<IInteractable> interactablesInRange;

    void Start()
    {
        interactablesInRange = new HashSet<IInteractable>();
    }
    
    public void HandleInteractInput(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
            OnInteract();
    }

    private void OnInteract()
    {
        var pos = transform.position;
        var interactable = interactablesInRange.MinBy(interactable => (interactable.Position - pos).sqrMagnitude);
        interactable?.Interact();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IInteractable interactable) && interactable.CanInteract()) 
            interactablesInRange.Add(interactable);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
            interactablesInRange.Remove(interactable);
    }
}
