using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TileMapManager : MonoBehaviour
{
    private GameManager gameManager;
    private EntityManager entityManager;
    private ResourceManager resourceManager;

    private Dictionary<string, bool> resourceTerrainTypes = new Dictionary<string, bool>();

    public Tilemap tilemap;
    public Tilemap extrasTilemap;
    
    [Header("Terrain Tiles")]
    public TileBase basicMarsTile;
    public TileBase ironMarsTile;

    [Header("Building Tiles")]
    public TileBase cityBuilding;

    private TileMap tileMap;

    void Awake()
    {
        resourceManager = GetComponent<ResourceManager>();
    }

    void Start()
    {
        gameManager = GetComponent<GameManager>();
        entityManager = GetComponent<EntityManager>();
    }

    public bool ResourceHere(Vector2Int position, bool overrideToTrue = false) {
        Tile tile = tileMap.GetTile(position);
        if (IsResourceType(tile.TerrainType)) {
            if (overrideToTrue)
                return true;
            else
                return resourceManager.CanMakeTerrain(tile.TerrainType);
        }
        return false;
    }
    private bool IsResourceType(string terrainType) {
        try {
            return resourceTerrainTypes[terrainType];
        } catch {
            return false;
        }
    }
    public void Initialize(TileMap tileMap)
    {
        this.tileMap = tileMap;

        foreach (Resource resource in resourceManager.resources) {
            resourceTerrainTypes[resource.TerrainType] = true;
        }

        RenderTiles();
    }

    public void RenderTiles()
    {
        foreach (var tile in tileMap.Tiles.Values)
        {
            TileBase tileBase = GetTileBase(tile);
            tilemap.SetTile(new Vector3Int(tile.Position.x, tile.Position.y, 0), tileBase);
            if (tile.HasExtra)
            {
                extrasTilemap.SetTile(new Vector3Int(tile.Position.x, tile.Position.y, 0), GetTileExtra(tile.ExtraType));
            }
        }
    }

    TileBase GetTileBase(Tile tile)
    {
        if (IsResourceType(tile.TerrainType)) {
            if (!resourceManager.CanMakeTerrain(tile.TerrainType)) {
                return basicMarsTile;
            }
        }

        switch (tile.TerrainType)
        {
            case "basicmars":
                return basicMarsTile;
            case "ironmars":
                return ironMarsTile;
            default:
                return null;
        }
    }
    TileBase GetTileExtra(string extraType)
    {
        switch (extraType)
        {
            case "City":
                return cityBuilding;
            default:
                return null;
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
}
