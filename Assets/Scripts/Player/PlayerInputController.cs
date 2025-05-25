using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

[DisallowMultipleComponent]
public class PlayerInputController : MonoBehaviour
{
    private Inputs _inputs;

    public PriorityEvent<InputAction.CallbackContext> Interact { get; } = new();

    public event Action<bool> SwitchSlot;
    public event Action<Vector2> MovementUpdate;
    public event Action<int> SelectSlot;
    public event Action Attack;
    public event Action DropItem;

    public Vector2 Movement { get; private set; }

    private void OnEnable()
    {
        _inputs = new Inputs();
        _inputs.Enable();
        _inputs.UI.Disable();

        _inputs.Player.Move.performed += SetMovement;
        _inputs.Player.Move.canceled += OnMoveCancel;
        _inputs.Player.Interact.started += Interact.ExecuteEvents;
        _inputs.Player.Attack.performed += OnAttack;
        _inputs.Player.SwitchSlot.started += OnSlotSwitch;
        _inputs.Player.Slot1.started += OnSlot1;
        _inputs.Player.Slot2.started += OnSlot2;
        _inputs.Player.Slot3.started += OnSlot3;
        _inputs.Player.DropItem.started += OnDropItem;
        _inputs.Player.Pause.started += OnPause;

        _inputs.UI.Resume.started += OnResume;
    }

    private void OnDisable()
    {
        _inputs.Player.Move.performed -= SetMovement;
        _inputs.Player.Move.canceled -= OnMoveCancel;
        _inputs.Player.Interact.started -= Interact.ExecuteEvents;
        _inputs.Player.Attack.performed -= OnAttack;
        _inputs.Player.SwitchSlot.started -= OnSlotSwitch;
        _inputs.Player.Slot1.started -= OnSlot1;
        _inputs.Player.Slot2.started -= OnSlot2;
        _inputs.Player.Slot3.started -= OnSlot3;
        _inputs.Player.DropItem.started -= OnDropItem;
        _inputs.Player.Pause.started -= OnPause;

        _inputs.UI.Resume.started -= OnResume;
        
        _inputs.Disable();
    }

    public void Unpause()
    {
        _inputs.Player.Enable();
        _inputs.UI.Disable();
    }

    public void DisablePlayer()
    {
        _inputs.Player.Disable();
        Movement = Vector2.zero;
        MovementUpdate?.Invoke(Vector2.zero);
    }

    #region Handlers

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

    private void OnSlotSwitch(InputAction.CallbackContext ctx)
    {
        var value = ctx.ReadValue<float>();
        if (value == 0)
            return;

        SwitchSlot?.Invoke(ctx.ReadValue<float>() > 0);
    }
    
    // копипаст для возможности отписки от ивента
    private void OnSlot1(InputAction.CallbackContext ctx) => OnSlotSelect(0);
    
    private void OnSlot2(InputAction.CallbackContext ctx) => OnSlotSelect(1);
    
    private void OnSlot3(InputAction.CallbackContext ctx) => OnSlotSelect(2);

    private void OnSlotSelect(int slot) => SelectSlot?.Invoke(slot);

    private void OnAttack(InputAction.CallbackContext ctx) => Attack?.Invoke();

    private void OnDropItem(InputAction.CallbackContext ctx) => DropItem?.Invoke();

    private void OnPause(InputAction.CallbackContext ctx)
    {
        DisablePlayer();
        _inputs.UI.Enable();
        PauseManager.Instance.PauseGame();
    }

    private void OnResume(InputAction.CallbackContext ctx) => PauseManager.Instance.ResumeGame();

    #endregion
}