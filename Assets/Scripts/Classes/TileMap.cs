using System.Collections.Generic;
using UnityEngine;

public class TileMap
{
    public Dictionary<Vector2Int, Tile> Tiles { get; private set; }
    private float gridCellX = 2.39f;
    private float gridCellY = 2.77f;

    public TileMap(int width, int height)
    {
        Tiles = new Dictionary<Vector2Int, Tile>();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var position = new Vector2Int(x, y);
                Tiles[position] = new Tile(position, "basicmars"); // Initialize with default terrain type
            }
        }
    }

    public Tile GetTile(Vector2Int position)
    {
        Tiles.TryGetValue(position, out var tile);
        return tile;
    }

    public void SetTile(Vector2Int position, Tile tile)
    {
        Tiles[position] = tile;
    }

    public Vector2 GetWorldPosition(Vector2Int position)
    {
        if (position.x % 2 == 0)
            return new Vector2(position.x * gridCellX, position.y * gridCellY);
        else
            return new Vector2(0.5f * gridCellX + position.x * gridCellX, position.y * gridCellY);
    }
}
