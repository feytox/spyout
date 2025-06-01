using System;
using UnityEngine;

public interface IInteractable
{
    public event Action OnInteract;

    public void Interact();
    public bool CanInteract();
    public void OnInteractionEnter();
    public void OnInteractionExit();

    public Vector3 Position { get; }
}