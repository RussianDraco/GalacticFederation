using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int mapWidth = 50;
    public int mapHeight = 50;
    public TileMap tileMap;
    private TileMapManager tileMapManager;
    private EntityManager entityManager;
    private ActionManager actionManager;
    private ScienceManager scienceManager;
    private CityManager cityManager;
    private YieldManager yieldManager;
    private CivilizationManager civilizationManager;
    private AiManager aiManager;
    int turnNumber = 0;
    public GameObject playerWonScreen;
    public GameObject gameOverScreen;

    void Awake()
    {
        tileMap = new TileMap(mapWidth, mapHeight); 
        playerWonScreen.SetActive(false);
        gameOverScreen.SetActive(false);  
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
        aiManager = GetComponent<AiManager>();

        if (civilizationManager.Player.scienceIdentity == null) {civilizationManager.Player.Init(this.gameObject);}

        tileMapManager.Initialize(tileMap);

        entityManager.SpawnCivil(entityManager.civils[0], new Vector2Int(1, 0), -1);
        entityManager.SpawnMilit(entityManager.milits[0], new Vector2Int(0, 0), -1);
        
        entityManager.SpawnCivil(entityManager.civils[0], new Vector2Int(21, 20), 0);
        entityManager.SpawnMilit(entityManager.milits[0], new Vector2Int(20, 20), 0);

        entityManager.SpawnCivil(entityManager.civils[0], new Vector2Int(41, 40), 1);
        entityManager.SpawnMilit(entityManager.milits[0], new Vector2Int(40, 40), 1);

        entityManager.SpawnCivil(entityManager.civils[0], new Vector2Int(21, 0), 2);
        entityManager.SpawnMilit(entityManager.milits[0], new Vector2Int(20, 0), 2);

        entityManager.SpawnCivil(entityManager.civils[0], new Vector2Int(0, 21), 3);
        entityManager.SpawnMilit(entityManager.milits[0], new Vector2Int(0, 20), 3);

        entityManager.SpawnCivil(entityManager.civils[0], new Vector2Int(41, 0), 4);
        entityManager.SpawnMilit(entityManager.milits[0], new Vector2Int(40, 0), 4);

        entityManager.SpawnCivil(entityManager.civils[0], new Vector2Int(0, 41), 5);
        entityManager.SpawnMilit(entityManager.milits[0], new Vector2Int(0, 40), 5);
    }

    void RevertToMenu() {
        SceneManager.LoadScene("Menu");
    }
    private IEnumerator SleepCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
    }

    public void PlayerWon() {
        playerWonScreen.SetActive(true);
        StartCoroutine(SleepCoroutine(3f));
        RevertToMenu();
    }

    public void GameOver() {
        gameOverScreen.SetActive(true);
        StartCoroutine(SleepCoroutine(3f));
        RevertToMenu();
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
        if (turnNumber % 10 == 0) {
            Notifier.Notify("Turn: " + turnNumber);
        }
        civilizationManager.NextTurn();
        actionManager.NextTurn();
        entityManager.NextTurn();
        cityManager.NextTurn();
        aiManager.NextTurn();
        UpdateGame();
    }
}