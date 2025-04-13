using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[DisallowMultipleComponent]
public class PlayerInputs : MonoBehaviour
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

    void Awake()
    {
        Debug.Assert(
            _singleton == null,
            $"{gameObject.name} tried to awake {nameof(PlayerInputs)} second time!"
        );
        _singleton = this;
        DontDestroyOnLoad(gameObject);

        var playerInput = GetComponent<PlayerInput>(); // not really need to assign anywhere else

        _moveAction = playerInput.actions["Move"];
        Debug.Assert(_moveAction != null);

        _interactAction = playerInput.actions["Interact"];
        Debug.Assert(_interactAction != null);

        _sprintAction = playerInput.actions["Sprint"];
        Debug.Assert(_sprintAction != null);
    }

    private static PlayerInputs _singleton;

    public static PlayerInputs GetInstance()
    {
        Debug.Assert(
            _singleton != null,
            $"Tried to access {nameof(PlayerInputs)} before it was initialized!"
        );
        return _singleton;
    }
}
