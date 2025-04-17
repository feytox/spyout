#nullable enable
public class Inventory
{
    public ItemStack?[] Items { get; }

    public Inventory(int capacity)
    {
        Items = new ItemStack?[capacity];
    }

    public ItemStack? this[int slot] => Items[slot];

    public ItemStack? SetStack(ItemStack? stack, int slot)
    {
        var prevStack = this[slot];
        Items[slot] = stack;
        return prevStack;
    }

    public ItemStack? PopStack(int slot) => SetStack(null, slot);

    public bool TryAppendStack(ItemStack stack)
    {
        for (var i = 0; i < Items.Length; i++)
        {
            if (Items[i] is not null)
                continue;

            Items[i] = stack;
            return true;
        }

        return false;
    }
}