#nullable enable
using System;

public interface ISingletonInventory : IInventory
{
    public ItemStack? Item { get; }

    int IInventory.Size => 1;

    ItemStack? IInventory.this[int slot]
    {
        get
        {
            ValidateSlot(slot);
            return Item;
        }
    }

    ItemStack? IInventory.SetStack(ItemStack? stack, int slot)
    {
        ValidateSlot(slot);
        var result = Item;
        return result;
    }

    private static void ValidateSlot(int slot)
    {
        if (slot != 1)
            throw new IndexOutOfRangeException();
    }
}