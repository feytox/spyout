using System;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(DoorDetector))]
public class LockedDoorInteractable : MonoBehaviour, IPlayerInteractable
{
    [SerializeField] private Item _doorKey;
    [SerializeField] private Sprite _unlockedSprite;

    private PopupController _popup;
    private DoorDetector _doorDetector;
    private bool _unlocked;

    private void Start()
    {
        _popup = GetComponentInChildren<PopupController>();
        _doorDetector = GetComponent<DoorDetector>();
    }

    public event Action OnInteract;

    public void Interact()
    {
        var inventory = PlayerController.GetInstance().Inventory;
        if (inventory.ActiveItem!.Decrement())
            inventory.Inventory.RefreshSlot(inventory.ActiveSlot);

        OnInteract?.Invoke();
        UnlockDoor();
        (this as IPlayerInteractable).OnInteractionExit();
    }

    public bool CanInteract()
    {
        if (_unlocked)
            return false;

        var activeStack = PlayerController.GetInstance().Inventory.ActiveItem;
        return _doorKey == activeStack?.Item;
    }

    private void UnlockDoor()
    {
        _doorDetector.DoorRenderer.sprite = _unlockedSprite;
        _doorDetector.DoorType = DoorType.Public;
        _doorDetector.Open(PlayerController.GetInstance().Collider);
        _unlocked = true;
    }

    public Vector3 Position => transform.position;

    PopupController IPlayerInteractable.Popup => _popup;

    bool IPlayerInteractable.Interacted { get; set; }
}