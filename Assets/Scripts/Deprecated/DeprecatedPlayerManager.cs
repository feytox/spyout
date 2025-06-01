using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Convenient way to modify player, bind input actions, etc
/// </summary>
[Obsolete]
[RequireComponent(typeof(PlayerInput))]
public class DeprecatedPlayerManager : MonoBehaviour
{
    public DeprecatedPlayerController PlayerController { get; private set; }
    private DeprecatedCameraController CameraController { get; set; }

    public InputAction MoveAction { get; private set; }
    private InputAction InteractAction { get; set; }

    // default actions names should be reviewed
    // just move is enough for now

    private void Awake()
    {
        PlayerController = GetComponentInChildren<DeprecatedPlayerController>();
        CameraController = GetComponentInChildren<DeprecatedCameraController>();

        var playerInput = GetComponent<PlayerInput>(); // not really need to assign anywhere else
        MoveAction = playerInput.actions["Move"];
        InteractAction = playerInput.actions["Interact"];

        Debug.Assert(PlayerController != null);
        Debug.Assert(CameraController != null);
        Debug.Assert(MoveAction != null);
        Debug.Assert(InteractAction != null);
    }
}
