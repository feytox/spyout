using UnityEngine;

public static class MathExt
{
    public static int MathMod(int a, int b) {
        return (Mathf.Abs(a * b) + a) % b;
    }

    public static int ManhattanDistance(Vector2Int a, Vector2Int b) => Mathf.Abs(b.x - a.x) + Mathf.Abs(b.y - a.y);
    
    public static float ManhattanDistance(Vector3 a, Vector3 b) => Mathf.Abs(b.x - a.x) + Mathf.Abs(b.y - a.y);
}