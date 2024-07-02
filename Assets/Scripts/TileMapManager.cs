using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapManager : MonoBehaviour
{
    private GameManager gameManager;
    private EntityManager entityManager;

    public Tilemap tilemap;
    public Tilemap extrasTilemap;
    
    [Header("Terrain Tiles")]
    public TileBase basicMarsTile;

    [Header("Building Tiles")]
    public TileBase settlementBuilding;

    private TileMap tileMap;

    void Start()
    {
        gameManager = GetComponent<GameManager>();
        entityManager = GetComponent<EntityManager>();
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
            if (tile.HasExtra)
            {
                extrasTilemap.SetTile(new Vector3Int(tile.Position.x, tile.Position.y, 0), GetTileExtra(tile.ExtraType));
            }
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
    TileBase GetTileExtra(string extraType)
    {
        switch (extraType)
        {
            case "Settlement":
                return settlementBuilding;
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
