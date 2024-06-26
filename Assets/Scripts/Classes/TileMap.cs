using System.Collections.Generic;
using UnityEngine;

public class TileMap
{
    public Dictionary<Vector2Int, Tile> Tiles { get; private set; }

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
}
