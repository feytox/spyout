using UnityEngine;

[DisallowMultipleComponent]
public class ParentPositionSync : MonoBehaviour
{
    void Start() => transform.SetParent(transform.parent.gameObject.transform);
}