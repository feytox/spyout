using System.Collections.Generic;
using Priority_Queue;
using UnityEngine;

public static class PathFinder
{
    // https://www.redblobgames.com/pathfinding/a-star/introduction.html
    public static IEnumerable<Vector3Int> FindAStarPath(TileGrid grid, Vector3Int start, Vector3Int end)
    {
        var frontier = new FastPriorityQueue<Location>(grid.MaxTileCount);
        frontier.Enqueue(new Location(start), 0);
        var cameFrom = new Dictionary<Vector3Int, Vector3Int>();
        var costSoFar = new Dictionary<Vector3Int, int> { {start, 0}};

        while (frontier.Count != 0)
        {
            var current = frontier.Dequeue().Pos;
            var currentCost = costSoFar[current];
            
            if (current == end)
                break;
            
            foreach (var next in grid.Get4Neighbours(current))
            {
                var newCost = currentCost + grid.GetCost(next);
                if (costSoFar.TryGetValue(next, out var cost) && newCost >= cost)
                    continue;

                var priority = newCost + Heuristic(end, next);
                frontier.Enqueue(new Location(next), priority);
                cameFrom[next] = current;
                costSoFar[next] = newCost;
            }
        }

        var pos = end;
        while (pos != start)
        {
            yield return pos;
            pos = cameFrom[pos];
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