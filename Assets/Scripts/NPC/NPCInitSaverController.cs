using JetBrains.Annotations;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(NPCController))]
public class NPCInitSaverController : MonoBehaviour
{
    [SerializeField] [CanBeNull] private NPCTaskManager _taskManager;

    private NPCController _npc;
    private Vector3 _position;

    public void ApplyInitData()
    {
        _npc.transform.position = _position;
        _taskManager?.ResetTasks();
    }

    void Start()
    {
        _npc = GetComponent<NPCController>();
        SaveInitData();
    }

    private void SaveInitData()
    {
        _position = _npc.Position;
    }
}