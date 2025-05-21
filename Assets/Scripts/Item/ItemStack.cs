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

    /// <summary>
    /// Позволяет переопределить количество восстанавливаемого здоровья для этого конкретного стака.
    /// Если null, используется значение из Item.HealAmount.
    /// </summary>
    public float? CustomHealAmount { get; set; }

    /// <summary>
    /// Возвращает действительное количество восстанавливаемого здоровья,
    /// учитывая возможное переопределение.
    /// </summary>
    public float EffectiveHealAmount => CustomHealAmount ?? Item.HealAmount;

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
        var usageSound = Item.UsageSound;
        character.Sounds.PlayRandomSound(usageSound);
        
        return _itemHandler is null || _itemHandler.UseItem(character);
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
}
