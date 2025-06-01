using UnityEngine;
using Utils;

[DisallowMultipleComponent]
[RequireComponent(typeof(NpcController))]
[RequireComponent(typeof(NpcTaskManager))]
public class NpcLoadController : MonoBehaviour
{
    private const float DistanceCooldownSeconds = 0.3f;

    [SerializeField] private float _maxPlayerDistance = 25f;

    private NpcController _npc;
    private NpcTaskManager _taskManager;
    private Cooldown _distanceCooldown;
    private bool _isActive = true;

    private void Start()
    {
        _npc = GetComponent<NpcController>();
        _taskManager = GetComponent<NpcTaskManager>();
        _distanceCooldown = new Cooldown(DistanceCooldownSeconds);
    }

    private void FixedUpdate()
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