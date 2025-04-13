using System.Collections.Generic;
using UnityEngine;

public static class PathFinder
{
    // https://www.redblobgames.com/pathfinding/a-star/introduction.html
    public static IEnumerable<Vector3Int> FindAStarPath(TileGrid grid, Vector3Int start, Vector3Int end)
    {
        var frontier = new PriorityQueue<Vector3Int, int>();
        frontier.Enqueue(end, 0); // no need to reverse if we start at the end
        var track = new Dictionary<Vector3Int, PointData> { { end, new PointData(null, 0) } };

        while (frontier.Count != 0)
        {
            var current = frontier.Dequeue();
            var currentCost = track[current].CostSoFar;

            if (current == start)
                break;

            ProcessNeighbours(current, currentCost, start, frontier, grid, track);
        }

        Vector3Int? pos = start;
        while (pos is not null)
        {
            yield return pos.Value;
            pos = track[pos.Value].CameFrom;
        }
    }

    private static void ProcessNeighbours(Vector3Int current, int currentCost, Vector3Int start,
        PriorityQueue<Vector3Int, int> frontier, TileGrid grid, Dictionary<Vector3Int, PointData> track)
    {
        foreach (var next in grid.Get4Neighbours(current))
        {
            var newCost = currentCost + grid.GetCost(next);
            if (track.TryGetValue(next, out var data) && newCost >= data.CostSoFar)
                continue;

            var priority = newCost + Heuristic(start, next);
            frontier.Enqueue(next, priority);
            track[next] = new PointData(current, newCost);
        }
    }

    private static int Heuristic(Vector3Int pos1, Vector3Int pos2)
    {
        return Mathf.Abs(pos2.x - pos1.x) + Mathf.Abs(pos2.y - pos1.y);
    }


    private struct PointData
    {
        public Vector3Int? CameFrom { get; }
        public int CostSoFar { get; }

        public PointData(Vector3Int? cameFrom, int costSoFar)
        {
            CameFrom = cameFrom;
            CostSoFar = costSoFar;
        }
    }
}