using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CityManager : MonoBehaviour {
    [HideInInspector] public List<City> cities = new List<City>();
    public CityManageController cityManageController;
    private ResourceManager resourceManager;
    private BuildingManager buildingManager;
    private EntityManager entityManager;
    private YieldManager yieldManager;
    private TileMapManager tileMapManager;
    private GameManager gameManager;
    private CivilizationManager CM;
    private TileMap tileMap;
    public GameObject LineDrawerPrefab;
    private List<OwnersNames> OwnersNames = new List<OwnersNames>();

    public GameObject energyDeficitPrefab;
    private Dictionary<Vector2Int, GameObject> energyDeficits = new Dictionary<Vector2Int, GameObject>();

    private const int CITY_MIN_DIST = 5;

    private void Awake() {
        string jsonPath = Application.dataPath + "/Jsons/citynames.json";
        if (File.Exists(jsonPath))
        {
            string json = File.ReadAllText(jsonPath);
            OwnersNamesWrapper ownersNamesWrapper = JsonUtility.FromJson<OwnersNamesWrapper>(json);
            OwnersNames = ownersNamesWrapper.ownersNames;
        }
        else
        {
            Debug.LogError("JSON file not found: " + jsonPath);
        }

        GetComponent<CivilizationManager>().SummonCivs(AllCivNames());
    }

    List<string> AllCivNames() {
        List<string> civNames = new List<string>();
        foreach (OwnersNames ownerNames in OwnersNames) {
            civNames.Add(ownerNames.OwnerName);
        }
        return civNames;
    }

    private void Start() {
        buildingManager = GetComponent<BuildingManager>();
        entityManager = GetComponent<EntityManager>();
        yieldManager = GetComponent<YieldManager>();
        gameManager = GetComponent<GameManager>();
        resourceManager = GetComponent<ResourceManager>();
        tileMapManager = GetComponent<TileMapManager>();
        tileMap = GameObject.Find("MANAGER").GetComponent<GameManager>().tileMap;
        CM = GetComponent<CivilizationManager>();
    }

    public void SetEnergyDeficit(Vector2Int position, bool deficit) {
        if (deficit) {
            if (!energyDeficits.ContainsKey(position)) {
                energyDeficits[position] = Instantiate(energyDeficitPrefab, tileMap.GetWorldPosition(position), Quaternion.identity);
            }
        } else {
            if (energyDeficits.ContainsKey(position)) {
                Destroy(energyDeficits[position]);
                energyDeficits.Remove(position);
            }
        }
    }

    private bool IsBuildingOn(City city, Vector2Int position) {
        foreach (Building building in city.buildings) {
            if (building.Position == position) {
                return true;
            }
        }
        return false;
    }
    public Vector2Int? ChooseBuildTile(City city, Building building) {
        List<Vector2Int> buildableTiles = new List<Vector2Int>();
        List<Vector2Int> buildableTiles_unideals = new List<Vector2Int>();
        if (building.TerrainType == "" || building.TerrainType == null) {
            foreach (Tile tile in city.cityTiles) {
                if (tile.Position == city.Position) { continue; }
                if (!IsBuildingOn(city, tile.Position)) {
                    if (tileMapManager.IsResourceType(tile.TerrainType)) {
                        buildableTiles_unideals.Add(tile.Position);
                    } else {
                        buildableTiles.Add(tile.Position);
                    }
                }
            }
        } else {
            foreach (Tile tile in city.cityTiles) {
                if (tile.Position == city.Position) { continue; }
                if (tile.TerrainType == building.TerrainType && !IsBuildingOn(city, tile.Position)) {
                    buildableTiles.Add(tile.Position);
                }
            }
        }

        if ((building.TerrainType == "" || building.TerrainType == null) && buildableTiles.Count == 0 && buildableTiles_unideals.Count > 0) {
            return buildableTiles_unideals[Random.Range(0, buildableTiles_unideals.Count)];
        }
        if (buildableTiles.Count == 0) {
            Debug.Log("No buildable tiles for " + building.Name + " in " + city.Name);
            Notifier.Notify("No buildable tiles for " + building.Name + " in " + city.Name);
            return null;
        }

        return buildableTiles[Random.Range(0, buildableTiles.Count)];
    }

    string GenerateCityName(int ownerId) {
        string ownerName = CM.GetCiv(ownerId).Name;
        OwnersNames ownerNames = OwnersNames.Find(x => x.OwnerName == ownerName);
        if (ownerNames == null) {
            return "City " + (cities.Count + 1);
        } else {
            int copyNum = Mathf.FloorToInt(cities.Count / ownerNames.CityNames.Count);

            if (copyNum == 0) {
                return ownerNames.CityNames[cities.Count % ownerNames.CityNames.Count];
            } else {
                return ownerNames.CityNames[cities.Count % ownerNames.CityNames.Count] + " " + (copyNum + 1);
            }
        }
    }

    public City CityOnPosition(Vector2Int position) {
        foreach (City city in cities) {
            if (city.Position == position) {
                return city;
            }
        }
        return null;
    }

    public void AddCity(Vector2Int position, int ownerId) {
        Civilization civ = CM.GetCiv(ownerId);
        City city = new City();
        city.Name = GenerateCityName(ownerId);
        city.Position = position;
        city.buildings = new List<Building>();
        city.Owner = ownerId;
        city.Production = null;

        city.Yields = new CityYieldsHolder();

        Tile center = tileMap.GetTile(position);
        city.cityTiles = tileMap.GetNeighbours(center);
        city.cityTiles.Add(center);

        city.borderLine = Instantiate(LineDrawerPrefab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<LineScript>();
        city.borderLine.SetRenderer();

        cities.Add(city);
        civ.cityIdentity.AddCity(city);
        if (ownerId == -1) {Notifier.Notify(city.Name + " was founded");}
        civ.yieldIdentity.RecalculateYields(false);
        RedrawCityBorders();
        gameManager.UpdateGame();
    }

    public void SelectCity(City city) {
        cityManageController.SelectCity(city);
    }

    public string ParseLiteralCityOption(string cityOption) {
        string[] optionParts = cityOption.Split('-');
        string optionType = optionParts[0];
        string optionValue = optionParts[1];

        if (optionType == "B") {
            List<ResourceRequirement> resourceRequirements = buildingManager.GetBuilding(optionValue).resourceRequirements;
            if (resourceRequirements.Count > 0) {
                string resourceString = "[";
                foreach (ResourceRequirement resourceRequirement in resourceRequirements) {
                    resourceString += resourceRequirement.ResourceName + ":" + resourceRequirement.Amount + ",";
                }
                resourceString = resourceString.Remove(resourceString.Length - 1) + "]";
                return cityOption.Replace("B-", "Build the ").Replace("CU-", "Train a ").Replace("MU-", "Train a ") + resourceString;
            }
        }

        return cityOption.Replace("B-", "Build the ").Replace("CU-", "Train a ").Replace("MU-", "Train a ");
    }

    Sprite GrabIcon(string iconPath) {
        if (Resources.Load<Sprite>(iconPath) == null) {
            Debug.LogError("Icon not found at " + iconPath);
        }

        return Resources.Load<Sprite>(iconPath);
    }
    public Sprite CityOptionIcon(string cityOption) {
        string[] optionParts = cityOption.Split('-');
        string optionType = optionParts[0];
        string optionValue = optionParts[1];

        switch (optionType) {
            case "B":
                return GrabIcon(buildingManager.GetBuilding(optionValue).IconPath);
            case "CU":
                return GrabIcon(entityManager.GetCivil(optionValue).IconPath);
            case "MU":
                return GrabIcon(entityManager.GetMilit(optionValue).IconPath);
            default:
                Debug.LogError("Invalid city option type: " + optionType);
                return null;
        }
    }

    public List<string> CityOptions(City city, int ownerId) {
        List<string> cityOptions = new List<string>();

        foreach (Building building in buildingManager.PossibleBuildings(city, ownerId)) {
            cityOptions.Add("B-" + building.Name);
        }

        (List<Civil> civils, List<Milit> milits) = entityManager.PossibleUnits(city, ownerId);

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

    //might need to make available to other civs other than player
    public void CityOptionFunction(string option) {
        string[] optionParts = option.Split('-');
        string optionType = optionParts[0];
        string optionValue = optionParts[1];

        City city = cityManageController.selectedCity;
        city.ProductionProgress = 0;

        switch (optionType) {
            case "B":
                Building building = buildingManager.buildings.Find(x => x.Name == optionValue);
                Vector2Int? position = ChooseBuildTile(city, building); //add a function to let user choose/recieve a position within city borders to build the Building there & functionality for Buildings to stand on their own tiles & removing buildings (BUT NOT IF OTHER BUILDINGS REQUIRE THEIR EXISTENCE)
                if (position == null) {return;}
                if (building.resourceRequirements.Count > 0) {
                    if (!CM.Player.resourceIdentity.RemoveResources(building.resourceRequirements)) {
                        Debug.Log("Not enough resources to build " + building.Name);
                        Notifier.Notify("Not enough resources to build " + building.Name);
                        return;
                    }
                }
                building.Position = position.Value;
                city.Production = new BuildingProduction(building);
                tileMap.AddTileExtra(building.Position, "construction");
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
        gameManager.UpdateGame();
    }
    public void CityOptionFunction(int ownerId, City city, string option) { //non-player function
        string[] optionParts = option.Split('-');
        string optionType = optionParts[0];
        string optionValue = optionParts[1];

        city.ProductionProgress = 0;

        switch (optionType) {
            case "B":
                Building building = buildingManager.buildings.Find(x => x.Name == optionValue);
                Vector2Int? position = ChooseBuildTile(city, building); //add a function to let user choose/recieve a position within city borders to build the Building there & functionality for Buildings to stand on their own tiles & removing buildings (BUT NOT IF OTHER BUILDINGS REQUIRE THEIR EXISTENCE)
                if (position == null) {return;}
                if (building.resourceRequirements.Count > 0) {
                    if (!CM.GetCiv(ownerId).resourceIdentity.RemoveResources(building.resourceRequirements)) {
                        return;
                    }
                }
                building.Position = position.Value;
                city.Production = new BuildingProduction(building);
                tileMap.AddTileExtra(building.Position, "construction");
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

        gameManager.UpdateGame();
    }

    public void CityBroken(City city, Milit attacker) {
        city.Owner = attacker.Owner;
        city.Health = 1;
    }

    public bool CanDistBuildCity(Vector2Int position) {
        foreach (City city in cities) {
            if (Vector2Int.Distance(city.Position, position) < CITY_MIN_DIST) {
                return false;
            }
        }
        return true;
    }

    public void RedrawCityBorders() { //sucks right now
        foreach (City city in cities) {
            city.borderLine.DrawLine(tileMap.SurroundingPoints(city.cityTiles), Color.magenta);
        }
    }

    public void NextTurn() {
        cityManageController.Deselect();
        foreach (City city in cities) {
            if (city.Production != null) {
                city.ProductionProgress += city.Yields.ProductionPoints;
                if (city.ProductionProgress >= city.Production.Cost) {
                    city.Production.Complete(city);
                    city.Production = null;
                    city.ProductionProgress = 0;
                }
            }
            city.PopulationGrowth();
            city.PassiveHealing();
        }
    }

    public void AddCityYields(int ownerId) {
        if (CM == null) { CM = GetComponent<CivilizationManager>(); }
        foreach (City city in CM.GetCiv(ownerId).cityIdentity.cities) {
            city.AddSumYields(yieldManager);
        }
    }

    public string GetTileCityName(Tile tile) {
        City closestCity = null;
        float closestDist = 1000;
        foreach (City city in cities) {
            float dist = Vector2Int.Distance(city.Position, tile.Position);
            if (dist < closestDist) {
                closestCity = city;
                closestDist = dist;
            }
        }
        if (closestCity.cityTiles.Contains(tile)) {
            return closestCity.Name;
        }
        Debug.LogError("Tile checked on " + tile.Position.ToString() + " is not in any city");
        return null;
    }
}

[System.Serializable]
public class City {
    public string Name;
    public Vector2Int Position;
    public List<Building> buildings;
    public int Owner;
    public ICityProduction Production;
    public float ProductionProgress;
    public List<Tile> cityTiles;
    public LineScript borderLine;
    public float Health = 80.0f;
    //max health is CityYieldsHolder.Strength

    public CityYieldsHolder Yields;

    public void PassiveHealing() {
        if (Health == Yields.Strength) {return;}
        else {
            Health += Yields.Strength / 10;
            if (Health >= Yields.Strength) {
                Health = Yields.Strength;
            }
        }
    }

    public void TakeDamage(float dmg, Milit attacker) {
        Health -= dmg;
        if (Health <= 0) {
            GameObject.Find("MANAGER").GetComponent<CityManager>().CityBroken(this, attacker);
        }
    }

    public void YieldRecalculation() {
        Yields = new CityYieldsHolder();
        Yields.Strength = 80.0f;
        foreach (Building building in buildings) {
            if (building.Yields.Energy < 0) {
                if (Yields.Energy + building.Yields.Energy >= 0) {
                    building.ApplyBuildingEffects(this);
                    GameObject.Find("MANAGER").GetComponent<CityManager>().SetEnergyDeficit(building.Position, false);
                } else {
                    Yields.Energy -= building.Yields.Energy;
                    GameObject.Find("MANAGER").GetComponent<CityManager>().SetEnergyDeficit(building.Position, true);
                }
            }
        }
    }

    public void AddSumYields(YieldManager yieldManager) {
        Civilization civ = GameObject.Find("MANAGER").GetComponent<CivilizationManager>().GetCiv(Owner);
        YieldRecalculation();
        civ.yieldIdentity.sciencePoints += Yields.Science;
        civ.yieldIdentity.goldPoints += Yields.Gold;
    }

    public void PopulationGrowth() {
        Yields.Population = Mathf.Min(Yields.Housing, Yields.Population + (Yields.Food / (6 * Yields.Population + Mathf.Pow(Yields.Population, 1.5f))));
        BorderGrowth();
    }

    public void BorderGrowth() {
        if (cityTiles.Count >= 19) { return; }

        int borderGrowth = ((int)Mathf.Floor(Yields.Population) + 6) - cityTiles.Count;
        if (borderGrowth > 0) {
            //add one tile to the city (if possible) (tile cannot exceed 2 tiles away from the city center)
            //currently made to only expand to 2 tiles away (manually calculated directions for 2 away from center)
            //there might be other conditions to stop tile from being added, i.e. part of another city
            TileMap tileMap = GameObject.Find("MANAGER").GetComponent<GameManager>().tileMap;
            foreach (Tile tile in tileMap.GetSecondNeighbours(tileMap.GetTile(Position))) {
                if (!cityTiles.Contains(tile)) {
                    cityTiles.Add(tile);
                    GameObject.Find("MANAGER").GetComponent<CityManager>().RedrawCityBorders();
                    break;
                }
            }
        }
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
    public Vector2Int Position;

    public void Complete(City city) {
        Debug.Log("Building " + Name + " completed in " + city.Name + " at " + Position.ToString());
        Notifier.Notify("Building " + Name + " completed in " + city.Name);
        city.buildings.Add((Building)Production);
        city.YieldRecalculation();
        GameObject.Find("MANAGER").GetComponent<GameManager>().tileMap.AddTileExtra(Position, ((Building)Production).ExtraType, true);
    }

    public BuildingProduction(Building building) {
        Name = building.Name;
        Cost = building.Cost;
        Production = building;
        Position = building.Position;
    }
}

public class CivilProduction : ICityProduction {
    public string Name { get; }
    public int Cost { get; }
    public object Production { get; set; }

    public void Complete(City city) {
        Debug.Log("Civil " + Name + " completed in " + city.Name);
        Notifier.Notify(Name + " completed in " + city.Name);
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
        Notifier.Notify(Name + " completed in " + city.Name);
        GameObject.Find("MANAGER").GetComponent<EntityManager>().CitySpawn(city, (Milit)Production);
    }

    public MilitProduction(Milit milit) {
        Name = milit.Name;
        Cost = milit.Cost;
        Production = milit;
    }
}

public class CityYieldsHolder : YieldsHolder {
    public float Population = 0;

    public CityYieldsHolder(float Population = 1, int Housing = 2, float Food = 1, float ProductionPoints = 1, float Science = 1, float Gold = 1, float Energy = 0, float Strength = 80) : base(Housing, Food, ProductionPoints, Science, Gold, Energy, Strength) {
        this.Population = Population;
    }
}

[System.Serializable]
public class OwnersNamesWrapper {
    public List<OwnersNames> ownersNames;
}
[System.Serializable]
public class OwnersNames {
    public string OwnerName;
    public List<string> CityNames;

    public OwnersNames(string OwnerName, List<string> CityNames) {
        this.OwnerName = OwnerName;
        this.CityNames = CityNames;
    }
}