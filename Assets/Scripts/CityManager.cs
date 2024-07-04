using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityManager : MonoBehaviour {
    [HideInInspector] public List<City> cities = new List<City>();
    public CityManageController cityManageController;
    private BuildingManager buildingManager;
    private EntityManager entityManager;
    private YieldManager yieldManager;

    private void Start() {
        buildingManager = GetComponent<BuildingManager>();
        entityManager = GetComponent<EntityManager>();
        yieldManager = GetComponent<YieldManager>();
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
        city.Production = null;

        city.Population = 1;
        city.Housing = 1;
        city.Food = 1;
        city.ProductionPoints = 1;
        city.Science = 1;
        city.Gold = 1;


        cities.Add(city);
    }

    public void SelectCity(City city) {
        cityManageController.SelectCity(city);
    }

    public string ParseLiteralCityOption(string cityOption) {
        return cityOption.Replace("B-", "Build the ").Replace("CU-", "Train a ").Replace("MU-", "Train a ");
    }

    public List<string> CityOptions(City city) {
        List<string> cityOptions = new List<string>();

        foreach (Building building in buildingManager.PossibleBuildings(city)) {
            cityOptions.Add("B-" + building.Name);
        }

        (List<Civil> civils, List<Milit> milits) = entityManager.PossibleUnits(city);

        foreach (Civil civil in civils) {
            cityOptions.Add("CU-" + civil.Name);
        }
        foreach (Milit milit in milits) {
            cityOptions.Add("MU-" + milit.Name);
        }

        return cityOptions;
    }

    //City options operate and are stored as strings for the functions; the function parses and executes the respective functionality
    /*
    Make Building - "B-{BuildingName}"
    Make Civil Unit - "CU-{UnitName}"
    Make Milit Unit - "MU-{UnitName}"
    */
    public void CityOptionFunction(string option) {
        string[] optionParts = option.Split('-');
        string optionType = optionParts[0];
        string optionValue = optionParts[1];

        City city = cityManageController.selectedCity;
        city.ProductionProgress = 0;

        switch (optionType) {
            case "B":
                Building building = buildingManager.buildings.Find(x => x.Name == optionValue);
                city.Production = new BuildingProduction(building);
                break;
            case "CU":
                city.Production = new CivilProduction(entityManager.GetCivil(optionValue));
                break;
            case "MU":
                city.Production = new MilitProduction(entityManager.GetMilit(optionValue));
                break;
            default:
                Debug.LogError("Invalid city option type: " + optionType);
                break;
        }

        cityManageController.Deselect();
    }

    public void NextTurn() {
        cityManageController.Deselect();
        foreach (City city in cities) {
            if (city.Production != null) {
                city.ProductionProgress += 1; //production quantity will have to be calculated later
                if (city.ProductionProgress >= city.Production.Cost) {
                    city.Production.Complete(city);
                    city.Production = null;
                    city.ProductionProgress = 0;
                }
            }
        }
    }

    public void AddCityYields() {
        foreach (City city in cities) {
            city.AddYields(yieldManager);
        }
    }
}

[System.Serializable]
public class City {
    public string Name;
    public Vector2Int Position;
    public List<Building> buildings;
    public string Owner;
    public ICityProduction Production;
    public int ProductionProgress;

    public int Population;
    public int Housing;
    public int Food;
    public int ProductionPoints;
    public int Science;
    public int Gold;

    public void AddYields(YieldManager yieldManager) {
        yieldManager.sciencePoints += Science;
        yieldManager.goldPoints += Gold;
    }
}

public interface ICityProduction {
    string Name { get; }
    int Cost { get; }
    object Production { get; set; }
    void Complete(City city);
}

public class BuildingProduction : ICityProduction {
    public string Name { get; }
    public int Cost { get; }
    public object Production { get; set; }

    public void Complete(City city) {
        Debug.Log("Building " + Name + " completed in " + city.Name);
        city.buildings.Add((Building)Production);
    }

    public BuildingProduction(Building building) {
        Name = building.Name;
        Cost = building.Cost;
        Production = building;
    }
}

public class CivilProduction : ICityProduction {
    public string Name { get; }
    public int Cost { get; }
    public object Production { get; set; }

    public void Complete(City city) {
        Debug.Log("Civil " + Name + " completed in " + city.Name);
        GameObject.Find("MANAGER").GetComponent<EntityManager>().CitySpawn(city, (Civil)Production);
    }

    public CivilProduction(Civil civil) {
        Name = civil.Name;
        Cost = civil.Cost;
        Production = civil;
    }
}
public class MilitProduction : ICityProduction {
    public string Name { get; }
    public int Cost { get; }
    public object Production { get; set; }

    public void Complete(City city) {
        Debug.Log("Milit " + Name + " completed in " + city.Name);
        GameObject.Find("MANAGER").GetComponent<EntityManager>().CitySpawn(city, (Milit)Production);
    }

    public MilitProduction(Milit milit) {
        Name = milit.Name;
        Cost = milit.Cost;
        Production = milit;
    }
}