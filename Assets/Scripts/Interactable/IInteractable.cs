using UnityEngine;

public interface IInteractable
{
    public void Interact();

    public bool CanInteract();
    
    public Vector3 Position { get; }
}