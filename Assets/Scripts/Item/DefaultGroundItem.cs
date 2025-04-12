using System;
using UnityEngine;

/// <summary>
/// Контроллер для предмета, поставленного в редакторе
/// </summary>
public class DefaultGroundItem : GroundItem
{
    public Item DefaultItem;

    [Range(1, ItemStack.MaxCount)] public int Count = 1;

    private ItemStack _item;

    public override ItemStack Item
    {
        get => _item;
        set => throw new InvalidOperationException("Cannot set stack to Ground Item");
    }

    private void Start() => UpdateItem();

    protected override void BeforeSpriteUpdate() => UpdateItem();

    private void UpdateItem()
    {
        Debug.Assert(DefaultItem != null);
        _item = new ItemStack(DefaultItem, Count);
    }
}