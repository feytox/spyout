using UnityEngine;

public interface IPositionProvider
{
    public bool IsDead { get; }
    public Vector2 Position { get; }
}