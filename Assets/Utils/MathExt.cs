using UnityEngine;

public static class MathExt
{
    public static int MathMod(int a, int b) {
        return (Mathf.Abs(a * b) + a) % b;
    }
}