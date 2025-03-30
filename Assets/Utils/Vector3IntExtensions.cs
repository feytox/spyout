using UnityEngine;

public static class Vector3IntExtensions
{
    public static Vector3Int Min(this Vector3Int vec, Vector3Int other)
    {
        return new Vector3Int(Mathf.Min(vec.x, other.x), Mathf.Min(vec.y, other.y), Mathf.Min(vec.z, other.z));
    }
    
    public static Vector3Int Max(this Vector3Int vec, Vector3Int other)
    {
        return new Vector3Int(Mathf.Max(vec.x, other.x), Mathf.Max(vec.y, other.y), Mathf.Max(vec.z, other.z));
    }
}