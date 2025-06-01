using JetBrains.Annotations;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(NpcController))]
public class NpcInitSaverController : MonoBehaviour
{
    [SerializeField] [CanBeNull] private NpcTaskManager _taskManager;

    private NpcController _npc;
    private Vector3 _position;

    public void ApplyInitData()
    {
        _npc.transform.position = _position;
        _taskManager?.ResetTasks();
    }

    private void Start()
    {
        _npc = GetComponent<NpcController>();
        SaveInitData();
    }

    private void SaveInitData()
    {
        _position = _npc.Position;
    }
}