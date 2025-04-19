using System;
using UnityEngine;

[Obsolete]
[RequireComponent(typeof(SpriteRenderer))]
public class DeprecatedSpriteController : MonoBehaviour
{
    private DeprecatedPlayerController playerController;

    void Start()
    {
        var playerManager = GetComponentInParent<DeprecatedPlayerManager>();
        Debug.Assert(playerManager != null);

        playerController = playerManager.PlayerController;
        transform.SetParent(playerController.BodyTransform, false); // i think there's even better solution
    }
}
