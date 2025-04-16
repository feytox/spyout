using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[DisallowMultipleComponent]
[RequireComponent(typeof(Grid))]
public class GridController : MonoBehaviour
{
    public Tilemap[] obstacleTilemaps;

    private Grid _grid;
    private TileGrid _tileGrid;

    private void Awake()
    {
        Debug.Assert(
            _singleton == null, $"{gameObject.name} tried to awake {nameof(GridController)} second time."
        );
        _singleton = this;

        _grid = GetComponent<Grid>();
        RefreshGrid();
    }

    private void RefreshGrid() // maybe add refreshing after tile change
    {
        Debug.Assert(obstacleTilemaps.Length != 0);
        _tileGrid = TileGrid.Parse(obstacleTilemaps);
    }

    public static IEnumerable<Vector3Int> FindPath(GameObject walker, Vector3 startPos, Vector3 endPos)
    {
        var instance = GetInstance();
        var start = instance._grid.WorldToCell(startPos);
        var end = instance._grid.WorldToCell(endPos);
        return PathFinder.FindAStarPath(walker, instance._tileGrid, start, end);
    }

    private static GridController _singleton;

    private static GridController GetInstance()
    {
        Debug.Assert(
            _singleton != null, $"Tried to access {nameof(GridController)} before it was initialized!"
        );
        return _singleton;
    }
}