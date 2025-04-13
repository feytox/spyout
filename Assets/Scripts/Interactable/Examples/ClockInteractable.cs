using UnityEngine;

public class ClockInteractable : MonoBehaviour, IInteractable
{
    private int clickedCount;
    
    public void Interact()
    {
        clickedCount++;
        Debug.Log($"Clock x{clickedCount}");
    }

    public bool CanInteract() => true;
    
    public Vector3 Position => transform.position;
}