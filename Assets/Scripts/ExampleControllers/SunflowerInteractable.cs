using UnityEngine;

public class SunflowerInteractable : MonoBehaviour, IPlayerInteractable
{
    private int clickedCount;
    
    public void Interact()
    {
        clickedCount++;
        Debug.Log($"Sunflower x{clickedCount}");
    }

    public bool CanInteract() => true;
    
    public Vector3 Position => transform.position;
}