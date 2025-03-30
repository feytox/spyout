using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Grid
{
    // maybe use Vector2Int, idk
    public Vector3Int Min { get; }
    public Vector3Int Max { get; }

    private readonly Dictionary<Vector3Int, ITileInfo> tilesData;

    private Grid(Dictionary<Vector3Int, ITileInfo> tilesData, Vector3Int min, Vector3Int max)
    {
        this.tilesData = tilesData;
        Min = min;
        Max = max;
    }

    // for tests
    public override string ToString()
    {
        var count = tilesData.Count;
        var nonWalkable = tilesData.Values.Count(info => !info.Walkable);
        return $"min: {Min} max: {Max} count: {count} non-walkable: {nonWalkable}";
    }

    public static Grid Parse(Tilemap[] tilemaps)
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

        return new Grid(tilesData, min, max);
    }

    private static void ParseTilemap(Dictionary<Vector3Int, ITileInfo> tilesData, Tilemap tilemap)
    {
        var forceNotWalkable = !CanWalkThrough(tilemap.gameObject);

        var bounds = tilemap.cellBounds;
        foreach (var pos in bounds.allPositionsWithin) // maybe use tilemap.GetTilesBlock(), but it hasn't poses
        {
            var tile = tilemap.GetTile(pos);
            if (tile == null)
                continue;

            var walkable = !forceNotWalkable && CanWalkThrough(tilemap.GetInstantiatedObject(pos));
            tilesData[pos] = new SimpleTileInfo(walkable, 1); // STOPSHIP: fix double poses duplication problem
        }
    }

    private static bool CanWalkThrough([CanBeNull] GameObject gameObject)
    {
        // TODO: maybe change
        return gameObject == null || !gameObject.TryGetComponent(out Rigidbody2D rigidbody) || !rigidbody.simulated;
    }
}