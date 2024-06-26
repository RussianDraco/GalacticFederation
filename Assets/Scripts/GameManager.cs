using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int mapWidth = 10;
    public int mapHeight = 10;
    public TileMap tileMap;
    public TileMapManager tileMapManager;
    public EntityManager entityManager;

    void Awake()
    {
        tileMap = new TileMap(mapWidth, mapHeight);   
    }

    private void Start()
    {
        tileMapManager.Initialize(tileMap);
        Debug.Log(entityManager.civils[0].Name); //need to fix civil importing so that they get their entity attributes
    }

    private void Update()
    {
        UpdateGame();
    }

    private void UpdateGame()
    {
        tileMapManager.RenderTiles();
        entityManager.UpdateEntities();
    }
}