using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Базовый абстрактный класс для контроллеров поведения NPC.
/// Определяет, какие задачи (<see cref="NPCTask"/>) будет выполнять NPC.
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(NPCTaskManager))]
public abstract class NPCBehaviorController : MonoBehaviour
{
    /// <summary>
    /// Определяет, следует ли периодически пересоздавать задачи после обработки всех задач.
    /// Если true, <see cref="NPCTaskManager"/> будет вызывать <see cref="CreateTasks"/> повторно.
    /// </summary>
    public abstract bool Reloadable { get; }

    /// <summary>
    /// Создает перечисление задач (<see cref="NPCTask"/>), которые должен выполнять NPC в рамках данного поведения.
    /// </summary>
    /// <param name="taskData">Данные, необходимые для создания задач (например, ссылки на NPC, цель и т.д.).</param>
    /// <returns>Перечисление задач для выполнения.</returns>
    public abstract IEnumerable<NPCTask> CreateTasks(TaskData taskData);
}