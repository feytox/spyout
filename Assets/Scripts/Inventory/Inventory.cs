#nullable enable
using System;
using System.Linq;

public class Inventory
{
    private ItemStack?[] Items { get; }
    public int Size => Items.Length;

    public event Action<int, ItemStack?>? OnSlotUpdated;

    public Inventory(int capacity)
    {
        Items = new ItemStack?[capacity];
    }

    public ItemStack? this[int slot] => Items[slot];

    public ItemStack? SetStack(ItemStack? stack, int slot)
    {
        var prevStack = this[slot];
        Items[slot] = stack;
        OnSlotUpdated?.Invoke(slot, stack);
        return prevStack;
    }

    public ItemStack? PopStack(int slot) => SetStack(null, slot);

    public bool TryAppendStack(ItemStack stack)
    {
        for (var i = 0; i < Items.Length; i++)
        {
            var currentStack = Items[i];
            if (currentStack is null)
            {
                Items[i] = stack;
                OnSlotUpdated?.Invoke(i, stack);
                return true;
            }

            if (!currentStack.CanCombine(stack))
                continue;
            
            currentStack.CombineWith(stack);
            if (stack.IsEmpty)
                return true;
        }

        return false;
    }

    public override string ToString()
    {
        var itemsStr = Items.Select((stack, i) => (stack, i))
            .Where(tuple => tuple.stack is not null)
            .Select(tuple => $"{tuple.i}: {tuple.stack}");

        return $"[{string.Join(", ", itemsStr)}]";
    }
}