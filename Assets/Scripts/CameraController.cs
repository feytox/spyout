using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    private float offsetY = 0.35f;
    private Transform cameraTransform;
    private Transform bodyTransform;

    void Awake()
    {
        cameraTransform = transform;
    }

    void Start()
    {
        var playerManager = GetComponentInParent<PlayerManager>();
        Debug.Assert(playerManager != null);

        bodyTransform = playerManager.PlayerController.BodyTransform;
    }

    void LateUpdate()
    {
        var tempPos = bodyTransform.position;
        tempPos.y += offsetY;
        tempPos.z = cameraTransform.position.z;
        cameraTransform.position = tempPos;
    }
}
