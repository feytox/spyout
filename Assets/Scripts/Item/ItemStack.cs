#nullable enable

/// <summary>
/// Стопка предметов с определённым количеством.
/// Своего рода инстанс предмета с возможностью добавлять мета-информацию
/// </summary>
public class ItemStack
{
    public const int MaxCount = 16;

    public Item Item { get; }
    public int Count { get; }

    public ItemStack(Item item, int count = 1)
    {
        Item = item;
        Count = count;
    }

    // TODO: add durability
}