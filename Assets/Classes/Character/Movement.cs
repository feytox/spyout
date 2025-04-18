#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Classes.Character
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    [DisallowMultipleComponent]
    public class Movement : MonoBehaviour
    {
        public const float PointReachedHeuristic = 0.1f;
        public float MoveSpeed = 170f;

        private Vector2 _moveDirection = Vector2.zero;
        public Vector2 MoveDirection
        {
            get => _moveDirection;
            private set
            {
                if (value == _moveDirection)
                    return;
                _moveDirection = value;
                OnMoveDirectionChanged?.Invoke(value);
            }
        }

        public event Action<Vector2>? OnMoveDirectionChanged;

        private Rigidbody2D? _rigidbody;
        private Collider2D? _collider; // unused in code

        // mutually exclusive
        private IEnumerator<Vector2>? _currentPath = null;
        private Vector2? _currentGoal = null;
        private Vector2? _currentMovingDirection = null;

        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();

            Debug.Assert(_rigidbody != null, "Movement must have Rigidbody2D component");
            Debug.Assert(_collider != null, "Movement must have Collider2D component");

            _rigidbody!.linearDamping = 30f;
        }

        /// <summary>
        /// Move straight in direction infinitely
        /// </summary>
        public void MoveInDirection(Vector2 direction)
        {
            if (direction == Vector2.zero)
            {
                Stop();
                return;
            }
            _currentPath = null;
            _currentGoal = null;
            _currentMovingDirection = direction.normalized;
        }

        /// <summary>
        /// Move straight to goal
        /// </summary>
        public void MoveTo(Vector2 goal)
        {
            _currentPath = null;
            _currentGoal = goal;
            _currentMovingDirection = null;
        }

        /// <summary>
        /// Move along path. Path must guarantee that there is no walls between points
        /// </summary>
        public void MoveAlongPath(IEnumerable<Vector2> path) => MoveAlongPath(path.GetEnumerator());

        /// <summary>
        /// Move along path. Path must guarantee that there is no walls between points
        /// </summary>
        public void MoveAlongPath(IEnumerator<Vector2> path)
        {
            if (!path.MoveNext())
            {
                Stop();
                return;
            }
            _currentPath = path;
            _currentGoal = null;
            _currentMovingDirection = null;
        }

        public void Stop()
        {
            _currentPath = null;
            _currentGoal = null;
            _currentMovingDirection = null;
        }

        void FixedUpdate()
        {
            if (_currentPath != null)
            {
                if (!IsPointReached(_currentPath.Current))
                    SetMoveDirectionToPoint(_currentPath.Current);
                else
                {
                    if (!_currentPath.MoveNext())
                    {
                        _currentPath = null;
                        MoveDirection = Vector2.zero;
                    }
                    else
                        SetMoveDirectionToPoint(_currentPath.Current);
                }
            }
            else if (_currentGoal != null)
            {
                if (!IsPointReached(_currentGoal.Value))
                    SetMoveDirectionToPoint(_currentGoal.Value);
                else
                {
                    _currentGoal = null;
                    MoveDirection = Vector2.zero;
                }
            }
            else if (_currentMovingDirection != null)
                MoveDirection = _currentMovingDirection.Value;
            else
                MoveDirection = Vector2.zero;

            Step();
        }

        private void Step()
        {
            if (MoveDirection == Vector2.zero)
                return;

            _rigidbody!.AddForce(MoveDirection * MoveSpeed, ForceMode2D.Force);
        }

        private bool IsPointReached(Vector2 point) =>
            Vector2Ext.Heuristic(transform.position, point) < PointReachedHeuristic;

        private void SetMoveDirectionToPoint(Vector2 point) =>
            MoveDirection = (point - (Vector2)transform.position).normalized;
    }
}
