using UnityEngine;

public interface IWalkable
{
    public Vector3 Position { get; }

    public bool CanWalkThrough(GameObject walker);
}
