using UnityEngine;

[DisallowMultipleComponent]
public class Waypoint : MonoBehaviour
{
    public Vector2 Position => transform.position;

    private void Awake() => gameObject.SetActive(false);
}