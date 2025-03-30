using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteController : MonoBehaviour
{
    public float offsetY = 0.35f;
    private Transform spriteTransform;
    private PlayerController playerController;
    private Transform bodyTransform;

    void Awake()
    {
        spriteTransform = transform;
    }

    void Start()
    {
        var playerManager = GetComponentInParent<PlayerManager>();
        Debug.Assert(playerManager != null);

        playerController = playerManager.PlayerController;
        bodyTransform = playerController.BodyTransform;
    }

    // Not sure that it will work properly cuz theres no latefixedupdate
    void Update()
    {
        var tempPos = bodyTransform.position;
        tempPos.y += offsetY;
        tempPos.z = spriteTransform.position.z;
        spriteTransform.position = tempPos;
    }
}
