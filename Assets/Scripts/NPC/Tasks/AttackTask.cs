using System;
using JetBrains.Annotations;

public class AttackTask : NPCTask
{
    public AttackTask([NotNull] TaskData taskData) : base(taskData)
    {
    }

    public override bool Step()
    {
        throw new NotImplementedException();
    }

    public override NPCTask CreateNextTask(TaskData taskData)
    {
        throw new NotImplementedException();
    }
}