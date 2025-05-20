using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Collider2D))]
public class LargeTilemapObject : MonoBehaviour
{
    private Collider2D _collider;

    void Awake() => _collider = GetComponent<Collider2D>();

    public IEnumerable<Vector2Int> GetTilePoses(Tilemap tilemap)
    {
        var minPos = tilemap.WorldToCell(_collider.bounds.min);
        var maxPos = tilemap.WorldToCell(_collider.bounds.max);
        var layer = 1 << gameObject.layer;
        
        for (var x = minPos.x; x <= maxPos.x; x++)
        for (var y = minPos.y; y <= maxPos.y; y++)
        {
            var cellCenter = ((Vector2) tilemap.CellToWorld(new Vector3Int(x, y, 0))).ToCellCenter();
            if (_collider.OverlapPoint(cellCenter))
                yield return new Vector2Int(x, y);
            
            var foundCollider = Physics2D.OverlapBox(cellCenter, Vector2.one, 0, layer);
            if (foundCollider)
                yield return new Vector2Int(x, y);
        }
    }
}