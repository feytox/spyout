using UnityEngine;

public interface IWalkable
{
    public Vector3 Position { get; }

    public bool CanWalkThrough(IWalker walker);

    public bool CanSeeThrough(IWalker walker);
}