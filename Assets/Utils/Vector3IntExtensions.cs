using UnityEngine;

public static class Vector3IntExtensions
{
    public static Vector3Int MinXYZ(this Vector3Int vec, Vector3Int other)
    {
        return new Vector3Int(Mathf.Min(vec.x, other.x), Mathf.Min(vec.y, other.y), Mathf.Min(vec.z, other.z));
    }
    
    public static Vector3Int MaxXYZ(this Vector3Int vec, Vector3Int other)
    {
        return new Vector3Int(Mathf.Max(vec.x, other.x), Mathf.Max(vec.y, other.y), Mathf.Max(vec.z, other.z));
    }

    public static Vector2 ToCenterPos(this Vector3Int pos)
    {
        return new Vector2(pos.x + 0.5f, pos.y + 0.5f);
    }
}