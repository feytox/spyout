using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Базовый абстрактный класс для контроллеров поведения NPC.
/// Определяет, какие задачи (<see cref="NpcTask"/>) будет выполнять NPC.
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(NpcTaskManager))]
public abstract class NpcBehavior : MonoBehaviour
{
    /// <summary>
    /// Определяет, следует ли периодически пересоздавать задачи после обработки всех задач.
    /// Если true, <see cref="NpcTaskManager"/> будет вызывать <see cref="CreateTasks"/> повторно.
    /// </summary>
    public abstract bool Reloadable { get; }

    /// <summary>
    /// Создает перечисление задач (<see cref="NpcTask"/>), которые должен выполнять NPC в рамках данного поведения.
    /// </summary>
    /// <param name="taskData">Данные, необходимые для создания задач (например, ссылки на NPC, цель и т.д.).</param>
    /// <returns>Перечисление задач для выполнения.</returns>
    public abstract IEnumerable<NpcTask> CreateTasks(TaskData taskData);
}