using System;
using System.Collections.Generic;
using System.Linq;
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

    public IEnumerable<Vector2Int> FindPathOrClosest(GameObject walker, Vector2Int start, Vector2Int end,
        int maxPathLength = 5000)
    {
        if (_tileGrid.IsWalkable(walker, end))
            return PathFinder.FindAStarPath(walker, _tileGrid, start, end, maxPathLength);

        var closest = _tileGrid.Get4Neighbours(walker, end)
            .Cast<Vector2Int?>()
            .FirstOrDefault();

        return closest is null
            ? Enumerable.Empty<Vector2Int>()
            : PathFinder.FindAStarPath(walker, _tileGrid, start, closest.Value, maxPathLength);
    }

    [Obsolete]
    public static IEnumerable<Vector2Int> FindPath(GameObject walker, Vector3 startPos, Vector3 endPos)
    {
        var instance = GetInstance();
        var start = instance.WorldToCell(startPos);
        var end = instance.WorldToCell(endPos);
        return PathFinder.FindAStarPath(walker, instance._tileGrid, start, end, 5000);
    }

    #region Position Converters

    public Vector2Int WorldToCell(Vector3 position) => (Vector2Int)_grid.WorldToCell(position);

    public Vector2Int WorldToCell(Vector2 position) => (Vector2Int)_grid.WorldToCell(position);

    public Vector2 CellToNormalWorld(Vector2Int cellPos) => CellToWorld(cellPos).ToCellCenter();

    private Vector2 CellToWorld(Vector2Int cellPos) => _grid.CellToWorld((Vector3Int)cellPos);

    #endregion

    #region Singleton

    private static GridController _singleton;

    public static GridController GetInstance()
    {
        Debug.Assert(
            _singleton != null, $"Tried to access {nameof(GridController)} before it was initialized!"
        );
        return _singleton;
    }

    #endregion
}