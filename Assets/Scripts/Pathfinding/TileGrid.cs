#nullable enable
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileGrid
{
    private static readonly (int, int)[] CellNeighbours = { (0, 1), (0, -1), (1, 0), (-1, 0) };
    
    private readonly Vector3Int _min;
    private readonly Vector3Int _max; 
    
    // maybe use Vector2Int, idk
    private readonly Dictionary<Vector3Int, ITileInfo> _tilesData;

    private TileGrid(Dictionary<Vector3Int, ITileInfo> tilesData, Vector3Int min, Vector3Int max)
    {
        _tilesData = tilesData;
        _min = min;
        _max = max;
    }

    // maybe use 8 neighbours for diagonal movement
    public IEnumerable<Vector3Int> Get4Neighbours(GameObject walker, Vector3Int pos)
    {
        foreach (var (dx, dy) in CellNeighbours)
        {
            var neighbourPos = new Vector3Int(pos.x + dx, pos.y + dy, pos.z);
            if (IsWalkable(walker, neighbourPos))
                yield return neighbourPos;
        }
    }

    private bool IsWalkable(GameObject walker, Vector3Int pos)
    {
        if (_tilesData.TryGetValue(pos, out var info))
            return info.CanWalkThrough(walker);

        return pos.x >= _min.x && pos.y >= _min.y && pos.x <= _max.x && pos.y <= _max.y;
    }

    // maybe deprecate costs, idk
    public int GetCost(Vector3Int pos)
    {
        return _tilesData.TryGetValue(pos, out var info) ? info.Cost : 1;
    }

    public static TileGrid Parse(Tilemap[] tilemaps)
    {
        var tilesData = new Dictionary<Vector3Int, ITileInfo>();
        var min = new Vector3Int(int.MaxValue, int.MaxValue, int.MaxValue);
        var max = new Vector3Int(int.MinValue, int.MinValue, int.MinValue);

        foreach (var tilemap in tilemaps)
        {
            tilemap.CompressBounds();
            min = min.MinXYZ(tilemap.cellBounds.min);
            max = max.MaxXYZ(tilemap.cellBounds.max);
            ParseTilemap(tilesData, tilemap);
        }

        return new TileGrid(tilesData, min, max);
    }

    private static void ParseTilemap(Dictionary<Vector3Int, ITileInfo> tilesData, Tilemap tilemap)
    {
        if (tilemap.TryGetComponent(out TilemapCollider2D _))
            ParseStaticTiles(tilesData, tilemap);
        
        ParseDynamicTiles(tilesData, tilemap);
    }

    private static void ParseStaticTiles(Dictionary<Vector3Int, ITileInfo> tilesData, Tilemap tilemap)
    {
        var bounds = tilemap.cellBounds;
        foreach (var pos in bounds.allPositionsWithin) // TODO: maybe use tilemap.GetTilesBlock()
        {
            var tile = tilemap.GetTile(pos);
            if (tile == null)
                continue;

            tilesData[pos] = new SimpleTileInfo(1, false);
        }
    }

    private static void ParseDynamicTiles(Dictionary<Vector3Int, ITileInfo> tilesData, Tilemap tilemap)
    {
        var dynamicTiles = tilemap.GetComponentsInChildren<IWalkable>();
        foreach (var tile in dynamicTiles)
        {
            if (tile is null)
                continue;

            var pos = tilemap.WorldToCell(tile.Position);
            tilesData[pos] = new TileInfo(1, tile.CanWalkThrough);
            Debug.Log(pos);
        }
    }
}