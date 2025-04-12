#nullable enable
using UnityEngine;

/// <summary>
/// Контроллер предмета, лежащего на земле.
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(SpriteRenderer))]
public abstract class GroundItem : CustomSpriteComponent, ISingletonInventory
{
    public virtual ItemStack? Item { get; }
    
    protected override Sprite? Sprite => Item?.Item.Sprite;
}