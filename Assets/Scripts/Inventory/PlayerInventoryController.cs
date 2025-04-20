#nullable enable
using UnityEngine;

public class PlayerInventoryController : InventoryController
{
    private int SelectedSlot { get; set; }
    private ItemStack? SelectedItem => Inventory[SelectedSlot];

    protected override void OnStart()
    {
        var inputs = PlayerController.Inputs;
        inputs.InteractStarted.Subscribe(10, _ => UseSelectedItem());
        inputs.PrevItem += _ => ChangeSlot(-1);
        inputs.NextItem += _ => ChangeSlot(1);
    }

    private bool UseSelectedItem() => SelectedItem is null || SelectedItem!.UseItem();

    private void ChangeSlot(int delta)
    {
        var size = Inventory.Size;
        SelectedSlot += delta;
        if (SelectedSlot < 0 || SelectedSlot >= size)
            SelectedSlot = MathExt.MathMod(SelectedSlot, size);

        Debug.Log($"selected {SelectedSlot}: {SelectedItem}");
    }
}