using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ResourceManager : MonoBehaviour {
    [HideInInspector] public List<Resource> resources = new List<Resource>();

    private CivilizationManager civilizationManager;

    public Resource GetResource(string name) {return resources.Find(x => x.Name == name);}

    void Awake()
    {
        string jsonPath = Application.dataPath + "/Jsons/resources.json";
        if (File.Exists(jsonPath))
        {
            string json = File.ReadAllText(jsonPath);
            ResourceWrapper resourceWrapper = JsonUtility.FromJson<ResourceWrapper>(json);
            resources = resourceWrapper.resources;
        }
        else
        {
            Debug.LogError("JSON file not found: " + jsonPath);
        }

        civilizationManager = GetComponent<CivilizationManager>();
    }

    public bool CanMakeTerrain(string terrainType, int ownerId) {
        foreach (Resource resource in resources) {
            if (resource.TerrainType == terrainType) {
                if (resource.innovationRequirement == -1) {
                    return true;
                }
                //the ai might see resources bc of this
                if (civilizationManager.GetCiv(ownerId).scienceIdentity.IsResearched(resource.innovationRequirement)) {
                    return true;
                } else {
                    return false;
                }
            }
        }
        Debug.LogError("CanMakeTerrain called with non-resource terrain type");
        return true;
    }
}


[System.Serializable]
public class ResourceWrapper {
    public List<Resource> resources = new List<Resource>();
}

[System.Serializable]
public class Resource {
    public string Name;
    public string TerrainType;
    public string IconPath;
    public int Id;
    public int innovationRequirement;
    public string extractionBuilding;

    public Resource(string Name, string TerrainType, string IconPath, int Id, int innovationRequirement, string extractionBuilding) { //maybe switch buildings to id later for efficency
        this.Name = Name;
        this.TerrainType = TerrainType;
        this.IconPath = IconPath;
        this.Id = Id;
        this.innovationRequirement = innovationRequirement;
        this.extractionBuilding = extractionBuilding;
    }
}