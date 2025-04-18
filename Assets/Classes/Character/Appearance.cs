using Enums;
using UnityEngine;
using UnityEngine.U2D.Animation;
using Utils;

namespace Classes.Character
{
    [RequireComponent(typeof(SpriteLibrary))]
    [RequireComponent(typeof(SpriteRenderer))]
    [DisallowMultipleComponent]
    public class Appearance : MonoBehaviour
    {
        // offset for positioning camera/UI?
        public Vector2 Offset { get; private set; }

        private Manager _manager;
        private SpriteLibrary _library;
        private SpriteRenderer _renderer;
        private Movement _movement;

        private Cooldown _frameUpdateInterval;
        private readonly Switcher<int> _frameIndexes = new(0, 1);
        private Facing _facing = Facing.Front;
        private MoveState _moveState = MoveState.Idle;

        void Awake()
        {
            _manager = GetComponentInParent<Manager>();
            _library = GetComponent<SpriteLibrary>();
            _renderer = GetComponent<SpriteRenderer>();

            Debug.Assert(_manager != null, "Appearance must be child of Character Manager");
            Debug.Assert(_library != null, "Appearance must have SpriteLibrary component");
            Debug.Assert(_renderer != null, "Appearance must have SpriteRenderer component");

            Offset = transform.localPosition; // saved once
            _frameUpdateInterval = new(0.2f);
        }

        void Start()
        {
            _movement = _manager.Movement; // nullable
        }

        void Update()
        {
            transform.localPosition = _manager.LocalPosition + Offset;

            if (_movement == null)
                return;

            var newMoveState = _movement.MoveDirection.ToMoveState();
            var newFacing = _movement.MoveDirection.ToFacing();

            if (
                _moveState == newMoveState
                && _facing == newFacing
                && !_frameUpdateInterval.IsExpired
            )
                return;

            _moveState = newMoveState;
            if (_moveState != MoveState.Idle)
                _facing = newFacing;

            if (_frameUpdateInterval.ResetIfExpired())
                _frameIndexes.Switch();

            _renderer.sprite = _library.GetSprite(
                _facing.ToString(),
                _moveState.ToSpriteLabel(_frameIndexes.Current)
            );
        }
    }
}
