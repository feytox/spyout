using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Контроллер, реализующий базовые шаблоны поведения NPC.
/// Использует перечисление <see cref="BasicBehavior"/> для выбора конкретного поведения.
/// </summary>
public class BasicBehaviorController : NPCBehaviorController
{
    /// <summary>
    /// Выбранный базовый шаблон поведения для NPC.
    /// </summary>
    [SerializeField] private BasicBehavior _npcBehavior = BasicBehavior.None;

    /// <summary>
    /// Определяет, следует ли перезагружать задачи для текущего поведения.
    /// Возвращает true для поведений, требующих постоянного обновления (например, следование).
    /// </summary>
    public override bool Reloadable => _npcBehavior switch
    {
        BasicBehavior.FollowPlayer => true,
        BasicBehavior.FollowAndAttack => true,
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
            BasicBehavior.None => Array.Empty<NPCTask>(),
            
            BasicBehavior.FollowPlayer => FollowTask.OfPlayer(taskData).Yield(),
            
            BasicBehavior.FollowAndAttack => new NPCTask[]
                { FollowTask.OfPlayer(taskData), AttackTask<PlayerController>.OfPlayer(taskData) },
            
            _ => throw new ArgumentOutOfRangeException(nameof(_npcBehavior), _npcBehavior, null)
        };
    }
}

/// <summary>
/// Перечисление базовых шаблонов поведения NPC.
/// </summary>
public enum BasicBehavior : byte
{
    None = 0,
    
    // tests
    FollowPlayer = 42,
    FollowAndAttack = 43
}