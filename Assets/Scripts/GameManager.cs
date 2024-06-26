using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int mapWidth = 10;
    public int mapHeight = 10;
    private static GameManager instance;
    private TileMap tileMap;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    GameObject singleton = new GameObject();
                    instance = singleton.AddComponent<GameManager>();
                    singleton.name = "GameManager";
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

    private void Start()
    {
        tileMap = new TileMap(mapWidth, mapHeight);
        TileMapManager.Instance.Initialize(tileMap);
    }

    private void Update()
    {
        UpdateGame();
    }

    private void UpdateGame()
    {
        TileMapManager.Instance.RenderTiles();
    }
}