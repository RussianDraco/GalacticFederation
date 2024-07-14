using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilizationManager : MonoBehaviour {
    public List<Civilization> civs = new List<Civilization>();
    public Civilization Player;

    private void Awake() {
        civs.Add(new Civilization { Name = "Player", ownerId = -1 });
        Player = civs[0];
        civs.Add(new Civilization { Name = "AI 1", ownerId = 0 });

        GameObject manager = GameObject.Find("MANAGER");
        foreach (Civilization civ in civs) {
            civ.Init(manager);
        }
    }

    public Civilization GetCiv(int ownerId) {
        if (ownerId == -1) {return Player;}
        return civs[ownerId + 1]; //player will always be at index 0 so an offset is needed
    }

    public void NextTurn() {
        foreach (Civilization civ in civs) {
            civ.NextTurn();
        }
    }
}

public class Civilization { //class for a civ other than the player
    public string Name;
    public int ownerId; //-1 is the player
    public ScienceIdentity scienceIdentity;
    public YieldIdentity yieldIdentity;
    public CityIdentity cityIdentity;
    public EntityIdentity entityIdentity;
    public ResourceIdentity resourceIdentity;

    public void Init(GameObject manager) {
        if (scienceIdentity != null) {return;}
        scienceIdentity = new ScienceIdentity(ownerId, manager, manager.GetComponent<ScienceManager>());
        scienceIdentity.Init();

        yieldIdentity = new YieldIdentity(ownerId, manager);
        yieldIdentity.Init();

        cityIdentity = new CityIdentity(ownerId);

        entityIdentity = new EntityIdentity(ownerId);

        resourceIdentity = new ResourceIdentity(ownerId, manager);
        resourceIdentity.Init();
    }

    public void NextTurn() {
        scienceIdentity.NextTurn(yieldIdentity);
        yieldIdentity.NextTurn();
        resourceIdentity.NextTurn(cityIdentity);
    }
}

//identities hold information to communicate with managers as apart of civilizations

public class ScienceIdentity {
    public List<bool> researchedInnovations = new List<bool>();
    Innovation currentResearch;
    float researchProgress = 0;

    int Owner;
    ScienceManager scienceManager;
    GameObject manager;
    public ScienceIdentity(int Owner, GameObject manager, ScienceManager scienceManager) {
        this.Owner = Owner;
        this.manager = manager;
        this.scienceManager = scienceManager;
    }

    public void Init() {
        for (int i = 0; i < scienceManager.innovations.Count; i++) {
            researchedInnovations.Add(false);
        }
    }
    public void NextTurn(YieldIdentity yieldIdentity) {
        if (currentResearch != null) {
            researchProgress += yieldIdentity.sciencePoints;
            
            if (Owner == -1) {scienceManager.progressText.text = (Mathf.Round(researchProgress / (float)currentResearch.Cost * 100)).ToString() + "%";}
            
            if (researchProgress >= currentResearch.Cost) {
                researchedInnovations[currentResearch.Id] = true;
                currentResearch = null;
                researchProgress = 0;

                if (Owner == -1) {
                    scienceManager.progressImg.sprite = scienceManager.GrabIcon("Innovations/default");
                    Color imageColor = scienceManager.progressImg.color;
                    imageColor.a = 1.0f;
                    scienceManager.progressImg.color = imageColor;
                    scienceManager.progressText.text = "";
                }
            }
        }
    }

    public bool IsResearched(int innovId) {return researchedInnovations[innovId];}

    public bool CanResearch(Innovation innovation, bool onlypreqs = false) {
        if (IsResearched(innovation.Id) && !onlypreqs) return false; //already researched

        foreach (int id in innovation.Prerequisites) {
            if (!researchedInnovations[id]) { //INNOVATIONS.JSON MUST BE ORDERED BY ID
                return false;
            }
        }
        return true;
    }

    public void StartResearch(int innovId) {
        Innovation innovation = scienceManager.innovations[innovId];
        if (CanResearch(innovation)) {
            currentResearch = innovation;
            researchProgress = 0;

            if (Owner == -1) { //player specific actions
                scienceManager.progressImg.sprite = scienceManager.GrabIcon(innovation.IconPath);
                Color imageColor = scienceManager.progressImg.color;
                imageColor.a = 0.5f;
                scienceManager.progressImg.color = imageColor;
                scienceManager.progressText.text = (Mathf.Round(researchProgress / (float)currentResearch.Cost * 100)).ToString() + "%";
            }
        }
    }
}
public class YieldIdentity {
    public float Gold = 0f;

    public float sciencePoints = 1f;
    public float goldPoints = 1f;

    int Owner;
    GameObject manager;
    public YieldIdentity(int Owner, GameObject manager) {
        this.Owner = Owner;
        this.manager = manager;
    }

    public void Init() {
        //RecalculateYields(false);
    }

    void CollectYields() {
        Gold += goldPoints;
    }

    public void RecalculateYields(bool collect = true) {
        sciencePoints = 1f;
        goldPoints = 1f;

        manager.GetComponent<CityManager>().AddCityYields(Owner);

        if (collect) CollectYields();

        if (Owner == -1) {
            manager.GetComponent<YieldManager>().goldText.text = Gold.ToString("F1") + " (+" + goldPoints.ToString("F1") + ")";
            manager.GetComponent<YieldManager>().scienceText.text = "+" + sciencePoints.ToString("F1");
        }
    }

    public void NextTurn() {
        RecalculateYields();
    }
}
public class CityIdentity {
    public List<City> cities = new List<City>();
    
    int Owner;
    public CityIdentity(int Owner) {
        this.Owner = Owner;
    }

    public void AddCity(City city) {
        cities.Add(city);
    }
}

public class EntityIdentity {
    public List<Civil> civils = new List<Civil>();
    public List<Milit> milits = new List<Milit>();

    int Owner;
    public EntityIdentity(int Owner) {
        this.Owner = Owner;
    }

    public void AddCivil(Civil civil) {
        civils.Add(civil);
    }
    public void AddMilit(Milit milit) {
        milits.Add(milit);
    }

    public void KillCivil(Civil civil) {
        civils.Remove(civil);
    }
    public void KillMilit(Milit milit) {
        milits.Remove(milit);
    }
}

public class ResourceIdentity {
    public Dictionary<string, int> resources = new Dictionary<string, int>();

    int Owner;
    GameObject manager;
    public ResourceIdentity(int Owner, GameObject manager) {
        this.Owner = Owner;
        this.manager = manager;
    }

    public void Init() {
        foreach (Resource resource in manager.GetComponent<ResourceManager>().resources) {
            resources[resource.Name] = 0;
        }
    }

    //might need to optimize resource adding
    public void NextTurn(CityIdentity cityIdentity) {
        Dictionary<string, Resource> extractionBuildings = manager.GetComponent<BuildingManager>().extractionBuildings;
        foreach (City city in cityIdentity.cities) {
            foreach (Building building in city.buildings) {
                if (extractionBuildings.ContainsKey(building.Name)) {
                    resources[extractionBuildings[building.Name].Name] += 1;
                }
            }
        }
    }

    public bool HaveResources(List<ResourceRequirement> list) {
        foreach (ResourceRequirement requirement in list) {
            if (resources[requirement.ResourceName] < requirement.Amount) {
                return false;
            }
        }
        return true;
    }
    public bool RemoveResources(List<ResourceRequirement> list) {
        if (HaveResources(list)) {
            foreach (ResourceRequirement requirement in list) {
                resources[requirement.ResourceName] -= requirement.Amount;
            }
            return true;
        } else {return false;}
    }
}