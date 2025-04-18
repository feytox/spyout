using Classes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(PlayerInput))]
    public class Input : Singleton<Input>
    {
        private InputAction _moveAction;
        public static InputAction MoveAction
        {
            get => GetInstance()._moveAction;
            set => GetInstance()._moveAction = value;
        }

        private InputAction _interactAction;
        public static InputAction InteractAction
        {
            get => GetInstance()._interactAction;
            set => GetInstance()._interactAction = value;
        }

        private InputAction _sprintAction;
        public static InputAction SprintAction
        {
            get => GetInstance()._sprintAction;
            set => GetInstance()._sprintAction = value;
        }

        protected override void Init()
        {
            var playerInput = GetComponent<PlayerInput>(); // not really need to assign anywhere else
            Debug.Assert(playerInput != null);

            _moveAction = playerInput.actions["Move"];
            Debug.Assert(_moveAction != null);

            _interactAction = playerInput.actions["Interact"];
            Debug.Assert(_interactAction != null);

            _sprintAction = playerInput.actions["Sprint"];
            Debug.Assert(_sprintAction != null);
        }
    }
}
