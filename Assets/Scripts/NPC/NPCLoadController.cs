using UnityEngine;
using Utils;

[DisallowMultipleComponent]
[RequireComponent(typeof(NPCController))]
[RequireComponent(typeof(NPCTaskManager))]
public class NPCLoadController : MonoBehaviour
{
    private const float _distanceCooldownSeconds = 0.3f;

    [SerializeField] private float _maxPlayerDistance = 25f;

    private NPCController _npc;
    private NPCTaskManager _taskManager;
    private Cooldown _distanceCooldown;
    private bool _isActive = true;

    void Start()
    {
        _npc = GetComponent<NPCController>();
        _taskManager = GetComponent<NPCTaskManager>();
        _distanceCooldown = new Cooldown(_distanceCooldownSeconds);
    }

    void FixedUpdate()
    {
        if (!_distanceCooldown.ResetIfExpired())
            return;

        var playerPos = PlayerController.GetInstance().Position;
        var npcPos = _npc.Position;
        var isInRange = MathExt.ManhattanDistance(playerPos, npcPos) <= _maxPlayerDistance;
        if (isInRange == _isActive)
            return;

        _npc.enabled = isInRange;
        _taskManager.enabled = isInRange;
        _isActive = isInRange;
    }
}