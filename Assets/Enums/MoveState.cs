using System;
using System.Collections.Generic;
using UnityEngine;

namespace Enums
{
    public enum MoveState : byte
    {
        Idle = 0,
        Walk = 1,
    }

    public static class MovementStateExtensions
    {
        public static MoveState ToMoveState(this Vector2 move) =>
            move == Vector2.zero ? MoveState.Idle : MoveState.Walk;

        public static string ToString(this MoveState state) =>
            Enum.GetName(typeof(MoveState), state) ?? ThrowInvalidMoveState<string>(state);

        public static string ToSpriteLabel(this MoveState state, int frameIndex)
        {
            // do not generate variables at runtime
            Debug.Assert(_spriteLabels.ContainsKey(state), "Invalid move state");
            Debug.Assert(
                0 <= frameIndex && frameIndex < _spriteLabels[state].Length,
                "Invalid frame index"
            );

            return _spriteLabels[state][frameIndex];
        }

        private static Dictionary<MoveState, string[]> _spriteLabels = new()
        {
            { MoveState.Idle, new string[] { "Idle1", "Idle2" } },
            { MoveState.Walk, new string[] { "Walk1", "Walk2" } },
        };

        private static T ThrowInvalidMoveState<T>(MoveState state) =>
            throw new ArgumentOutOfRangeException(nameof(state), state, "Invalid facing");
    }
}
