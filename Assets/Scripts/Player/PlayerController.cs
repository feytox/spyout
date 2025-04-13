using UnityEngine;

// The only difference of player and any other character is that he has such independent script in the hierarchy
// Just binds character movement to inputs, its all it really does
// In case that player and controller is only one on scene, it is possible to easy swap player characters
// So i made controller as simple as f*ck
// It uses CharacterManager as a base and nothing more. Perfect!

[DisallowMultipleComponent]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private CharacterManager _playerManager;
    public static CharacterManager PlayerManager
    {
        get => GetInstance()._playerManager;
        set => GetInstance()._playerManager = value;
    }

    void Awake()
    {
        Debug.Assert(
            _singleton == null,
            $"{gameObject.name} tried to awake {nameof(PlayerController)} second time!"
        );
        _singleton = this;
        // DontDestroyOnLoad(gameObject);

        Debug.Assert(_playerManager != null, "PlayerManager is not assigned!");

        var moveAction = PlayerInputs.MoveAction; // checked
        moveAction.performed += ctx => _playerManager.MoveInDirection(ctx.ReadValue<Vector2>()); // Already normalized
        moveAction.canceled += ctx => _playerManager.MoveInDirection(Vector2.zero);
    }

    private static PlayerController _singleton;

    private static PlayerController GetInstance()
    {
        Debug.Assert(
            _singleton != null,
            $"Tried to access {nameof(PlayerController)} before it was initialized!"
        );
        return _singleton;
    }
}
