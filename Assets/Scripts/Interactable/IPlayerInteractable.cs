using UnityEngine;

public interface IPlayerInteractable
{
    public void Interact();

    public bool CanInteract();
    
    public Vector3 Position { get; }
}