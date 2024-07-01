using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScienceManager : MonoBehaviour {
    [HideInInspector] public List<Innovation> innovations = new List<Innovation>(); //starts with id 0; should be ordered by id
    Innovation currentResearch;
    int researchProgress = 0;

    void Awake()
    {
        string jsonPath = Application.dataPath + "/Jsons/innovations.json";
        if (System.IO.File.Exists(jsonPath))
        {
            string json = System.IO.File.ReadAllText(jsonPath);
            InnovationWrapper innovationWrapper = JsonUtility.FromJson<InnovationWrapper>(json);
            innovations = innovationWrapper.innovations;
        }
        else
        {
            Debug.LogError("JSON file not found: " + jsonPath);
        }
    
    }

    public bool IsResearched(int innovId) {return innovations[innovId].isResearched;}

    bool CanResearch(Innovation innovation) {
        foreach (int id in innovation.Prerequisites) {
            if (!innovations[id].isResearched) { //INNOVATIONS.JSON MUST BE ORDERED BY ID
                return false;
            }
        }
        return true;
    }

    public void StartResearch(int innovId) {
        Innovation innovation = innovations[innovId];
        if (CanResearch(innovation)) {
            currentResearch = innovation;
            researchProgress = 0;
        }
    }

    public void NextTurn() {
        if (currentResearch != null) {
            researchProgress++;
            if (researchProgress >= currentResearch.Cost) {
                currentResearch.isResearched = true;
                currentResearch = null;
                researchProgress = 0;
            }
        }
    }
}

[System.Serializable]
public class InnovationWrapper {
    public List<Innovation> innovations;
}

[System.Serializable]
public class Innovation {
    public string Name;
    public string Description;
    public int Id;
    public string IconPath;
    public int Cost;
    public bool isResearched;
    public List<int> Prerequisites; // List of Innovation Ids

    public Innovation(string Name, string Description, int Id, string IconPath, int Cost, List<int> Prerequisites, bool isResearched = false) {
        this.Name = Name;
        this.Description = Description;
        this.Id = Id;
        this.IconPath = IconPath;
        this.Cost = Cost;
        this.Prerequisites = Prerequisites;
        this.isResearched = isResearched;
    }
}