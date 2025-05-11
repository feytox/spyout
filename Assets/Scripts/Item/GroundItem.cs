#nullable enable
using UnityEngine;

/// <summary>
/// Контроллер предмета, лежащего на земле.
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public abstract class GroundItem : CustomSpriteComponent, IPlayerInteractable
{
    private PopupController? _popup;
    public abstract ItemStack? Stack { get; set; }
    
    protected override Sprite? Sprite => Stack?.Item.Sprite;

    void Awake() => _popup = GetComponentInChildren<PopupController>();

    public void Interact()
    {
        var playerInventory = PlayerController.GetInstance().Inventory.Inventory;
        if (TryPickup(playerInventory!)) 
            Destroy(gameObject);
    }

    private bool TryPickup(Inventory inventory)
    {
        return Stack is not null && inventory.TryAppendStack(Stack);
    }

    public bool CanInteract() => true;

    public Vector3 Position => transform.position;

    PopupController? IPlayerInteractable.Popup => _popup;
}