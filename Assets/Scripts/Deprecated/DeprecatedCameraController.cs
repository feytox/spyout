using System;
using UnityEngine;

[Obsolete]
[RequireComponent(typeof(Camera))]
public class DeprecatedCameraController : MonoBehaviour
{
    private void Start()
    {
        var playerManager = GetComponentInParent<DeprecatedPlayerManager>();
        Debug.Assert(playerManager != null);

        transform.SetParent(playerManager.PlayerController.BodyTransform);
    }
}
