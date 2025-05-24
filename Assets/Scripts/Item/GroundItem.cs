#nullable enable
using System;
using UnityEngine;

/// <summary>
/// Контроллер предмета, лежащего на земле.
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class GroundItem : CustomSpriteComponent, IPlayerInteractable
{
    public ItemStack? Stack { get; set; }
    private PopupController? _popup;
    
    protected override Sprite? Sprite => Stack?.Item.Sprite;
    protected override Material? Material => Stack?.Item.OutlineMaterial;

    void Awake() => _popup = GetComponentInChildren<PopupController>();

    public static void SpawnItem(ItemStack stack, Vector3 position)
    {
        var item = new GameObject("Spawned Item");
        var groundItem = item.AddComponent<GroundItem>();
        groundItem.Stack = stack;
        groundItem.RefreshSprite();
        item.transform.position = position;
    }

    public event Action? OnInteract;

    public void Interact()
    {
        var pickupSound = Stack?.Item.PickupSound;
        var playerInventory = PlayerController.GetInstance().Inventory.Inventory;
        if (!TryPickup(playerInventory!)) 
            return;
        
        if (pickupSound is not null)
            SoundFXManager.Instance.PlayRandomSound(pickupSound, transform);
        
        OnInteract?.Invoke();
        Destroy(gameObject);
    }

    private bool TryPickup(Inventory inventory)
    {
        return Stack is not null && inventory.TryAppendStack(Stack);
    }

    public bool CanInteract() => true;

    public Vector3 Position => transform.position;

    PopupController? IPlayerInteractable.Popup => _popup;

    bool IPlayerInteractable.Interacted { get; set; }
}