using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapManager : MonoBehaviour
{
    public GameManager gameManager;
    public EntityManager entityManager;

    public Tilemap tilemap;
    public TileBase basicMarsTile;

    private TileMap tileMap;

    public void Initialize(TileMap tileMap)
    {
        this.tileMap = tileMap;
        RenderTiles();
    }

    public void RenderTiles()
    {
        foreach (var tile in tileMap.Tiles.Values)
        {
            TileBase tileBase = GetTileBase(tile);
            tilemap.SetTile(new Vector3Int(tile.Position.x, tile.Position.y, 0), tileBase);
        }
    }

    TileBase GetTileBase(Tile tile)
    {
        switch (tile.TerrainType)
        {
            case "basicmars":
                return basicMarsTile;
            default:
                return null;
        }
    }

    public void AddBuilding(Vector2Int position, string buildingType)
    {
        var tile = tileMap.GetTile(position);
        if (tile != null && !tile.HasBuilding)
        {
            tile.HasBuilding = true;
            tile.BuildingType = buildingType;
        }
    }

    public void ChangeTerrain(Vector2Int position, string newTerrainType)
    {
        var tile = tileMap.GetTile(position);
        if (tile != null)
        {
            tile.TerrainType = newTerrainType;
        }
    }

    public void HarvestResource(Vector2Int position)
    {
        var tile = tileMap.GetTile(position);
        if (tile != null && tile.HasResource)
        {
            tile.HasResource = false;
            tile.ResourceType = null;
        }
    }
}
