using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Контроллер, реализующий базовые шаблоны поведения NPC.
/// Использует перечисление <see cref="BasicBehaviorType"/> для выбора конкретного поведения.
/// </summary>
public class BasicBehavior : NPCBehavior
{
    /// <summary>
    /// Выбранный базовый шаблон поведения для NPC.
    /// </summary>
    [SerializeField] private BasicBehaviorType _npcBehavior = BasicBehaviorType.None;

    /// <summary>
    /// Определяет, следует ли перезагружать задачи для текущего поведения.
    /// Возвращает true для поведений, требующих постоянного обновления (например, следование).
    /// </summary>
    public override bool Reloadable => _npcBehavior switch
    {
        BasicBehaviorType.FollowPlayer => true,
        BasicBehaviorType.FollowAndAttack => true,
        _ => false
    };

    /// <summary>
    /// Создает задачи (<see cref="NPCTask"/>) в соответствии с выбранным <see cref="_npcBehavior"/>.
    /// </summary>
    /// <param name="taskData">Данные, необходимые для создания задач.</param>
    /// <returns>Перечисление задач для выполнения.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Выбрасывается, если значение <see cref="_npcBehavior"/> не поддерживается.</exception>
    public override IEnumerable<NPCTask> CreateTasks(TaskData taskData)
    {
        return _npcBehavior switch
        {
            BasicBehaviorType.None => Array.Empty<NPCTask>(),

            BasicBehaviorType.FollowPlayer => FollowTask<PlayerController>.OfPlayer(taskData, false).Yield(),

            BasicBehaviorType.FollowAndAttack => AttackTask<PlayerController>.OfPlayer(taskData, false).Yield(),

            _ => throw new ArgumentOutOfRangeException(nameof(_npcBehavior), _npcBehavior, null)
        };
    }
}

/// <summary>
/// Перечисление базовых шаблонов поведения NPC.
/// </summary>
public enum BasicBehaviorType : byte
{
    None = 0,

    // tests
    FollowPlayer = 42,
    FollowAndAttack = 43
}