using System;

[Obsolete]
public class TestPlayerInventory : IImplementedInventory
{
    private const int InventorySize = 8;

    public ItemStack[] Items { get; } = new ItemStack[InventorySize];
}