using System;
using System.Collections.Generic;
using UnityEngine;

public class BasicBehaviorController : NPCBehaviorController
{
    [SerializeField] private BasicBehavior _npcBehavior = BasicBehavior.None;

    public override IEnumerable<NPCTask> CreateTasks(TaskData taskData)
    {
        return _npcBehavior switch
        {
            BasicBehavior.None => Array.Empty<NPCTask>(),
            BasicBehavior.FollowPlayer => new NPCTask[] { new FollowPlayerTask(taskData) },
            _ => throw new ArgumentOutOfRangeException(nameof(_npcBehavior), _npcBehavior, null)
        };
    }
}

public enum BasicBehavior : byte
{
    None = 0,

    // tests
    FollowPlayer = 42
}