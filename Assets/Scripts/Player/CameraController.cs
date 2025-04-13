using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    private const float offsetY = 0.35f;
    private Transform cameraTransform;
    private Transform bodyTransform;

    void Awake()
    {
        cameraTransform = transform;
    }

    void Start()
    {
        // var playerManager = GetComponentInParent<PlayerManager>();
        // Debug.Assert(playerManager != null);

        bodyTransform = PlayerController.BodyTransform;
    }

    void LateUpdate()
    {
        var tempPos = bodyTransform.localPosition;
        tempPos.y += offsetY;
        tempPos.z = cameraTransform.localPosition.z;
        cameraTransform.localPosition = tempPos;
    }
}
