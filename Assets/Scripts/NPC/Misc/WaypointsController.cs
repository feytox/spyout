using System;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class WaypointsController : MonoBehaviour
{
    [NonSerialized] public Vector2[] Points;

    void Start()
    {
        var waypoints = GetComponentsInChildren<Waypoint>(true);
        Points = waypoints.Select(waypoint => waypoint.Position).ToArray();
    }
}