using UnityEngine;

/// <summary>
/// Контроллер для предмета, поставленного в редакторе
/// </summary>
public class DefaultGroundItem : GroundItem
{
    public Item DefaultItem;

    [Range(1, Item.DefaultMaxCount)] public int Count = 1;

    private void Start() => UpdateItem();

    protected override void BeforeSpriteRefresh() => UpdateItem();

    private void UpdateItem()
    {
        Debug.Assert(DefaultItem != null);
        Stack = new ItemStack(DefaultItem, Count);
    }
}