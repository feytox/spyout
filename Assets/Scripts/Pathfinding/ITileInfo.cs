using System;

public interface ITileInfo
{
    public bool CanWalkThrough(IWalker walker);
}

public readonly struct SimpleTileInfo : ITileInfo
{
    private readonly bool _isWalkable;

    public SimpleTileInfo(bool isWalkable) => _isWalkable = isWalkable;

    public bool CanWalkThrough(IWalker walker) => _isWalkable;
}

public class TileInfo : ITileInfo
{

    private readonly Func<IWalker, bool> _walkable;
    
    public TileInfo(Func<IWalker, bool> walkablePredicate)
    {
        _walkable = walkablePredicate;
    }
    
    public bool CanWalkThrough(IWalker walker) => _walkable(walker);
}