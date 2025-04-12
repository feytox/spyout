#nullable enable
using UnityEngine;

/// <summary>
/// Контроллер предмета, лежащего на земле.
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(SpriteRenderer))]
public abstract class GroundItem : CustomSpriteComponent, ISingletonInventory
{
    public abstract ItemStack? Item { get; set; }
    
    protected override Sprite? Sprite => Item?.Item.Sprite;

    public bool TryPickup(IInventory inventory)
    {
        return Item is not null && inventory.TryAppendStack(Item);
    }
}