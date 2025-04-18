using Classes;
using Classes.Character;
using UnityEngine;

namespace Player
{
    public class Movement : Singleton<Movement>
    {
        [SerializeField]
        private Manager _playerManager;
        public static Manager PlayerManager => GetInstance()._playerManager;

        protected override void Init() { }

        void Start()
        {
            Debug.Assert(_playerManager != null, "Movement must have player manager");
            Debug.Assert(_playerManager.Movement != null, "Player manager must have movement");
            Debug.Assert(Input.MoveAction != null, "Input must have MoveAction");

            var movement = _playerManager.Movement;
            Input.MoveAction.performed += ctx => movement.MoveInDirection(ctx.ReadValue<Vector2>());
            Input.MoveAction.canceled += ctx => movement.Stop();
        }
    }
}
