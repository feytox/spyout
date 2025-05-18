using UnityEngine;

public static class VectorsExtensions
{
    public static Vector2Int MinXY(this Vector2Int vec, Vector3Int other)
    {
        return new Vector2Int(Mathf.Min(vec.x, other.x), Mathf.Min(vec.y, other.y));
    }
    
    public static Vector2Int MaxXY(this Vector2Int vec, Vector3Int other)
    {
        return new Vector2Int(Mathf.Max(vec.x, other.x), Mathf.Max(vec.y, other.y));
    }

    public static Vector2Int ToXY(this Vector3Int vec) => (Vector2Int)vec;

    public static Vector2 ToCellCenter(this Vector2Int pos) => new(pos.x + 0.5f, pos.y + 0.5f);

    public static Vector2 ToCellCenter(this Vector2 pos) => new(pos.x + 0.5f, pos.y + 0.5f);
}