using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider2D))]
public class DoorDetector : MonoBehaviour
{
    public DoorType DoorType;
    public SpriteRenderer DoorSprite;
    public Rigidbody2D DoorBody;

    private int _openedTimes;
    
    void Start()
    {
        Debug.Assert(DoorSprite != null);
        Debug.Assert(DoorBody != null);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"enter");
        if (!other.TryGetComponent(out IPermission permission))
            return;
        
        if (permission.CanOpenDoor(DoorType))
            Open();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log($"exit");
        if (!other.TryGetComponent(out IPermission permission))
            return;
        
        if (permission.CanOpenDoor(DoorType))
            Close();
    }

    private void Open()
    {
        Debug.Log($"open {_openedTimes + 1} {transform.position}");
        if (++_openedTimes != 1) 
            return;
        
        DoorSprite.enabled = false;
        DoorBody.simulated = false;
    }

    private void Close()
    {
        Debug.Log($"close {_openedTimes - 1} {transform.position}");
        if (--_openedTimes != 0) 
            return;
        
        DoorSprite.enabled = true;
        DoorBody.simulated = true;
    }
}
