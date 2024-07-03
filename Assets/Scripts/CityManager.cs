using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityManager : MonoBehaviour {
    [HideInInspector] public List<City> cities = new List<City>();
    public CityManageController cityManageController;

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

    public List<object> CityOptions() {
        //calculate city options on selected city
        return null;
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