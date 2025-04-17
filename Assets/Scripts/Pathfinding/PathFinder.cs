using System.Collections.Generic;
using UnityEngine;

public static class PathFinder
{
    // https://www.redblobgames.com/pathfinding/a-star/introduction.html
    public static IEnumerable<Vector2Int> FindAStarPath(GameObject walker, TileGrid grid, 
        Vector2Int start, Vector2Int end)
    {
        var frontier = new PriorityQueue<Vector2Int, int>();
        frontier.Enqueue(end, 0); // no need to reverse if we start at the end
        var track = new Dictionary<Vector2Int, PointData> { { end, new PointData(null, 0) } };

        while (frontier.Count != 0)
        {
            var current = frontier.Dequeue();
            var currentCost = track[current].CostSoFar;

            if (current == start)
                break;

            ProcessNeighbours(walker, current, currentCost, start, frontier, grid, track);
        }

        Vector2Int? pos = start;
        while (pos is not null)
        {
            yield return pos.Value;
            pos = track[pos.Value].CameFrom;
        }
    }
    
    // TODO: less arguments count
    private static void ProcessNeighbours(GameObject walker, Vector2Int current, int currentCost, Vector2Int start,
        PriorityQueue<Vector2Int, int> frontier, TileGrid grid, Dictionary<Vector2Int, PointData> track)
    {
        foreach (var next in grid.Get4Neighbours(walker, current))
        {
            var newCost = currentCost + grid.GetCost(next);
            if (track.TryGetValue(next, out var data) && newCost >= data.CostSoFar)
                continue;

            var priority = newCost + Heuristic(start, next);
            frontier.Enqueue(next, priority);
            track[next] = new PointData(current, newCost);
        }
    }

    private static int Heuristic(Vector2Int pos1, Vector2Int pos2)
    {
        return Mathf.Abs(pos2.x - pos1.x) + Mathf.Abs(pos2.y - pos1.y);
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