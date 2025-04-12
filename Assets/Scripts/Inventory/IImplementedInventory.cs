#nullable enable

using System;

/// <summary>
/// Интерфейс инвентаря, реализованного на массиве предметов.
/// </summary>
public interface IImplementedInventory : IInventory
{
    public ItemStack?[] Items { get; }

    int IInventory.Size => Items.Length;

    ItemStack? IInventory.this[int slot] => Items[slot];
    
    ItemStack? IInventory.SetStack(ItemStack? stack, int slot)
    {
        var prevStack = this[slot];
        Items[slot] = stack;
        return prevStack;
    }
    
    /// <inheritdoc />
    /// <exception cref="IndexOutOfRangeException">
    /// Выбрасывается, если указанный диапазон выходит за пределы допустимых значений.
    /// </exception>
    bool IInventory.TryAppendStack(ItemStack stack) => TryInsertStack(stack, 0, Size);

    /// <inheritdoc />
    /// <exception cref="IndexOutOfRangeException">
    /// Выбрасывается, если указанный диапазон выходит за пределы допустимых значений.
    /// </exception>
    bool IInventory.TryInsertStack(ItemStack stack, int inclusiveStart, int exclusiveEnd)
    {
        if (inclusiveStart < 0 || inclusiveStart >= exclusiveEnd || exclusiveEnd > Size)
            throw new IndexOutOfRangeException();
        
        for (var i = inclusiveStart; i < exclusiveEnd; i++)
        {
            if (this[i] is not null)
                continue;

            SetStack(stack, i);
            return true;
        }

        return false;
    }
}