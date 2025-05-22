#nullable enable
using System.Collections.Generic;
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
    private readonly bool _endWhenNoTarget;
    private readonly Cooldown _targetUpdateCooldown = new(TargetUpdateCooldown);
    private readonly OverflowBuffer<Vector2> _currentPath = new();

    protected bool NoTarget;
    private GridController? _grid;
    private Vector2Int? _targetPos;
    private Task? _pathUpdateTask;

    protected FollowTask(TaskData taskData, T target, bool endWhenNoTarget) : base(taskData)
    {
        Target = target;
        _endWhenNoTarget = endWhenNoTarget;
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
        if (Target.IsDead)
            return _endWhenNoTarget;

        if (!IsUpdatingCompleted())
            return false;

        if (UpdatePath())
            return _endWhenNoTarget;

        return MoveByPath() && NoTarget && _endWhenNoTarget;
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
        return RefreshPath(_grid!.WorldToCell(NPC.transform.position));
    }

    private bool RefreshPath(Vector2Int currentPos)
    {
        var start = _currentPath.TryGetOrLast(LockedPathPoints - 1, out var lastPathPoint)
            ? _grid!.WorldToCell(lastPathPoint)
            : currentPos;

        _pathUpdateTask = CreatePathUpdateTask(start);
        return false;
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    private Task CreatePathUpdateTask(Vector2Int start)
    {
        return Task.Run(() => FindPath(start), NPC.destroyCancellationToken)
            .ContinueWith(OnUpdatePathComplete, TaskScheduler.FromCurrentSynchronizationContext());
    }

    private IEnumerable<Vector2> FindPath(Vector2Int start)
    {
        var targetPos = _targetPos!.Value;
        if (_grid!.IsPathVisible(NPC, start, targetPos, NPC.MaxPathLength))
            return _grid
                .FindPathOrClosest(NPC, start, targetPos, NPC.MaxPathLength)
                .Select(_grid.CellToNormalWorld);
        
        return Enumerable.Empty<Vector2>();
    }

    private void OnUpdatePathComplete(Task<IEnumerable<Vector2>> task)
    {
        if (task.Exception is not null)
        {
            Debug.LogException(task.Exception);
            return;
        }
        
        if (!task.IsCompletedSuccessfully)
            return;
        
        var path = task.Result.ToArray();
        if (path.Length == 0)
        {
            NoTarget = true;
            return;
        }

        NoTarget = false;
        _currentPath.Trim(LockedPathPoints);
        _currentPath.EnqueueRange(path);
    }
    
    /// <summary>
    /// Двигает NPC по пути
    /// </summary>
    /// <returns>true, если путь закончился</returns>
    private bool MoveByPath()
    {
        while (true)
        {
            if (!_currentPath.TryPeek(out var currentTarget))
                return true;

            if (!NPC.MoveToTarget(currentTarget))
                break;

            _currentPath.Dequeue();
        }

        return false;
    }

    private bool IsUpdatingCompleted() => _pathUpdateTask is null || _pathUpdateTask.IsCompleted;

    public override NPCTask? CreateNextTask(TaskData taskData) => null;
}