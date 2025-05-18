using System;
using UnityEngine;

public interface ITileInfo
{
    public bool CanWalkThrough(GameObject walker);
}

public readonly struct SimpleTileInfo : ITileInfo
{
    private readonly bool _isWalkable;

    public SimpleTileInfo(bool isWalkable) => _isWalkable = isWalkable;

    public bool CanWalkThrough(GameObject walker) => _isWalkable;

    public bool IsConstantState => true;
}

public class TileInfo : ITileInfo
{

    private readonly Func<GameObject, bool> _walkable;
    
    public TileInfo(Func<GameObject, bool> walkablePredicate)
    {
        _walkable = walkablePredicate;
    }
    
    public bool CanWalkThrough(GameObject walker) => _walkable(walker);

    public bool IsConstantState => false;
}