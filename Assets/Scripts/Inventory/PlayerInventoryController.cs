#nullable enable
using UnityEngine;

public class PlayerInventoryController : InventoryController
{
    private int SelectedSlot { get; set; }
    public override ItemStack? ActiveItem => Inventory?[SelectedSlot];

    protected override void OnStart()
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
        var size = Inventory!.Size;
        SelectedSlot += delta;
        if (SelectedSlot < 0 || SelectedSlot >= size)
            SelectedSlot = MathExt.MathMod(SelectedSlot, size);

        Debug.Log($"selected {SelectedSlot}: {ActiveItem}");
    }
}