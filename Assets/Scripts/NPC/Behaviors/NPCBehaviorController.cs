using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(NPCTaskManager))]
public abstract class NPCBehaviorController : MonoBehaviour
{
    public abstract IEnumerable<NPCTask> CreateTasks(TaskData taskData);
}