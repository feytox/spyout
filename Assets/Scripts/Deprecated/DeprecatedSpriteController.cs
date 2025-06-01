using System;
using UnityEngine;

[Obsolete]
[RequireComponent(typeof(SpriteRenderer))]
public class DeprecatedSpriteController : MonoBehaviour
{
    private DeprecatedPlayerController _playerController;

    private void Start()
    {
        var playerManager = GetComponentInParent<DeprecatedPlayerManager>();
        Debug.Assert(playerManager != null);

        _playerController = playerManager.PlayerController;
        transform.SetParent(_playerController.BodyTransform, false); // i think there's even better solution
    }
}
