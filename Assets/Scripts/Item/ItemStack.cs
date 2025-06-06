#nullable enable

using System;
using UnityEngine;

/// <summary>
/// Стопка предметов с определённым количеством.
/// Своего рода инстанс предмета с возможностью добавлять мета-информацию
/// </summary>
public class ItemStack : ICloneable
{
    public Item Item { get; }
    public int Count { get; set; }
    public bool IsEmpty => Count <= 0;

    private readonly ItemHandler? _itemHandler;

    public ItemStack(Item item, int count = 1)
    {
        Item = item;
        Count = count;
        _itemHandler = item.InteractHandler.CreateHandler(this);
    }

    public bool CanCombine(ItemStack stack) => Count < Item.MaxCount && stack.Item.Equals(Item);

    public void CombineWith(ItemStack stack)
    {
        var newCount = Mathf.Min(Count + stack.Count, Item.MaxCount);
        var deltaCount = newCount - Count;
        Count = newCount;
        stack.Count -= deltaCount;
    }

    /// <inheritdoc cref="ItemHandler.UseItem"/>
    public bool UseItem(ICharacter character)
    {
        if (_itemHandler is not null && !_itemHandler.UseItem(character))
            return false;

        var usageSound = Item.UsageSound;
        character.Sounds.PlayRandomSound(usageSound);
        return true;
    }

    /// <summary>
    /// Уменьшает количество предмета в текущем стаке
    /// </summary>
    /// <returns>true, если предметы в стаке закончились, false иначе</returns>
    public bool Decrement(int delta = 1)
    {
        Count -= delta;
        return IsEmpty;
    }

    public override string ToString()
    {
        return "{" + $"{Item.Name}: {Count}" + "}";
    }

    public ItemStack Copy() => (ItemStack)Clone();

    public object Clone() => new ItemStack(Item, Count);
}