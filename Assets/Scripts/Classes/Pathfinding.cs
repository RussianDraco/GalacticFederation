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
        return HexDistance(a, b);
    }

    private static float Distance(Vector2Int a, Vector2Int b)
    {
        return HexDistance(a, b);
    }

    private static float HexDistance(Vector2Int a, Vector2Int b)
    {
        // Convert axial to cube coordinates
        var ac = AxialToCube(a);
        var bc = AxialToCube(b);

        // Calculate the distance in cube coordinates
        return (Mathf.Abs(ac.x - bc.x) + Mathf.Abs(ac.y - bc.y) + Mathf.Abs(ac.z - bc.z)) / 2;
    }

    private static Vector3Int AxialToCube(Vector2Int hex)
    {
        var x = hex.x;
        var z = hex.y;
        var y = -x - z;
        return new Vector3Int(x, y, z);
    }

    private static IEnumerable<Vector2Int> GetNeighbors(Vector2Int current)
    {
        List<Vector2Int> directions;
        if (current.y % 2 == 0) {
            directions = new List<Vector2Int>
            {
                new Vector2Int(-1, 0),
                new Vector2Int(-1, 1),
                new Vector2Int(0, 1),
                new Vector2Int(1, 0),
                new Vector2Int(-1, -1),
                new Vector2Int(0, -1),
            };
        }
        else {
            directions = new List<Vector2Int>
            {
                new Vector2Int(-1, 0),
                new Vector2Int(0, 1),
                new Vector2Int(1, 1),
                new Vector2Int(1, 0),
                new Vector2Int(1, -1),
                new Vector2Int(0, -1),
            };
        }

        foreach (var direction in directions)
        {
            yield return current + direction;
        }
    }
}
