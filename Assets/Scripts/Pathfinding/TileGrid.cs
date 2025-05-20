#nullable enable
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileGrid
{
    private static readonly Vector2Int[] CellNeighbours = { new(0, 1), new(0, -1), new(1, 0), new(-1, 0) };
    private static readonly Vector2Int[] DiagonalNeighbours = { new(1, 1), new(-1, 1), new(1, -1), new(-1, -1) };

    private readonly Vector2Int _min;
    private readonly Vector2Int _max;
    
    private readonly Dictionary<Vector2Int, ITileInfo> _tilesData;

    private TileGrid(Dictionary<Vector2Int, ITileInfo> tilesData, Vector2Int min, Vector2Int max)
    {
        _tilesData = tilesData;
        _min = min;
        _max = max;
    }

    public IEnumerable<Vector2Int> Get8Neighbours(IWalker walker, Vector2Int pos)
    {
        foreach (var delta in CellNeighbours)
        {
            var tilePos = pos + delta;
            if (IsWalkable(walker, tilePos))
                yield return tilePos;
        }

        foreach (var delta in DiagonalNeighbours)
        {
            var tilePos = pos + delta;
            if (!IsWalkable(walker, tilePos))
                continue;
            if (IsWalkable(walker, pos + delta.WithX(0)) && IsWalkable(walker, pos + delta.WithY(0)))
                yield return tilePos;
        }
    }

    public IEnumerable<Vector2Int> Get4Neighbours(IWalker walker, Vector2Int pos)
    {
        foreach (var delta in CellNeighbours)
            if (IsWalkable(walker, pos + delta))
                yield return pos + delta;
    }

    public bool IsWalkable(IWalker walker, Vector2Int pos)
    {
        if (_tilesData.TryGetValue(pos, out var info))
            return info.CanWalkThrough(walker);

        return pos.x >= _min.x && pos.y >= _min.y && pos.x <= _max.x && pos.y <= _max.y;
    }

    public static TileGrid Parse(Tilemap[] tilemaps)
    {
        var tilesData = new Dictionary<Vector2Int, ITileInfo>();
        var min = new Vector2Int(int.MaxValue, int.MaxValue);
        var max = new Vector2Int(int.MinValue, int.MinValue);

        foreach (var tilemap in tilemaps)
        {
            tilemap.CompressBounds();
            min = min.MinXY(tilemap.cellBounds.min);
            max = max.MaxXY(tilemap.cellBounds.max);
            ParseTilemap(tilesData, tilemap);
        }

        return new TileGrid(tilesData, min, max);
    }

    private static void ParseTilemap(Dictionary<Vector2Int, ITileInfo> tilesData, Tilemap tilemap)
    {
        if (tilemap.TryGetComponent(out TilemapCollider2D _))
            ParseStaticTiles(tilesData, tilemap);

        ParseDynamicTiles(tilesData, tilemap);
    }

    private static void ParseStaticTiles(Dictionary<Vector2Int, ITileInfo> tilesData, Tilemap tilemap)
    {
        var bounds = tilemap.cellBounds;
        foreach (var pos in bounds.allPositionsWithin) // TODO: maybe use tilemap.GetTilesBlock()
        {
            var tile = tilemap.GetTile(pos);
            if (tile == null)
                continue;

            tilesData[pos.ToXY()] = new SimpleTileInfo(false);
        }
    }

    private static void ParseDynamicTiles(Dictionary<Vector2Int, ITileInfo> tilesData, Tilemap tilemap)
    {
        var dynamicTiles = tilemap.GetComponentsInChildren<IWalkable>();
        foreach (var tile in dynamicTiles)
        {
            if (tile is null)
                continue;

            var pos = tilemap.WorldToCell(tile.Position).ToXY();
            tilesData[pos] = new TileInfo(tile.CanWalkThrough);
        }
    }
}