using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    
    void Start()
    {
        var playerManager = GetComponentInParent<PlayerManager>();
        Debug.Assert(playerManager != null);

        transform.SetParent(playerManager.PlayerController.BodyTransform);
    }
}
