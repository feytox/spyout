using System;

public interface ITileInfo
{
    public bool Walkable { get; }
    public int Cost { get; }
}

public struct SimpleTileInfo : ITileInfo
{
    public bool Walkable { get; }
    public int Cost { get; }

    public SimpleTileInfo(int cost, bool walkable)
    {
        Cost = cost;
        Walkable = walkable;
    }
}

public class TileInfo : ITileInfo
{
    public bool Walkable => _walkable();
    public int Cost { get; }

    private readonly Func<bool> _walkable;

    public TileInfo(int cost, Func<bool> walkablePredicate)
    {
        Cost = cost;
        _walkable = walkablePredicate;
    }
}