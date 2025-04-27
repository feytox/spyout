using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

[DisallowMultipleComponent]
public class PlayerInputController : MonoBehaviour
{
    private Inputs _inputs;

    public PriorityEvent<InputAction.CallbackContext> InteractStarted { get; } = new();

    public event Action<InputAction.CallbackContext> PrevItem;
    public event Action<InputAction.CallbackContext> NextItem;
    public event Action<Vector2> MovementUpdate;

    public Vector2 Movement { get; private set; }

    private void OnEnable()
    {
        _inputs = new Inputs();
        _inputs.Enable();

        _inputs.Player.Move.performed += SetMovement;
        _inputs.Player.Move.canceled += OnMoveCancel;

        _inputs.Player.Interact.started += InteractStarted.ExecuteEvents;
        _inputs.Player.PreviousItem.started += OnPrevItem;
        _inputs.Player.NextItem.started += OnNextItem;
    }

    private void OnDisable()
    {
        _inputs.Player.Move.performed -= SetMovement;
        _inputs.Player.Move.canceled -= OnMoveCancel;

        _inputs.Player.Interact.started -= InteractStarted.ExecuteEvents;
        _inputs.Player.PreviousItem.started -= OnPrevItem;
        _inputs.Player.NextItem.started -= OnNextItem;

        _inputs.Disable();
    }

    private void SetMovement(InputAction.CallbackContext ctx)
    {
        Movement = ctx.ReadValue<Vector2>();
        MovementUpdate?.Invoke(Movement);
    }

    private void OnMoveCancel(InputAction.CallbackContext ctx)
    {
        Movement = Vector2.zero;
        MovementUpdate?.Invoke(Movement);
    }

    private void OnPrevItem(InputAction.CallbackContext ctx) => PrevItem?.Invoke(ctx);
    
    private void OnNextItem(InputAction.CallbackContext ctx) => NextItem?.Invoke(ctx);
}