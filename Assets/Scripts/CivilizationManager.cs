using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilizationManager : MonoBehaviour {
    public List<Civilization> civilizations = new List<Civilization>();

    private void Start() {
        civilizations.Add(new Civilization() {
            Name = "Enemy",
            ownerId = 0
        });

        foreach (Civilization civ in civilizations) {
            civ.Start();
        }
    }

    public string GetCivName(int id) {
        if (id == -1) {return "Player";}
        return civilizations[id].Name;
    }

    public void NextTurn() {
        foreach (Civilization civ in civilizations) {
            civ.NextTurn();
        }
    }
}

public class Civilization { //class for a civ other than the player
    public string Name;
    public int ownerId; //-1 is the player
    private GameObject manager;

    public void Start() {
        RecalculateYields(false);
        manager = GameObject.Find("MANAGER");

        for (int i = 0; i < manager.GetComponent<ScienceManager>().innovation.Count; i++) {
            researchedInnovations.Add(false);
        }
    }

    public void NextTurn() {
        RecalculateYields();
    }

    //yield
    public float Gold = 0f;
    
    public float sciencePoints = 1f;
    public float goldPoints = 1f;

    private void CollectYields() {
        Gold += goldPoints;
    }

    private void RecalculateYields(bool collect = true) {    
        sciencePoints = 1f;
        goldPoints = 1f;

        if (collect)
            CollectYields();
    }


    //science
    private Innovation currentResearch;
    private float researchProgress = 0;

    private List<bool> researchedInnovations = new List<bool>();

    public bool IsResearched(int innovId) {return researchedInnovations[innovId];}

    public bool CanResearch(Innovation innovation) {
        if (researchedInnovations[innovation.Id]) return false; //already researched

        foreach (int id in innovation.Prerequisites) {
            if (!researchedInnovations[id]) { //INNOVATIONS.JSON MUST BE ORDERED BY ID
                return false;
            }
        }
        return true;
    }

    public void StartResearch(int innovId) {
        Innovation innovation = manager.GetComponent<ScienceManager>().innovations[innovId];
        if (CanResearch(innovation)) {
            currentResearch = innovation;
            researchProgress = 0;
        }
    }

    void ScienceNextTurn() {
        if (currentResearch != null) {
            researchProgress += sciencePoints;
            if (researchProgress >= currentResearch.Cost) {
                researchedInnovations[currentResearch.Id] = true;
                currentResearch = null;
                researchProgress = 0;
            }
        }
    }
}