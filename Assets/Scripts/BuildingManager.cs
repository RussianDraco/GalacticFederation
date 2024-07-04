using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class BuildingManager : MonoBehaviour {
    [HideInInspector] public List<Building> buildings = new List<Building>();
    private ScienceManager scienceManager;

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

        scienceManager = GetComponent<ScienceManager>();
    }

    public List<Building> PossibleBuildings(City city) {
        List<Building> possibleBuildings = new List<Building>();
        foreach (Building building in buildings) {
            if (city.buildings.Contains(building)) {
                continue;
            }
            if (building.researchRequirement != -1) {
                if (!scienceManager.IsResearched(building.researchRequirement)) {
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

//buildings that are inside cities
[System.Serializable]
public class Building {
    public string Name;
    public string Description;
    public int Cost; //production cost
    public string BuildingFeature; //reference to BuildingGod.cs for the building's feature (thats the plan at least)
    public int researchRequirement; //research required to build, -1 if none
    public List<string> requiredBuildings; //list of buildings required to build
    public List<ResourceRequirement> resourceRequirements; //list of resources required to build //NOT ADDED YET

    public Building(string Name, string Description, int Cost, string BuildingFeature, int researchRequirement, List<string> requiredBuildings, List<ResourceRequirement> resourceRequirements)
    {
        this.Name = Name;
        this.Description = Description;
        this.Cost = Cost;
        this.BuildingFeature = BuildingFeature;
        this.researchRequirement = researchRequirement;
        this.requiredBuildings = requiredBuildings;
        this.resourceRequirements = resourceRequirements;
    }
}

[System.Serializable]
public class ResourceRequirement {
    public string resource;
    public int amount;
}