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

    // maybe use Vector2Int, idk
    private readonly Dictionary<Vector2Int, ITileInfo> _tilesData;

    private TileGrid(Dictionary<Vector2Int, ITileInfo> tilesData, Vector2Int min, Vector2Int max)
    {
        _tilesData = tilesData;
        _min = min;
        _max = max;
    }
    
    // TODO: ускорить???
    public void Get8Neighbours(GameObject walker, Vector2Int pos, List<Vector2Int> result)
    {
        result.Clear();
        
        foreach (var delta in CellNeighbours)
        {
            var neighbourPos = pos + delta;
            if (IsWalkable(walker, neighbourPos))
                result.Add(neighbourPos);
        }
        
        foreach (var delta in DiagonalNeighbours)
        {
            var firstNeighbour = new Vector2Int(pos.x + delta.x, pos.y);
            var secondNeighbour = new Vector2Int(pos.x, pos.y + delta.y);
            if (!result.Contains(firstNeighbour) || !result.Contains(secondNeighbour)) // < 8 elements, so it is fast
                return;
            
            var diagonalPos = pos + delta;
            if (IsWalkable(walker, diagonalPos))
                result.Add(diagonalPos);
        }
    }

    private bool IsWalkable(GameObject walker, Vector2Int pos)
    {
        if (_tilesData.TryGetValue(pos, out var info))
            return info.CanWalkThrough(walker);

        return pos.x >= _min.x && pos.y >= _min.y && pos.x <= _max.x && pos.y <= _max.y;
    }

    // maybe deprecate costs, idk
    public int GetCost(Vector2Int pos)
    {
        return _tilesData.TryGetValue(pos, out var info) ? info.Cost : 1;
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

            tilesData[pos.ToXY()] = new SimpleTileInfo(1, false);
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
            tilesData[pos] = new TileInfo(1, tile.CanWalkThrough);
        }
    }
}