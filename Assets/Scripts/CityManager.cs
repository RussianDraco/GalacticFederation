using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityManager : MonoBehaviour {
    [HideInInspector] public List<City> cities = new List<City>();
    public CityManageController cityManageController;
    private BuildingManager buildingManager;
    private EntityManager entityManager;

    private void Start() {
        buildingManager = GetComponent<BuildingManager>();
        entityManager = GetComponent<EntityManager>();
    }

    string GenerateCityName() {
        return "City " + (cities.Count + 1);
    }

    public City CityOnPosition(Vector2Int position) {
        foreach (City city in cities) {
            if (city.Position == position) {
                return city;
            }
        }
        return null;
    }

    public void AddCity(Vector2Int position) {
        City city = new City();
        city.Name = GenerateCityName();
        city.Position = position;
        city.buildings = new List<Building>();
        city.Owner = "Player";
        city.CurrentProduction = "";
        cities.Add(city);
    }

    public void SelectCity(City city) {
        cityManageController.SelectCity(city);
    }

    public string ParseLiteralCityOption(string cityOption) {
        
    }

    public List<string> CityOptions() {
        

        return new List<string>();
    }

    //City options operate and are stored as strings for the functions; the function parses and executes the respective functionality
    /*
    Make Building - "B|{BuildingName}"
    Make Unit - "U|{UnitName}"
    */
    public void CityOptionFunction(string option) {
        string[] optionParts = option.Split('|');
        string optionType = optionParts[0];
        string optionValue = optionParts[1];

        if (optionType == "B") {
            //Make Building
        } else if (optionType == "U") {
            //Make Unit
        }
    }
}

[System.Serializable]
public class City {
    public string Name;
    public Vector2Int Position;
    public List<Building> buildings;
    public string Owner;
    public string CurrentProduction;
}