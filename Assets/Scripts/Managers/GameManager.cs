using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int mapWidth = 10;
    public int mapHeight = 10;
    public TileMap tileMap;
    private TileMapManager tileMapManager;
    private EntityManager entityManager;
    private ActionManager actionManager;
    private ScienceManager scienceManager;
    private CityManager cityManager;
    private YieldManager yieldManager;
    private CivilizationManager civilizationManager;
    int turnNumber = 0;

    void Awake()
    {
        tileMap = new TileMap(mapWidth, mapHeight);   
    }

    private void Start()
    {
        tileMapManager = GetComponent<TileMapManager>();
        entityManager = GetComponent<EntityManager>();
        actionManager = GetComponent<ActionManager>();
        scienceManager = GetComponent<ScienceManager>();
        cityManager = GetComponent<CityManager>();
        yieldManager = GetComponent<YieldManager>();
        civilizationManager = GetComponent<CivilizationManager>();

        tileMapManager.Initialize(tileMap);
        entityManager.SpawnCivil(entityManager.civils[0], new Vector2Int(1, 0));
        entityManager.SpawnCivil(entityManager.civils[0], new Vector2Int(1, 1));
        entityManager.SpawnMilit(entityManager.milits[0], new Vector2Int(0, 0));
    }

    public void UpdateGame()
    {
        tileMapManager.RenderTiles();
        entityManager.UpdateEntities();
    }

    public void NextTurn()
    {
        turnNumber++;
        Debug.Log("Turn " + turnNumber);
        civilizationManager.NextTurn();
        yieldManager.NextTurn();
        actionManager.NextTurn();
        entityManager.NextTurn();
        cityManager.NextTurn();
        UpdateGame();
    }
}