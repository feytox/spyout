using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class InteractionDetector : MonoBehaviour
{
    private HashSet<IInteractable> interactablesInRange;

    void Start()
    {
        var playerManager = GetComponentInParent<PlayerManager>();
        Debug.Assert(playerManager != null);

        playerManager.InteractAction.started += _ => OnInteract();
        transform.SetParent(playerManager.PlayerController.transform, false);
        interactablesInRange = new HashSet<IInteractable>();
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