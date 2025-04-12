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
    
    /// <summary>
    /// Пытается вставить предмет в первый доступный слот в указанном диапазоне.
    /// </summary>
    /// <param name="stack">Предмет, который нужно вставить.</param>
    /// <param name="inclusiveStart">Начальный индекс диапазона (включительно).</param>
    /// <param name="exclusiveEnd">Конечный индекс диапазона (исключительно).</param>
    /// <returns>
    /// <c>true</c>, если предмет был успешно вставлен; иначе <c>false</c>.
    /// </returns>
    /// <exception cref="IndexOutOfRangeException">
    /// Выбрасывается, если указанный диапазон выходит за пределы допустимых значений.
    /// </exception>
    public bool TryInsertStack(ItemStack? stack, int inclusiveStart, int exclusiveEnd)
    {
        if (inclusiveStart < 0 || inclusiveStart >= exclusiveEnd || exclusiveEnd > Size)
            throw new IndexOutOfRangeException();
        
        for (var i = inclusiveStart; i < exclusiveEnd; i++)
        {
            if (Items[i] is not null)
                continue;

            SetStack(stack, i);
            return true;
        }

        return false;
    }
}