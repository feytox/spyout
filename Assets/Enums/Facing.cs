using System;
using UnityEngine;

namespace Enums
{
    public enum Facing : byte
    {
        Front = 0,
        Back = 1,
        Left = 2,
        Right = 3,
    }

    public static class FacingExtensions
    {
        // Returns front if direction is zero
        public static Facing ToFacing(this Vector2 direction) =>
            Mathf.Abs(direction.x) > Mathf.Abs(direction.y)
                ? (direction.x > 0 ? Facing.Right : Facing.Left)
                : (direction.y > 0 ? Facing.Back : Facing.Front);

        // Returns front if direction is zero
        public static Facing ToFacing(this Vector2Int direction) =>
            Math.Abs(direction.x) > Math.Abs(direction.y)
                ? (direction.x > 0 ? Facing.Right : Facing.Left)
                : (direction.y > 0 ? Facing.Back : Facing.Front);

        public static Vector2Int ToVector2Int(this Facing facing) =>
            facing switch
            {
                Facing.Front => Vector2Int.up,
                Facing.Back => Vector2Int.down,
                Facing.Left => Vector2Int.left,
                Facing.Right => Vector2Int.right,
                _ => ThrowInvalidFacing<Vector2Int>(facing),
            };

        public static Vector2 ToVector2(this Facing facing) =>
            facing switch
            {
                Facing.Front => Vector2.up,
                Facing.Back => Vector2.down,
                Facing.Left => Vector2.left,
                Facing.Right => Vector2.right,
                _ => ThrowInvalidFacing<Vector2>(facing),
            };

        public static Facing Opposite(this Facing facing) =>
            facing switch
            {
                Facing.Front => Facing.Back,
                Facing.Back => Facing.Front,
                Facing.Left => Facing.Right,
                Facing.Right => Facing.Left,
                _ => ThrowInvalidFacing<Facing>(facing),
            };

        public static string ToString(this Facing facing) =>
            Enum.GetName(typeof(Facing), facing) ?? ThrowInvalidFacing<string>(facing);

        private static T ThrowInvalidFacing<T>(Facing facing) =>
            throw new ArgumentOutOfRangeException(nameof(facing), facing, "Invalid facing");
    }
}
