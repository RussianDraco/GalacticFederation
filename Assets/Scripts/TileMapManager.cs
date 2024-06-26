using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapManager : MonoBehaviour
{
    private static TileMapManager instance;

    public Tilemap tilemap;
    public TileBase basicMarsTile;

    private TileMap tileMap;

    public static TileMapManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TileMapManager>();
                if (instance == null)
                {
                    GameObject singleton = new GameObject();
                    instance = singleton.AddComponent<TileMapManager>();
                    singleton.name = "TileMapManager";
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

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
