using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileGrid
{
    // maybe use Vector2Int, idk
    public Vector3Int Min { get; }
    public Vector3Int Max { get; }
    public int MaxTileCount { get; }

    private readonly Dictionary<Vector3Int, ITileInfo> tilesData;

    private TileGrid(Dictionary<Vector3Int, ITileInfo> tilesData, Vector3Int min, Vector3Int max)
    {
        this.tilesData = tilesData;
        Min = min;
        Max = max;
        MaxTileCount = (max.x - min.x + 1) * (max.y - min.y + 1);
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
        if (tilesData.TryGetValue(pos, out var info))
            return info.Walkable;

        return pos.x >= Min.x && pos.y >= Min.y && pos.x <= Max.x && pos.y <= Max.y;
    }

    // maybe deprecate costs, idk
    public int GetCost(Vector3Int pos)
    {
        return tilesData.TryGetValue(pos, out var info) ? info.Cost : 1;
    }

    // for tests
    public override string ToString()
    {
        var count = tilesData.Count;
        var nonWalkable = tilesData.Values.Count(info => !info.Walkable);
        return $"min: {Min} max: {Max} count: {count} non-walkable: {nonWalkable}";
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

            var walkable = tilemapWalkable && CanWalkThrough(tilemap.GetInstantiatedObject(pos));
            tilesData[pos] = new SimpleTileInfo(walkable, 1);
        }
    }

    private static bool CanWalkThrough([CanBeNull] GameObject gameObject)
    {
        // TODO: maybe change
        return gameObject == null || !gameObject.TryGetComponent(out Rigidbody2D rigidbody) || !rigidbody.simulated;
    }
}