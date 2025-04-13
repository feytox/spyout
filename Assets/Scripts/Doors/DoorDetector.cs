using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider2D))]
public class DoorDetector : MonoBehaviour
{
    public DoorType DoorType;
    public SpriteRenderer DoorRenderer;
    public Rigidbody2D DoorBody;
    public Sprite OpenedSprite;
    
    private Sprite _closedSprite;
    private HashSet<int> _visitors;
    
    void Start()
    {
        Debug.Assert(DoorRenderer != null);
        Debug.Assert(DoorBody != null);
        Debug.Assert(OpenedSprite != null);
        
        _closedSprite = DoorRenderer.sprite;
        _visitors = new HashSet<int>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out IDoorPermission permission))
            return;

        Debug.Log(DoorType); // TODO: remove logging
        if (permission.CanOpenDoor(DoorType))
            Open(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.TryGetComponent(out IDoorPermission permission))
            return;

        if (permission.CanOpenDoor(DoorType))
            Close(other);
    }

    private void Open(Collider2D obj)
    {
        var added = _visitors.Add(obj.GetInstanceID());
        if (_visitors.Count != 1 || !added)
            return;

        DoorBody.simulated = false;
        DoorRenderer.sprite = OpenedSprite;
    }

    private void Close(Collider2D obj)
    {
        var removed = _visitors.Remove(obj.GetInstanceID());
        if (_visitors.Count != 0 || !removed)
            return;

        DoorBody.simulated = true;
        DoorRenderer.sprite = _closedSprite;
    }
}