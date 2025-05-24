using UnityEngine;

public class PlayerInventoryController : InventoryController
{
    [SerializeField]
    private GroundItem _dropItemPrefab;
    
    void Start()
    {
        var inputs = PlayerController.Inputs;
        var player = PlayerController.GetInstance();

        inputs.Interact.Subscribe(10, _ => UseSelectedItem(player));
        inputs.SwitchSlot += ChangeSlot;
        inputs.SelectSlot += SetActiveSlot;
        inputs.DropItem += DropItem;
    }

    private bool UseSelectedItem(PlayerController player)
    {
        if (ActiveItem is null)
            return true;
        
        var result = ActiveItem.UseItem(player);
        Inventory.RefreshSlot(ActiveSlot);
        return result;
    }

    private void ChangeSlot(bool isPositive)
    {
        var size = Inventory.Size;
        var delta = isPositive ? 1 : -1;
        var slot = MathExt.MathMod(ActiveSlot + delta, size);
        SetActiveSlot(slot);
    }

    private void DropItem()
    {
        var activeItem = ActiveItem;
        if (activeItem is null || activeItem.IsEmpty)
            return;

        var stack = new ItemStack(activeItem.Item);
        if (activeItem.Decrement())
            Inventory.PopStack(ActiveSlot);
        else
            Inventory.RefreshSlot(ActiveSlot);
        
        var groundItem = Instantiate(_dropItemPrefab, transform.position, Quaternion.identity);
        groundItem.Stack = stack;
        groundItem.RefreshSprite();
    }
}