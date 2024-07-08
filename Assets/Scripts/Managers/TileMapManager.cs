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
    public TileBase siliconMarsTile;

    [Header("Building Tiles")]
    public TileBase construction;
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

    public bool ResourceHere(Vector2Int position, int ownerId, bool overrideToTrue = false) {
        Tile tile = tileMap.GetTile(position);
        if (IsResourceType(tile.TerrainType)) {
            if (overrideToTrue)
                return true;
            else
                return resourceManager.CanMakeTerrain(tile.TerrainType, ownerId);
        }
        return false;
    }
    public bool IsResourceType(string terrainType) {
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
            if (!resourceManager.CanMakeTerrain(tile.TerrainType, -1)) {
                return basicMarsTile;
            }
        }

        switch (tile.TerrainType)
        {
            case "basicmars":
                return basicMarsTile;
            case "ironmars":
                return ironMarsTile;
            case "siliconmars":
                return siliconMarsTile;
            default:
                return null;
        }
    }
    TileBase GetTileExtra(string extraType)
    {
        TileBase grabbedTile = Resources.Load<TileBase>("ExtraTiles/" + extraType);
        if (grabbedTile != null) {
            return grabbedTile;
        }

        switch (extraType.ToLower())
        {
            case "construction":
                return construction;
            case "city":
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
