using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PathFinder
{
    public static IEnumerable<Vector2Int> FindAStarPath(GameObject walker, TileGrid grid,
        Vector2Int start, Vector2Int end, int maxPathLength)
    {
        var ctx = new AStarContext(walker, grid, start, end);

        while (ctx.Frontier.Count > 0)
        {
            var current = ctx.Frontier.Dequeue();
            if (current == end)
                break;
            
            ctx.ProcessNeighbours(current, maxPathLength);
        }

        if (!ctx.Track.ContainsKey(end))
            return Enumerable.Empty<Vector2Int>();

        var result = new List<Vector2Int>();
        Vector2Int? point = end;
        while (point is not null)
        {
            result.Add(point.Value);
            point = ctx.Track[point.Value].CameFrom;
        }
        
        return Enumerable.Reverse(result).Skip(1);
    }

    private static int Heuristic(Vector2Int a, Vector2Int b) => Mathf.Abs(b.x - a.x) + Mathf.Abs(b.y - a.y);
    
    private class AStarContext
    {
        private readonly GameObject _walker;
        private readonly TileGrid _grid;
        private readonly Vector2Int _end;
        private readonly List<Vector2Int> _buffer;
        public readonly PriorityQueue<Vector2Int, int> Frontier;
        public readonly Dictionary<Vector2Int, PointData> Track;

        public AStarContext(GameObject walker, TileGrid grid, Vector2Int start, Vector2Int end)
        {
            _walker = walker;
            _grid = grid;
            _end = end;
            Frontier = new PriorityQueue<Vector2Int, int>();
            Frontier.Enqueue(start, 0);
            Track = new Dictionary<Vector2Int, PointData> { { start, new PointData(null, 0) } };
            _buffer = new List<Vector2Int>();
        }

        public void ProcessNeighbours(Vector2Int current, int maxPathLength)
        {
            var currentCost = Track[current].CostSoFar;
            _grid.Get8Neighbours(_walker, current, _buffer);
            foreach (var next in _buffer)
            {
                var newCost = currentCost + _grid.GetCost(next);
                if (newCost > maxPathLength)
                    continue;
                
                if (Track.TryGetValue(next, out var data) && newCost >= data.CostSoFar)
                    continue;

                var priority = newCost + Heuristic(_end, next);
                Frontier.Enqueue(next, priority);
                Track[next] = new PointData(current, newCost);
            }
        }
    }
    
    private struct PointData
    {
        public readonly Vector2Int? CameFrom;
        public readonly int CostSoFar;

        public PointData(Vector2Int? cameFrom, int costSoFar)
        {
            CameFrom = cameFrom;
            CostSoFar = costSoFar;
        }
    }
}