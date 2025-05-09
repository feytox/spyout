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
    public int MaxCount = DefaultMaxCount;
    public float Damage;
    public ItemType ItemType;
    public ItemHandlerType InteractHandler;
    
    public Item(string name, Sprite sprite)
    {
        Name = name;
        Sprite = sprite;
    }
}

public enum ItemType : byte
{
    Default = 0,
    Collectable = 1
}