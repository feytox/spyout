#nullable enable
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileGrid
{
    private static readonly (int, int)[] CellNeighbours = { (0, 1), (0, -1), (1, 0), (-1, 0) };
    
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

    // maybe use 8 neighbours for diagonal movement
    public IEnumerable<Vector2Int> Get4Neighbours(GameObject walker, Vector2Int pos)
    {
        foreach (var (dx, dy) in CellNeighbours)
        {
            var neighbourPos = new Vector2Int(pos.x + dx, pos.y + dy);
            if (IsWalkable(walker, neighbourPos))
                yield return neighbourPos;
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