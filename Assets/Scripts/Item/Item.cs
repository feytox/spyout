#nullable enable
using UnityEngine;

/// <summary>
/// Предмет-тип, который указывается в <see cref="ItemStack"/>
/// </summary>
[CreateAssetMenu(menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public string Name;
    public Sprite Sprite;

    public Item(string name, Sprite sprite)
    {
        Name = name;
        Sprite = sprite;
    }
}