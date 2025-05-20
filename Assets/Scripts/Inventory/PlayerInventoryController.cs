public class PlayerInventoryController : InventoryController
{
    void Start()
    {
        var inputs = PlayerController.Inputs;
        var player = PlayerController.GetInstance();

        inputs.Interact.Subscribe(10, _ => UseSelectedItem(player));
        inputs.SwitchSlot += ChangeSlot;
        inputs.SelectSlot += SetActiveSlot;
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
}