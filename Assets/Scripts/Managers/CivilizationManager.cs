using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilizationManager : MonoBehaviour {
    public List<Civilization> civs = new List<Civilization>();
    public Civilization Player;

    private void Start() {
        civs.Add(new Civilization { Name = "Player", ownerId = -1 });
        Player = civs[0];
        civs.Add(new Civilization { Name = "AI 1", ownerId = 0 });


        GameObject manager = GameObject.Find("MANAGER");
        foreach (Civilization civ in civs) {
            civ.Start(manager);
        }
    }

    public Civilization GetCiv(int ownerId) {
        if (ownerId == -1) {return Player;}
        return civs[ownerId];
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

    public void Start(GameObject manager) {
        scienceIdentity = new ScienceIdentity(ownerId, manager, manager.GetComponent<ScienceManager>());
        scienceIdentity.Start();

        yieldIdentity = new YieldIdentity(ownerId, manager);
        yieldIdentity.Start();

        cityIdentity = new CityIdentity(ownerId);
    }

    public void NextTurn() {
        scienceIdentity.NextTurn();
        yieldIdentity.NextTurn();
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

    public void Start() {
        for (int i = 0; i < scienceManager.innovations.Count; i++) {
            researchedInnovations.Add(false);
        }
    }
    public void NextTurn() {
        if (currentResearch != null) {
            researchProgress += manager.GetComponent<YieldManager>().sciencePoints;
            
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

    public bool CanResearch(Innovation innovation) {
        if (IsResearched(innovation.Id)) return false; //already researched

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

    public void Start() {
        RecalculateYields(false);
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
    public List<int> cities = new List<int>(); //WILL NEED TO IMPLEMENT REMOVAL OF CITIES (DECREASING INDICES ABOVE THE DELETED ID)

    public void AddCity() {
        cities.Add(cities.Count - 1);
    }
}