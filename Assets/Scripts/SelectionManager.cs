using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour {
    //current selection choices - city, unit(civil/milit)
    CityManager cityManager;
    ActionManager actionManager;
    TileMap tileMap;
    Tile currentTile;
    public GameObject cityButton;
    public CityManageController cityManageController;

    void Start()
    {
        cityManager = GetComponent<CityManager>();
        actionManager = GetComponent<ActionManager>();
        tileMap = GetComponent<GameManager>().tileMap;
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0) && !actionManager.isMoving) {
            City city = cityManager.CityOnPosition(tileMap.GetGridPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
            if (city != null) {
                cityManageController.SelectCity(city);
            }
        }
    }

    public void EntitySelected(object entity) {
        cityManageController.Deselect();
        if (entity is Civil)
            currentTile = tileMap.Tiles[((Civil)entity).Position];
        else
            currentTile = tileMap.Tiles[((Milit)entity).Position];
        
        if (cityManager.CityOnPosition(currentTile.Position) != null) 
            cityButton.SetActive(true);
        else
            cityButton.SetActive(false);
    }

    public void EntityDeselected() {
        cityButton.SetActive(false);
    }

    public void CityButtonClicked() {
        actionManager.Deselection();
        cityManager.SelectCity(cityManager.CityOnPosition(currentTile.Position));
        cityButton.SetActive(false);
    }
}