using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BoatController : MonoBehaviour, IPlayerInteractable
{
    private Rigidbody2D rb;
    private PopupController popup;
    private const float boatSpeed = 1.5f;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        popup = GetComponentInChildren<PopupController>();
    }

    // void FixedUpdate()
    // {
    //     rb.linearVelocity = Vector2.up * boatSpeed;
    // }

    public void Interact()
    {
        
    }

    public bool CanInteract() => true;

    public Vector3 Position => transform.position;

    PopupController IPlayerInteractable.Popup => popup;
}