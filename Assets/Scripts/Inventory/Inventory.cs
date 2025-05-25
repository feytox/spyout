#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;

public class Inventory
{
    public int Size => Items.Length;

    public event Action<int, ItemStack?>? OnSlotUpdated;
    public event Action<Item, int>? OnCollectableItemChange;
    
    public Dictionary<Item, int> CollectableItems { get; private set; } = new();
    public ItemStack?[] Items { get; private set; }
    
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
        if (TryCollectItem(stack))
            return true;
        
        for (var i = 0; i < Items.Length; i++)
            if (TryPutStack(i, stack)) 
                return true;

        return false;
    }

    public void RefreshSlot(int slot)
    {
        var stack = this[slot];
        if (stack is not null && stack.IsEmpty) 
            PopStack(slot);
        else
            OnSlotUpdated?.Invoke(slot, stack);
    }

    private bool TryPutStack(int slot, ItemStack stack)
    {
        var currentStack = Items[slot];
        if (currentStack is null)
        {
            Items[slot] = stack;
            OnSlotUpdated?.Invoke(slot, stack);
            return true;
        }

        if (!currentStack.CanCombine(stack))
            return false;
            
        currentStack.CombineWith(stack);
        OnSlotUpdated?.Invoke(slot, currentStack);
        return stack.IsEmpty;
    }

    private bool TryCollectItem(ItemStack stack)
    {
        if (stack.Item.ItemType != ItemType.Collectable) 
            return false;

        var count = stack.Count + CollectableItems.GetValueOrDefault(stack.Item);
        CollectableItems[stack.Item] = count;
        OnCollectableItemChange?.Invoke(stack.Item, count);
        return true;
    }

    public void LoadFromSave(ItemStack[] items)
    {
        Items = items;
        
        for (var slot = 0; slot < Items.Length; slot++) 
            OnSlotUpdated?.Invoke(slot, Items[slot]);
    }

    public override string ToString()
    {
        var itemsStr = Items.Select((stack, i) => (stack, i))
            .Where(tuple => tuple.stack is not null)
            .Select(tuple => $"{tuple.i}: {tuple.stack}");

        return $"[{string.Join(", ", itemsStr)}]";
    }
}