using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//probably need to add a secondary building system (like civ6 districts) or something

public class BuildingManager : MonoBehaviour {
    [HideInInspector] public List<Building> buildings = new List<Building>();
    private ScienceManager scienceManager;
    private CivilizationManager CM;
    private ResourceManager resourceManager;
    [HideInInspector] public Dictionary<string, Resource> extractionBuildings = new Dictionary<string, Resource>();

    void Awake()
    {
        string jsonPath = Application.dataPath + "/Jsons/buildings.json";
        if (File.Exists(jsonPath))
        {
            string json = File.ReadAllText(jsonPath);
            BuildingWrapper buildingWrapper = JsonUtility.FromJson<BuildingWrapper>(json);
            buildings = buildingWrapper.buildings;
        }
        else
        {
            Debug.LogError("JSON file not found: " + jsonPath);
        }
    }

    private void Start() {
        scienceManager = GetComponent<ScienceManager>();
        resourceManager = GetComponent<ResourceManager>();
        CM = GetComponent<CivilizationManager>();

        foreach (Resource resource in resourceManager.resources) {
            extractionBuildings[GetBuilding(resource.extractionBuilding).Name] = resource;
        }
    }

    public Building GetBuilding(string buildingName) {return buildings.Find(x => x.Name == buildingName);}
    public string BuildingToolTip(Building building) {
        return building.Name + "\n" + building.Description;
    }

    private bool HasBuilding(List<Building> buildings, string buildingName) {
        foreach (Building b in buildings) {
            if (b.Name == buildingName) {
                return true;
            }
        }
        return false;
    }
    public List<Building> PossibleBuildings(City city, int ownerId) {
        List<Building> possibleBuildings = new List<Building>();
        foreach (Building building in buildings) {
            if (!building.IsRepeatable) {
                if (HasBuilding(city.buildings, building.Name)) {
                    continue;
                }
            }
            if (building.researchRequirement != -1) {
                if (!CM.GetCiv(ownerId).scienceIdentity.IsResearched(building.researchRequirement)) {
                    continue;
                }
            }
            bool canBuild = true;
            foreach (string requiredBuilding in building.requiredBuildings) {
                if (!city.buildings.Exists(x => x.Name == requiredBuilding)) {
                    canBuild = false;
                    break;
                }
            }
            if (canBuild) {
                possibleBuildings.Add(building);
            }
        }
        return possibleBuildings;
    }
}

[System.Serializable]
public class BuildingWrapper {
    public List<Building> buildings;
}

[System.Serializable]
public class YieldsHolder {
    public int Housing = 0;
    public float Food = 0;
    public float ProductionPoints = 0;
    public float Science = 0;
    public float Gold = 0;

    public YieldsHolder(int Housing = 0, float Food = 0, float ProductionPoints = 0, float Science = 0, float Gold = 0) {
        this.Housing = Housing;
        this.Food = Food;
        this.ProductionPoints = ProductionPoints;
        this.Science = Science;
        this.Gold = Gold;
    }

    public void AddYields(YieldsHolder yieldsHolder) {
        this.Housing += yieldsHolder.Housing;
        this.Food += yieldsHolder.Food;
        this.ProductionPoints += yieldsHolder.ProductionPoints;
        this.Science += yieldsHolder.Science;
        this.Gold += yieldsHolder.Gold;
    }    
}

//buildings that are inside cities
/*IF ERROR W/ BUILDING: ok listen myself in the future, there is a potential problem with Building class; i think that somewhere in my code i am comparing a Building from buildings List in BuildingManager to a real building thats part of a city; that might stop working bc now i have parameters in the building class that dont remain the same relative to their original state as recieved from the json file, i.e. position*/
[System.Serializable]
public class Building {
    public string Name;
    public string Description;
    public int Cost; //production cost
    public string ExtraType; //reference to name of tile in TileMapManager
    public int researchRequirement; //research required to build, -1 if none
    public List<string> requiredBuildings; //list of buildings required to build
    public List<ResourceRequirement> resourceRequirements; //list of resources required to build //NOT ADDED YET
    public string TerrainType = ""; //terrain type the building can be built on
    public YieldsHolder Yields = new YieldsHolder();
    public string IconPath = "Buildings/default";
    public bool IsRepeatable = false;
    public Vector2Int Position;

    public Building(string Name, string Description, int Cost, string ExtraType, int researchRequirement, List<string> requiredBuildings, List<ResourceRequirement> resourceRequirements, YieldsHolder Yields, string IconPath = null, bool IsRepeatable = false, string TerrainType = "")
    {
        this.Name = Name;
        this.Description = Description;
        this.Cost = Cost;
        this.ExtraType = ExtraType;
        this.researchRequirement = researchRequirement;
        this.requiredBuildings = requiredBuildings;
        this.resourceRequirements = resourceRequirements;
        this.Yields = Yields;
        if (IconPath != null) {
            this.IconPath = IconPath;
        }
        if (IsRepeatable) {
            this.IsRepeatable = IsRepeatable;
        }
        if (TerrainType != "") {
            this.TerrainType = TerrainType;
        }
    }

    public void ApplyBuildingEffects(City city) {
        city.Yields.AddYields(this.Yields);

        //apply specific building effects per name
        //match (Name) {case "...": ...}
    }
}

[System.Serializable]
public class ResourceRequirement {
    public string ResourceName;
    public int Amount;

    public ResourceRequirement(string ResourceName, int Amount) {
        this.ResourceName = ResourceName;
        this.Amount = Amount;
    }
}