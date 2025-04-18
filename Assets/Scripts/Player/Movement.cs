using Classes;
using Classes.Character;
using UnityEngine;

namespace Player
{
    public class Movement : Singleton<Movement>
    {
        [SerializeField]
        private Manager _playerManager;

        protected override void Init() { }

        void Start()
        {
            Debug.Assert(_playerManager != null, "CameraController must have player manager");
            Debug.Assert(_playerManager.Movement != null, "Player manager must have appearance");
            Debug.Assert(Input.MoveAction != null, "Input must have MoveAction");

            var movement = _playerManager.Movement;
            Input.MoveAction.performed += ctx => movement.MoveInDirection(ctx.ReadValue<Vector2>());
            Input.MoveAction.canceled += ctx => movement.Stop();
        }
    }
}
