using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Convenient way to modify player, bind input actions, etc
/// </summary>
[RequireComponent(typeof(PlayerInput))]
public class PlayerManager : MonoBehaviour
{
    public PlayerController PlayerController { get; private set; }
    public CameraController CameraController { get; private set; }

    public InputAction MoveAction { get; private set; }

    // default actions names should be reviewed
    // just move is enough for now

    void Awake()
    {
        PlayerController = GetComponentInChildren<PlayerController>();
        CameraController = GetComponentInChildren<CameraController>();

        var playerInput = GetComponent<PlayerInput>(); // not really need to assign anywhere else
        MoveAction = playerInput.actions["Move"];

        Debug.Assert(PlayerController != null);
        Debug.Assert(CameraController != null);
        Debug.Assert(MoveAction != null);
    }
}
