using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BasicBehaviorController : NPCBehaviorController
{
    [SerializeField] private BasicBehavior _npcBehavior = BasicBehavior.None;

    public override bool Reloadable => _npcBehavior switch
    {
        BasicBehavior.FollowPlayer => true,
        BasicBehavior.FollowAndAttack => true,
        _ => false
    };

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

public enum BasicBehavior : byte
{
    None = 0,

    // tests
    FollowPlayer = 42,
    FollowAndAttack = 43
}