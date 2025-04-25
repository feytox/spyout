#nullable enable
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(NPCController))]
public class NPCTaskManager : MonoBehaviour, INPCTaskScheduler
{
    private readonly Stack<NPCTask> _taskStack = new();
    private NPCController? _npc;
    private TaskData? _taskData;

    void Start()
    {
        _npc = GetComponent<NPCController>();
        _taskData = new TaskData(this, _npc);

        var npcBehavior = GetComponent<NPCBehaviorController>();
        Debug.Assert(npcBehavior != null,
            $"You need to add {nameof(NPCBehaviorController)} to this NPC (at least {nameof(BasicBehaviorController)})");
        if (npcBehavior == null)
            return;

        foreach (var task in npcBehavior.CreateTasks(_taskData).Reverse())
            PushTask(task);
    }

    private void FixedUpdate()
    {
        if (!_taskStack.TryPeek(out var currentTask))
            return;

        currentTask.TryStartTask();
        if (!currentTask.Step())
            return;

        _taskStack.Pop();
        var next = currentTask.CreateNextTask(_taskData!);
        if (next is not null)
            _taskStack.Push(next);
    }

    public void PushTask(NPCTask task) => _taskStack.Push(task);
}

public interface INPCTaskScheduler
{
    public void PushTask(NPCTask task);
}