using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Grid))]
public class GridController : MonoBehaviour
{
    private static GridController singleton;

    public Tilemap[] obstacleTilemaps;

    private Grid grid;
    private TileGrid tileGrid;

    private void Awake()
    {
        Debug.Assert(singleton == null, $"{gameObject.name} tried to awake {nameof(GridController)} second time.");
        singleton = this;

        grid = GetComponent<Grid>();
        RefreshGrid();
    }

    private void RefreshGrid() // maybe add refreshing after tile change
    {
        Debug.Assert(obstacleTilemaps.Length != 0);
        tileGrid = TileGrid.Parse(obstacleTilemaps);
    }

    public static IEnumerable<Vector3Int> FindPath(Vector3 startPos, Vector3 endPos)
    {
        var instance = GetInstance();
        var start = instance.grid.WorldToCell(startPos);
        var end = instance.grid.WorldToCell(endPos);
        return PathFinder.FindAStarPath(instance.tileGrid, start, end);
    }

    private static GridController GetInstance()
    {
        Debug.Assert(singleton != null, $"Tried to access {nameof(GridController)} before it was initialized.");
        return singleton;
    }
}