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
    private NPCBehavior? _npcBehavior;
    private TaskData? _taskData;

    void Start()
    {
        _npc = GetComponent<NPCController>();
        _taskData = new TaskData(this, _npc);
        _npcBehavior = GetComponent<NPCBehavior>();
        Debug.Assert(_npcBehavior != null,
            $"You need to add {nameof(NPCBehavior)} to this NPC (at least {nameof(BasicBehavior)})");

        LoadTasks(true);
    }

    public void ResetTasks()
    {
        _taskStack.Clear();
        LoadTasks(true);
    }

    private void LoadTasks(bool force = false)
    {
        if (_npcBehavior is null)
            return;

        if (!force && !_npcBehavior.Reloadable)
            return;

        foreach (var task in _npcBehavior.CreateTasks(_taskData).Reverse())
            PushTask(task);
    }

    private void FixedUpdate()
    {
        if ((_npc! as ICharacter).IsDead)
            return;

        if (!_taskStack.TryPeek(out var currentTask))
        {
            LoadTasks();
            return;
        }

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
    /// <summary>
    /// Добавляет задачу в стек задач NPC.
    /// </summary>
    /// <param name="task">Задача для добавления.</param>
    public void PushTask(NPCTask task);
}