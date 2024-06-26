using UnityEngine;

[System.Serializable]
public class Tile {
    public Vector2Int Position { get; set; }
    public string TerrainType { get; set; }
    public bool HasResource { get; set; }
    public string ResourceType { get; set; }
    public bool HasBuilding { get; set; }
    public string BuildingType { get; set; }
    
    public Tile(Vector2Int position, string terrainType)
    {
        Position = position;
        TerrainType = terrainType;
        HasResource = false;
        HasBuilding = false;
    }
}