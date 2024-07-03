using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class BuildingManager : MonoBehaviour {
    [HideInInspector] public List<Building> buildings = new List<Building>();

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
    public string BuildingFeature; //reference to BuildingGod.cs for the building's feature
    public int researchRequirement; //research required to build
    public List<string> requiredBuildings; //list of buildings required to build
    public List<ResourceRequirement> resourceRequirements; //list of resources required to build

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