using UnityEngine;

public interface IInteractable
{
    public void Interact();
    public bool CanInteract();
    public void OnInteractionEnter();
    public void OnInteractionExit(); 
    
    public Vector3 Position { get; }
}