using System;

public enum NPCBehavior : byte
{
    None = 0,

    // tests
    FollowPlayer = 42
}

public static class NPCBehaviorExt
{
    public static NPCTask[] CreateTasks(this NPCBehavior npcBehavior, TaskData taskData)
    {
        return npcBehavior switch
        {
            NPCBehavior.None => Array.Empty<NPCTask>(),
            NPCBehavior.FollowPlayer => new NPCTask[] { new FollowPlayerTask(taskData) },
            _ => throw new ArgumentOutOfRangeException(nameof(npcBehavior), npcBehavior, null)
        };
    }
}