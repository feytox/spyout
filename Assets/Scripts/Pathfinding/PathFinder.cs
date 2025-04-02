using System.Collections.Generic;
using Priority_Queue;
using UnityEngine;

public static class PathFinder
{
    // https://www.redblobgames.com/pathfinding/a-star/introduction.html
    public static IEnumerable<Vector3Int> FindAStarPath(TileGrid grid, Vector3Int start, Vector3Int end)
    {
        var frontier = new FastPriorityQueue<Location>(grid.MaxTileCount);
        frontier.Enqueue(new Location(end), 0); // no need to reverse if we start at the end
        var cameFrom = new Dictionary<Vector3Int, Vector3Int?> { { end, null } };
        var costSoFar = new Dictionary<Vector3Int, int> { { end, 0 } };

        while (frontier.Count != 0)
        {
            var current = frontier.Dequeue().Pos;
            var currentCost = costSoFar[current];

            if (current == start)
                break;

            foreach (var next in grid.Get4Neighbours(current))
            {
                var newCost = currentCost + grid.GetCost(next);
                if (costSoFar.TryGetValue(next, out var cost) && newCost >= cost)
                    continue;

                var priority = newCost + Heuristic(start, next);
                frontier.Enqueue(new Location(next), priority);
                cameFrom[next] = current;
                costSoFar[next] = newCost;
            }
        }

        Vector3Int? pos = start;
        while (pos is not null)
        {
            yield return pos.Value;
            pos = cameFrom[pos.Value];
        }
    }

    private static int Heuristic(Vector3Int pos1, Vector3Int pos2)
    {
        return Mathf.Abs(pos2.x - pos1.x) + Mathf.Abs(pos2.y - pos2.y);
    }

    private class Location : FastPriorityQueueNode
    {
        public Vector3Int Pos { get; }

        public Location(Vector3Int pos) => Pos = pos;
    }
}