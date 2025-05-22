using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PathFinder
{
    public static bool IsPathVisible(IWalker walker, TileGrid grid, Vector2Int start, Vector2Int end, int maxPathLength)
    {
        if ((end - start).sqrMagnitude > maxPathLength * maxPathLength)
            return false;

        return VectorsExtensions.IterateLine(start, end).All(pos => grid.CanSeeThrough(walker, pos));
    }
    
    public static IEnumerable<Vector2Int> FindAStarPath(IWalker walker, TileGrid grid,
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
    
    private class AStarContext
    {
        private readonly IWalker _walker;
        private readonly TileGrid _grid;
        private readonly Vector2Int _end;
        public readonly PriorityQueue<Vector2Int, int> Frontier;
        public readonly Dictionary<Vector2Int, PointData> Track;

        public AStarContext(IWalker walker, TileGrid grid, Vector2Int start, Vector2Int end)
        {
            _walker = walker;
            _grid = grid;
            _end = end;
            Frontier = new PriorityQueue<Vector2Int, int>();
            Frontier.Enqueue(start, 0);
            Track = new Dictionary<Vector2Int, PointData> { { start, new PointData(null, 0) } };
        }

        public void ProcessNeighbours(Vector2Int current, int maxPathLength)
        {
            var currentCost = Track[current].CostSoFar;
            foreach (var next in _grid.Get8Neighbours(_walker, current))
            {
                var newCost = currentCost + 1;
                if (newCost > maxPathLength)
                    continue;
                
                if (Track.TryGetValue(next, out var data) && newCost >= data.CostSoFar)
                    continue;

                var priority = newCost + MathExt.ManhattanDistance(_end, next);
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