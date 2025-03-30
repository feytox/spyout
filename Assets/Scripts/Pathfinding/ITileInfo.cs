public interface ITileInfo
{
    public bool Walkable { get; }
    public int Cost { get; }
}

public struct SimpleTileInfo : ITileInfo
{
    public bool Walkable { get; }
    public int Cost { get; }

    public SimpleTileInfo(bool walkable, int cost)
    {
        Walkable = walkable;
        Cost = cost;
    }
}