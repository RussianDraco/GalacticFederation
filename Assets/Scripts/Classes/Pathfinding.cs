using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Pathfinding
{
    public static List<Vector2Int> FindPath(TileMap tileMap, Vector2Int start, Vector2Int goal)
    {
        var openSet = new HashSet<Vector2Int> { start };
        var cameFrom = new Dictionary<Vector2Int, Vector2Int>();

        var gScore = new Dictionary<Vector2Int, float>();
        var fScore = new Dictionary<Vector2Int, float>();

        foreach (var tile in tileMap.Tiles)
        {
            gScore[tile.Key] = float.MaxValue;
            fScore[tile.Key] = float.MaxValue;
        }

        gScore[start] = 0;
        fScore[start] = Heuristic(start, goal);

        while (openSet.Count > 0)
        {
            var current = openSet.OrderBy(node => fScore[node]).First();

            if (current == goal)
                return ReconstructPath(cameFrom, current);

            openSet.Remove(current);

            foreach (var neighbor in GetNeighbors(current))
            {
                if (!tileMap.Tiles.ContainsKey(neighbor))
                    continue;

                var tentativeGScore = gScore[current] + Distance(current, neighbor);

                if (tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + Heuristic(neighbor, goal);

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        return new List<Vector2Int>();
    }

    private static List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
    {
        var totalPath = new List<Vector2Int> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            totalPath.Insert(0, current);
        }
        return totalPath;
    }

    private static float Heuristic(Vector2Int a, Vector2Int b)
    {
        // Manhattan distance for hexagonal grids
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    private static float Distance(Vector2Int a, Vector2Int b)
    {
        return Vector2Int.Distance(a, b);
    }

    private static IEnumerable<Vector2Int> GetNeighbors(Vector2Int current)
    {
        // Define the six possible directions in a hexagonal grid
        var directions = new List<Vector2Int>
        {
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(0, -1),
            new Vector2Int(1, -1),
            new Vector2Int(-1, 1)
        };

        foreach (var direction in directions)
        {
            yield return current + direction;
        }
    }
}
