using System.Collections.Generic;
using UnityEngine;

public static class Pathfinding
{
    public static List<Vector2Int> FindPath(TileMap tileMap, Vector2Int start, Vector2Int goal)
    {
        // Implement A* or another pathfinding algorithm here
        // Return a list of positions representing the path from start to goal
        return new List<Vector2Int> { start, goal };
    }
}
