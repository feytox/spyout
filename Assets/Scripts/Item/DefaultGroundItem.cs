using System;
using UnityEngine;

/// <summary>
/// Контроллер для предмета, поставленного в редакторе
/// </summary>
public class DefaultGroundItem : GroundItem
{
    public Item DefaultItem;

    [Range(1, Item.DefaultMaxCount)] public int Count = 1;

    [Header("Heal Override")]
    [Tooltip("Если отмечено, будет использоваться значение Custom Heal Amount вместо значения из Item.")]
    public bool OverrideHealAmount = false;
    [Tooltip("Пользовательское количество восстанавливаемого здоровья для этого предмета.")]
    public float CustomHealAmountValue = 0f;

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
        if (OverrideHealAmount)
        {
            _stack.CustomHealAmount = CustomHealAmountValue;
        }
    }
}
