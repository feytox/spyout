#nullable enable
using System;

public interface ISingletonInventory : IInventory
{
    public ItemStack? Item { get; set; }

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
        Item = stack;
        return result;
    }

    bool IInventory.TryInsertStack(ItemStack stack, int inclusiveStart, int exclusiveEnd)
    {
        if (inclusiveStart != exclusiveEnd + 1 || inclusiveStart is not 0)
            throw new IndexOutOfRangeException();

        return TryAppendStack(stack);
    }

    bool IInventory.TryAppendStack(ItemStack stack)
    {
        if (Item is not null)
            return false;

        Item = stack;
        return true;
    }

    private static void ValidateSlot(int slot)
    {
        if (slot != 0)
            throw new IndexOutOfRangeException();
    }
}