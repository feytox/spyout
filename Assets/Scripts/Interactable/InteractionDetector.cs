using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(ParentPositionSync))]
public class InteractionDetector : MonoBehaviour
{
    private HashSet<IInteractable> _interactablesInRange;

    void Start()
    {
        PlayerController.Inputs.InteractStarted.Subscribe(0, _ => OnInteract());
        _interactablesInRange = new HashSet<IInteractable>();
    }

    private bool OnInteract()
    {
        var pos = transform.position;
        var interactable = _interactablesInRange.MinBy(interactable => (interactable.Position - pos).sqrMagnitude);
        if (interactable is null)
            return true;

        interactable.Interact();
        return false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IInteractable interactable) && interactable.CanInteract())
            _interactablesInRange.Add(interactable);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
            _interactablesInRange.Remove(interactable);
    }
}