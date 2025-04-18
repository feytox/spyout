using UnityEngine;

public static class Vector2Ext
{
    public static float Heuristic(Vector2 a, Vector2 b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }
}
