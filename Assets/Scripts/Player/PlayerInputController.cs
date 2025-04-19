using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

[DisallowMultipleComponent]
public class PlayerInputController : MonoBehaviour
{
    private Inputs _inputs;

    public PriorityEvent<InputAction.CallbackContext> InteractStarted { get; } = new();

    public Vector2 Movement { get; private set; }

    private void OnEnable()
    {
        _inputs = new Inputs();
        _inputs.Enable();

        _inputs.Player.Move.performed += SetMovement;
        _inputs.Player.Move.canceled += OnMoveCancel;

        _inputs.Player.Interact.started += InteractStarted.ExecuteEvents;
    }

    private void OnDisable()
    {
        _inputs.Player.Move.performed -= SetMovement;
        _inputs.Player.Move.canceled -= OnMoveCancel;

        _inputs.Player.Interact.started -= InteractStarted.ExecuteEvents;

        _inputs.Disable();
    }

    private void SetMovement(InputAction.CallbackContext ctx)
    {
        Movement = ctx.ReadValue<Vector2>();
    }

    private void OnMoveCancel(InputAction.CallbackContext ctx)
    {
        Movement = Vector2.zero;
    }
}