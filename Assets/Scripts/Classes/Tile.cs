using UnityEngine;

[System.Serializable]
public class Tile {
    public Vector2Int Position { get; set; }
    public string TerrainType { get; set; }
    public bool HasExtra { get; set; }
    public string ExtraType { get; set; } //resource, building, etc.
    
    public Tile(Vector2Int position, string terrainType)
    {
        Position = position;
        TerrainType = terrainType;
        HasExtra = false;
    }
}