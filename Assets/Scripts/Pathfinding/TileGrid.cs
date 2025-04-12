#nullable enable
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileGrid
{
    // maybe deprecate
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
    public IEnumerable<Vector3Int> Get4Neighbours(Vector3Int pos)
    {
        for (var dx = -1; dx <= 1; dx++)
        for (var dy = -1; dy <= 1; dy++)
        {
            if (!(dx == 0 ^ dy == 0))
                continue;

            var neighbourPos = new Vector3Int(pos.x + dx, pos.y + dy, pos.z);
            if (IsWalkable(neighbourPos))
                yield return neighbourPos;
        }
    }

    private bool IsWalkable(Vector3Int pos)
    {
        if (_tilesData.TryGetValue(pos, out var info))
            return info.Walkable;

        return pos.x >= _min.x && pos.y >= _min.y && pos.x <= _max.x && pos.y <= _max.y;
    }

    // maybe deprecate costs, idk
    public int GetCost(Vector3Int pos)
    {
        return _tilesData.TryGetValue(pos, out var info) ? info.Cost : 1;
    }

    // for tests
    public override string ToString()
    {
        var count = _tilesData.Count;
        var nonWalkable = _tilesData.Values.Count(info => !info.Walkable);
        return $"min: {_min} max: {_max} count: {count} non-walkable: {nonWalkable}";
    }

    public static TileGrid Parse(Tilemap[] tilemaps)
    {
        var tilesData = new Dictionary<Vector3Int, ITileInfo>();
        var min = new Vector3Int(int.MaxValue, int.MaxValue, int.MaxValue);
        var max = new Vector3Int(int.MinValue, int.MinValue, int.MinValue);

        foreach (var tilemap in tilemaps)
        {
            tilemap.CompressBounds();
            min = min.Min(tilemap.cellBounds.min);
            max = max.Max(tilemap.cellBounds.max);
            ParseTilemap(tilesData, tilemap);
        }

        return new TileGrid(tilesData, min, max);
    }

    private static void ParseTilemap(Dictionary<Vector3Int, ITileInfo> tilesData, Tilemap tilemap)
    {
        var tilemapWalkable = CanWalkThrough(tilemap.gameObject);

        var bounds = tilemap.cellBounds;
        foreach (var pos in bounds.allPositionsWithin) // TODO: maybe use tilemap.GetTilesBlock()
        {
            var tile = tilemap.GetTile(pos);
            if (tile == null)
                continue;

            tilesData[pos] = tilemapWalkable
                ? new TileInfo(1, () => CanWalkThrough(tilemap.GetInstantiatedObject(pos)))
                : new SimpleTileInfo(1, false);
        }
    }

    private static bool CanWalkThrough(GameObject? gameObject)
    {
        // TODO: change after merging with doors
        return gameObject == null || !gameObject.TryGetComponent(out Rigidbody2D rigidbody) || !rigidbody.simulated;
    }
}