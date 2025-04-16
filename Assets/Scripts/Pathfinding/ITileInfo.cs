using System;
using UnityEngine;

public interface ITileInfo
{
    public int Cost { get; }
    public bool CanWalkThrough(GameObject walker);
}

public readonly struct SimpleTileInfo : ITileInfo
{
    private readonly bool _isWalkable;
    public int Cost { get; }

    public SimpleTileInfo(int cost, bool isWalkable)
    {
        Cost = cost;
        _isWalkable = isWalkable;
    }
    
    public bool CanWalkThrough(GameObject walker) => _isWalkable;
}

public class TileInfo : ITileInfo
{
    public int Cost { get; }

    private readonly Func<GameObject, bool> _walkable;
    
    public TileInfo(int cost, Func<GameObject, bool> walkablePredicate)
    {
        Cost = cost;
        _walkable = walkablePredicate;
    }
    
    public bool CanWalkThrough(GameObject walker) => _walkable(walker);
}