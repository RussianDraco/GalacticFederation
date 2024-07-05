using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
        if (position.y % 2 == 0)
            return new Vector2(position.x * gridCellX, position.y * 0.75f * gridCellY);
        else
            return new Vector2(position.x * gridCellX + gridCellX / 2, position.y * 0.75f * gridCellY);
    }

    public Vector2Int GetGridPosition(Vector2 worldPosition)
    {
        var y = Mathf.FloorToInt((worldPosition.y + gridCellY / 2) / gridCellY / 0.75f);
        var x = Mathf.FloorToInt((worldPosition.x + gridCellX / 2 - (y % 2 == 0 ? 0 : gridCellX / 2)) / gridCellX);

        return new Vector2Int(x, y);
    }

    public bool AddTileExtra(Vector2Int position, string extraType, bool overwrite = false) //null for remove
    {
        // if extraType is "City"; add extra city build check (dist from other cities)
        var tile = GetTile(position);

        if (tile.HasExtra && !overwrite)
            return false;

        if (extraType == "City") {
            if (!GameObject.Find("MANAGER").GetComponent<CityManager>().CanDistBuildCity(position))
                return false;
        }

        if (extraType == null)
            tile.HasExtra = false;
        else
            tile.HasExtra = true;
        tile.ExtraType = extraType;
        SetTile(position, tile);
        return true;
    }

    public Tile CheckTile(Vector2Int position) {
        if (Tiles.ContainsKey(position)) {
            return Tiles[position];
        } else {
            return null;
        }
    }

    public List<Tile> GetNeighbours(Tile tile) { //need to integrate consideration of location as neighbours arent always the same relative positions away
        List<Tile> neighbours = new List<Tile>();
        Vector2Int[] directions = new Vector2Int[] {
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(0, -1),
            new Vector2Int(1, -1),
            new Vector2Int(-1, 1)
        };

        foreach (Vector2Int direction in directions) {
            Vector2Int neighbourPosition = tile.Position + direction;
            Tile neighbour = CheckTile(neighbourPosition);
            if (neighbour != null) {
                neighbours.Add(neighbour);
            }
        }

        return neighbours;
    }

    public List<Vector2> SurroundingPoints(List<Tile> tiles) {
        Vector2[] hexagonPoints = new Vector2[] {
            new Vector2(-gridCellX / 2, gridCellY * 0.25f),    // Bottom-left
            new Vector2(0, gridCellY * 0.5f),                  // Top
            new Vector2(gridCellX / 2, gridCellY * 0.25f),     // Bottom-right
            new Vector2(gridCellX / 2, -gridCellY * 0.25f),    // Top-right
            new Vector2(0, -gridCellY * 0.5f),                 // Bottom
            new Vector2(-gridCellX / 2, -gridCellY * 0.25f)    // Top-left
        };

        List<Vector2> positions = new List<Vector2>();
        foreach (Tile tile in tiles) {
            foreach (Vector2 hexagonPoint in hexagonPoints) {
                Vector2 position = GetWorldPosition(tile.Position) + hexagonPoint;
                positions.Add(position);
            }
        }
        return positions;
    }
}
