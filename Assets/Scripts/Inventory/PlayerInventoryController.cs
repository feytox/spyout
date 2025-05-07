public class PlayerInventoryController : InventoryController
{
    void Start()
    {
        var inputs = PlayerController.Inputs;
        var player = PlayerController.GetInstance();

        inputs.InteractStarted.Subscribe(10, _ => UseSelectedItem(player));
        inputs.PrevItem += _ => ChangeSlot(-1);
        inputs.NextItem += _ => ChangeSlot(1);
    }

    private bool UseSelectedItem(PlayerController player)
    {
        return ActiveItem is null || ActiveItem.UseItem(player);
    }

    private void ChangeSlot(int delta)
    {
        var size = Inventory.Size;
        var slot = MathExt.MathMod(ActiveSlot + delta, size);
        SetActiveSlot(slot);
    }
}