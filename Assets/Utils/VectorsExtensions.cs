using System.Runtime.CompilerServices;
using UnityEngine;

public static class VectorsExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2Int MinXY(this Vector2Int vec, Vector3Int other)
    {
        return new Vector2Int(Mathf.Min(vec.x, other.x), Mathf.Min(vec.y, other.y));
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2Int MaxXY(this Vector2Int vec, Vector3Int other)
    {
        return new Vector2Int(Mathf.Max(vec.x, other.x), Mathf.Max(vec.y, other.y));
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2Int WithX(this Vector2Int vec, int x) => new(x, vec.y);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2Int WithY(this Vector2Int vec, int y) => new(vec.x, y);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 WithX(this Vector2 vec, float x) => new(x, vec.y);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2Int ToXY(this Vector3Int vec) => (Vector2Int)vec;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 ToCellCenter(this Vector2Int pos) => new(pos.x + 0.5f, pos.y + 0.5f);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 ToCellCenter(this Vector2 pos) => new(pos.x + 0.5f, pos.y + 0.5f);
}