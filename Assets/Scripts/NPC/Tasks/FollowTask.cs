#nullable enable
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Utils;

/// <summary>
/// Задача следования за целью с поиском пути
/// </summary>
public class FollowTask<T> : NPCTask where T : IPositionProvider
{
    // TODO: move const to somewhere else
    private const int LockedPathPoints = 1;
    private const float TargetUpdateCooldown = 0.2f;

    protected readonly T Target;
    protected readonly bool EndWhenNoTarget;
    private readonly Cooldown _targetUpdateCooldown = new(TargetUpdateCooldown);
    private readonly OverflowBuffer<Vector2> _currentPath = new();

    private GridController? _grid;
    private Vector2Int? _targetPos;
    private Task? _pathUpdateTask;

    protected FollowTask(TaskData taskData, T target, bool endWhenNoTarget) : base(taskData)
    {
        Target = target;
        EndWhenNoTarget = endWhenNoTarget;
    }

    public static FollowTask<PlayerController> OfPlayer(TaskData taskData, bool endWhenNoTarget)
    {
        return new FollowTask<PlayerController>(taskData, PlayerController.GetInstance(), endWhenNoTarget);
    }

    protected override void OnTaskStart()
    {
        _currentPath.Clear();
        _grid = GridController.GetInstance();
    }

    public override bool Step()
    {
        if (Target.IsDead || UpdatePath())
            return EndWhenNoTarget;

        MoveByPath();
        return false;
    }
    
    private bool UpdatePath()
    {
        if (_targetPos is null)
            _targetUpdateCooldown.Reset();
        else if (!_targetUpdateCooldown.ResetIfExpired())
            return false;

        if (NPC.IsTargetReached(Target))
            return true;

        var newTargetPos = _grid!.WorldToCell(Target.Position);
        if (newTargetPos == _targetPos)
            return _currentPath.Count == 0 && IsUpdatingCompleted();

        if (!IsUpdatingCompleted())
            return false;

        _targetPos = newTargetPos;
        _currentPath.Trim(LockedPathPoints);
        return RefreshPath(_grid!.WorldToCell(NPC.transform.position));
    }

    private bool RefreshPath(Vector2Int currentPos)
    {
        var start = _currentPath.TryPeekLast(out var lastPathPoint)
            ? _grid!.WorldToCell(lastPathPoint)
            : currentPos;

        _pathUpdateTask = CreatePathUpdateTask(start);
        return false;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private Task CreatePathUpdateTask(Vector2Int start)
    {
        return Task.Run(() => _grid!
                .FindPathOrClosest(NPC, start, _targetPos!.Value, NPC.MaxPathLength)
                .Select(_grid.CellToNormalWorld), NPC.destroyCancellationToken)
            .ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                    _currentPath.EnqueueRange(task.Result.ToArray());
                else if (task.Exception is not null)
                    Debug.LogException(task.Exception);
            }, TaskScheduler.FromCurrentSynchronizationContext());
    }

    private void MoveByPath()
    {
        while (true)
        {
            if (!_currentPath.TryPeek(out var currentTarget))
                return;

            if (!NPC.MoveToTarget(currentTarget))
                break;

            _currentPath.Dequeue();
        }
    }

    private bool IsUpdatingCompleted() => _pathUpdateTask is null || _pathUpdateTask.IsCompleted;

    public override NPCTask? CreateNextTask(TaskData taskData) => null;
}