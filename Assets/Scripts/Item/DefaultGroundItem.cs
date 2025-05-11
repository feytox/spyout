using System;
using UnityEngine;

/// <summary>
/// Контроллер для предмета, поставленного в редакторе
/// </summary>
public class DefaultGroundItem : GroundItem
{
    public Item DefaultItem;

    [Range(1, Item.DefaultMaxCount)] public int Count = 1;

    private ItemStack _stack;

    public override ItemStack Stack
    {
        get => _stack;
        set => throw new InvalidOperationException("Cannot set stack to Ground Item");
    }

    void Start() => UpdateItem();

    protected override void BeforeSpriteUpdate() => UpdateItem();

    private void UpdateItem()
    {
        Debug.Assert(DefaultItem != null);
        _stack = new ItemStack(DefaultItem, Count);
    }
}