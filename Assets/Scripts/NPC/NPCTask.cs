#nullable enable
public abstract class NPCTask
{
    protected readonly NPCController NPC;
    private readonly INPCTaskScheduler _taskScheduler;
    private bool _isStarted;
    
    protected NPCTask(TaskData taskData)
    {
        NPC = taskData.NPC;
        _taskScheduler = taskData.TaskScheduler;
    }
    
    /// <summary>
    /// Совершает очередное действие задачи во время FixedUpdate.
    /// </summary>
    /// <returns>true, если выполнение задачи завершено, false иначе (в том числе, если прервано)</returns>
    public abstract bool Step();

    public abstract NPCTask? CreateNextTask(TaskData taskData);

    public void TryStartTask()
    {
        if (_isStarted)
            return;
        
        OnTaskStart();
        _isStarted = true;
    }
    
    /// <summary>
    /// Ставит в работу задачу-прерыватель.
    /// Следует использовать в <see cref="Step"/>
    /// </summary>
    /// <param name="interrupter">Задача, из-за которой была прервана текущая</param>
    /// <returns>false для правильной обработки <see cref="Step"/></returns>
    protected bool OnInterrupted(NPCTask interrupter)
    {
        _taskScheduler.PushTask(interrupter);
        _isStarted = false;
        return false;
    }
    
    /// <summary>
    /// Срабатывает при старте задачи, в том числе, если до этого задача была прервана.
    /// </summary>
    protected virtual void OnTaskStart() {}
}

public class TaskData
{
    public NPCController NPC { get; }
    public INPCTaskScheduler TaskScheduler { get; }
    
    public TaskData(INPCTaskScheduler taskScheduler, NPCController npc)
    {
        TaskScheduler = taskScheduler;
        NPC = npc;
    }
}