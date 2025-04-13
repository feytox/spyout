using System;

public interface ITileInfo
{
    public bool IsWalkable { get; }
    public int Cost { get; }
}

public struct SimpleTileInfo : ITileInfo
{
    public bool IsWalkable { get; }
    public int Cost { get; }

    public SimpleTileInfo(int cost, bool isWalkable)
    {
        Cost = cost;
        IsWalkable = isWalkable;
    }
}

public class TileInfo : ITileInfo
{
    public bool IsWalkable => _walkable();
    public int Cost { get; }

    private readonly Func<bool> _walkable;

    public TileInfo(int cost, Func<bool> walkablePredicate)
    {
        Cost = cost;
        _walkable = walkablePredicate;
    }
}