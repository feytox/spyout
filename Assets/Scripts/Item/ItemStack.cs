#nullable enable

using UnityEngine;

/// <summary>
/// Стопка предметов с определённым количеством.
/// Своего рода инстанс предмета с возможностью добавлять мета-информацию
/// </summary>
public class ItemStack
{
    public Item Item { get; }
    public int Count { get; private set; }
    public bool IsEmpty => Count <= 0;

    private readonly ItemHandler? _itemHandler;

    public ItemStack(Item item, int count = 1)
    {
        Item = item;
        Count = count;
        _itemHandler = item.ItemHandlerType.CreateHandler(this);
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
        return _itemHandler is null || _itemHandler.UseItem(character);
    }

    public override string ToString()
    {
        return "{" + $"{Item.Name}: {Count}" + "}";
    }
}