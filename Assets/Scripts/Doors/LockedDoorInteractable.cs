using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(DoorDetector))]
public class LockedDoorInteractable : MonoBehaviour, IPlayerInteractable
{
    [SerializeField] private Item _doorKey;
    [SerializeField] private Sprite _unlockedSprite;
    
    private PopupController _popup;
    private DoorDetector _doorDetector;

    void Start()
    {
        _popup = GetComponentInChildren<PopupController>();
        _doorDetector = GetComponent<DoorDetector>();
    }

    public void Interact()
    {
        var inventory = PlayerController.GetInstance().Inventory;
        if (inventory.ActiveItem!.Decrement())
            inventory.Inventory.RefreshSlot(inventory.ActiveSlot);
        
        UnlockDoor();
        (this as IPlayerInteractable).OnInteractionExit();
    }

    public bool CanInteract()
    {
        var activeStack = PlayerController.GetInstance().Inventory.ActiveItem;
        return _doorKey == activeStack?.Item;
    }

    private void UnlockDoor()
    {
        _doorDetector.DoorRenderer.sprite = _unlockedSprite;
        _doorDetector.DoorType = DoorType.Public;
        _doorDetector.Open(PlayerController.GetInstance().Collider);
    }

    public Vector3 Position => transform.position;

    PopupController IPlayerInteractable.Popup => _popup;

    bool IPlayerInteractable.Interacted { get; set; }
}