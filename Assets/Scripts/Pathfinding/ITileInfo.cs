using System;

public interface ITileInfo
{
    public bool CanWalkThrough(IWalker walker);

    public bool CanSeeThrough(IWalker walker);
}

public readonly struct SimpleTileInfo : ITileInfo
{
    private readonly bool _isWalkable;
    private readonly bool _canSeeThrough;

    public SimpleTileInfo(bool isWalkable, bool canSeeThrough)
    {
        _isWalkable = isWalkable;
        _canSeeThrough = canSeeThrough;
    }

    public bool CanWalkThrough(IWalker walker) => _isWalkable;
    public bool CanSeeThrough(IWalker walker) => _canSeeThrough;
}

public class TileInfo : ITileInfo
{
    private readonly Func<IWalker, bool> _walkable;
    private readonly Func<IWalker, bool> _canSeeThrough;

    public TileInfo(Func<IWalker, bool> walkablePredicate, Func<IWalker, bool> canSeeThrough)
    {
        _walkable = walkablePredicate;
        _canSeeThrough = canSeeThrough;
    }

    public bool CanWalkThrough(IWalker walker) => _walkable(walker);
    public bool CanSeeThrough(IWalker walker) => _canSeeThrough(walker);
}