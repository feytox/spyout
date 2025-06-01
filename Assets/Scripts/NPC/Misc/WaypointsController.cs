#nullable enable
using System;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class WaypointsController : MonoBehaviour
{
    public Lazy<Vector2Int[]> Points => new(ConvertPoints);

    private Vector2[]? _points;

    private void Awake()
    {
        var waypoints = GetComponentsInChildren<Waypoint>(true);
        _points = waypoints.Select(waypoint => waypoint.Position).ToArray();
    }

    private Vector2Int[] ConvertPoints()
    {
        Debug.Assert(_points is not null,
            $"Точки запрошены слишком рано, ещё до Awake в {nameof(WaypointsController)}");
        return _points!.Select(GridController.GetInstance().WorldToCell).ToArray();
    }
}