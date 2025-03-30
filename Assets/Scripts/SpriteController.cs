using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteController : MonoBehaviour
{
    private PlayerController playerController;

    void Start()
    {
        var playerManager = GetComponentInParent<PlayerManager>();
        Debug.Assert(playerManager != null);

        playerController = playerManager.PlayerController;
        transform.SetParent(playerController.BodyTransform, false); // i think there's even better solution
    }
}
