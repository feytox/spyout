using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider2D))]
public class DoorDetector : MonoBehaviour, IWalkable
{
    [SerializeField]
    private ShadowCaster2D _doorShadowCaster;
    
    public DoorType DoorType;
    public SpriteRenderer DoorRenderer;
    public Collider2D DoorCollider;
    public Sprite OpenedSprite;
    
    private readonly HashSet<int> _visitors = new();
    private Sprite _closedSprite;
    
    void Start()
    {
        Debug.Assert(DoorRenderer != null);
        Debug.Assert(OpenedSprite != null);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out IDoorPermission permission))
            return;
        
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
        
        DoorCollider.enabled = false;
        _closedSprite = DoorRenderer.sprite;    
        DoorRenderer.sprite = OpenedSprite;
        _doorShadowCaster.enabled = false;
    }

    private void Close(Collider2D obj)
    {
        var removed = _visitors.Remove(obj.GetInstanceID());
        if (_visitors.Count != 0 || !removed)
            return;

        DoorCollider.enabled = true;
        DoorRenderer.sprite = _closedSprite;
        _doorShadowCaster.enabled = true;
    }

    public Vector3 Position => transform.position;
    
    public bool CanWalkThrough(GameObject walker)
    {
        return walker.TryGetComponent(out IDoorPermission permission) && permission.CanOpenDoor(DoorType);
    }
}