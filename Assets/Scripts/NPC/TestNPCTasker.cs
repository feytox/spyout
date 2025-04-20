using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(NPCController))]
public class TestNPCTasker : MonoBehaviour
{
    private static readonly Vector2[] DeltaPoses = { new(0, 2), new(2, 0), new(0, -2), new(-2, 0)};

    [SerializeField] private float _movementCooldown = 1.5f;

    private NPCController _npc;

    private float _startTime;
    private int _currentDeltaPos;

    void Start()
    {
        _npc = GetComponent<NPCController>();
    }

    void FixedUpdate()
    {
        if (Time.time - _startTime < _movementCooldown)
            return;

        _startTime = Time.time;

        var currentPos = (Vector2)_npc.transform.position;
        _currentDeltaPos = (_currentDeltaPos + 1) % DeltaPoses.Length;
        _npc.CurrentTarget = currentPos + DeltaPoses[_currentDeltaPos];
    }
}