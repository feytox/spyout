#nullable enable
public abstract class NpcTask
{
    protected readonly NpcController Npc;
    private readonly INpcTaskScheduler _taskScheduler;
    private bool _isStarted;

    protected NpcTask(TaskData taskData)
    {
        Npc = taskData.Npc;
        _taskScheduler = taskData.TaskScheduler;
    }

    /// <summary>
    /// Совершает очередное действие задачи во время FixedUpdate.
    /// </summary>
    /// <returns>true, если выполнение задачи завершено, false иначе (в том числе, если прервано)</returns>
    public abstract bool Step();

    /// <summary>
    /// Создаёт следующую задачу в последовательности.
    /// </summary>
    /// <param name="taskData">Данные, необходимые для создания следующей задачи.</param>
    /// <returns>Следующая задача или null, если последовательность завершена.</returns>
    public abstract NpcTask? CreateNextTask(TaskData taskData);

    /// <summary>
    /// Пытается запустить задачу, если она ещё не была запущена.
    /// Вызывает <see cref="OnTaskStart"/>, если задача запускается.
    /// </summary>
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
    protected bool OnInterrupted(NpcTask interrupter)
    {
        _taskScheduler.PushTask(interrupter);
        _isStarted = false;
        return false;
    }

    /// <summary>
    /// Срабатывает при старте задачи, в том числе, если до этого задача была прервана.
    /// </summary>
    protected virtual void OnTaskStart()
    {
    }
}

public class TaskData
{
    public NpcController Npc { get; }
    public INpcTaskScheduler TaskScheduler { get; }

    public TaskData(INpcTaskScheduler taskScheduler, NpcController npc)
    {
        TaskScheduler = taskScheduler;
        Npc = npc;
    }
}