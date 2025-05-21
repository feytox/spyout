#nullable enable
using UnityEngine;

/// <summary>
/// Предмет-тип, который указывается в <see cref="ItemStack"/>
/// </summary>
[CreateAssetMenu(menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public const int DefaultMaxCount = 16;

    public string Name;
    public Sprite Sprite;
    public int MaxCount;
    public float Damage;
    public float HealAmount;
    public ItemType ItemType;
    public ItemHandlerType InteractHandler;
    public AudioClip[] UsageSound;
    public AudioClip[] AttackSound;
    public AudioClip[] AttackMissSound;
    public AudioClip[] PickupSound;

    public Item(string name, Sprite sprite, int maxCount, float damage, ItemType itemType,
        ItemHandlerType interactHandler, AudioClip[] usageSound, AudioClip[] attackSound, AudioClip[] pickupSound,
        AudioClip[] attackMissSound)
    {
        Name = name;
        Sprite = sprite;
        MaxCount = maxCount;
        Damage = damage;
        ItemType = itemType;
        InteractHandler = interactHandler;
        UsageSound = usageSound;
        AttackSound = attackSound;
        PickupSound = pickupSound;
        AttackMissSound = attackMissSound;
    }
}

public enum ItemType : byte
{
    Default = 0,
    Collectable = 1
}
