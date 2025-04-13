using UnityEngine;

/// <summary>
/// Синхронизирует положение объекта с родителем.
/// </summary>
[DisallowMultipleComponent]
public class ParentPositionSync : MonoBehaviour
{
    void Start() => transform.SetParent(transform.parent.gameObject.transform);
}