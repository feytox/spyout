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
    
    public static IEnumerable<Vector2Int> FindPath(GameObject walker, Vector3 startPos, Vector3 endPos)
    {
        var instance = GetInstance();
        var start = instance.WorldToCell(startPos);
        var end = instance.WorldToCell(endPos);
        return PathFinder.FindAStarPath(walker, instance._tileGrid, start, end);
    }

    public static IEnumerable<Vector2Int> FindPath(GameObject walker, Vector2Int start, Vector2Int end)
    {
        var instance = GetInstance();
        return PathFinder.FindAStarPath(walker, instance._tileGrid, start, end);
    }

    public static Vector2Int WorldToGridCell(Vector3 position) => GetInstance().WorldToCell(position);
    
    public static Vector2 CellToWorld(Vector2Int cellPos) => GetInstance()._grid.CellToWorld((Vector3Int)cellPos);

    public static Vector2 CellToNormalWorld(Vector2Int cellPos) => CellToWorld(cellPos).ToCellCenter();

    private Vector2Int WorldToCell(Vector3 position) => (Vector2Int)_grid.WorldToCell(position);

    private static GridController _singleton;

    private static GridController GetInstance()
    {
        Debug.Assert(
            _singleton != null, $"Tried to access {nameof(GridController)} before it was initialized!"
        );
        return _singleton;
    }
}