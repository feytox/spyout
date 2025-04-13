using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class CharacterManager : MonoBehaviour
{
    public float MoveSpeed = 2.5f;
    public float MaxHealth = 100f;
    public float Health;
    private new Rigidbody rigidbody;
    private new Collider collider;

    [SerializeField]
    private Vector2 SpriteOffset;

    protected void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }
    // void OnEnable() { }
    // void OnDisable() { }
    // void Start() { }
    // void FixedUpdate() { }
    // void Update() { }
    // void LateUpdate() { }
}
