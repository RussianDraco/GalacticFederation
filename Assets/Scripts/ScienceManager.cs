using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScienceManager : MonoBehaviour {
    private YieldManager yieldManager;
    [HideInInspector] public List<Innovation> innovations = new List<Innovation>(); //starts with id 0; should be ordered by id
    Innovation currentResearch;
    float researchProgress = 0;
    public TMP_Text progressText;
    public Image progressImg;

    void Awake() {
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
    void Start() {
        yieldManager = GetComponent<YieldManager>();
        progressImg.sprite = GrabIcon("Innovations/default");

        Color imageColor = progressImg.color;
        imageColor.a = 1.0f;
        progressImg.color = imageColor;
        
        progressText.text = "";
    }

    public bool IsResearched(int innovId) {return innovations[innovId].isResearched;}

    public bool CanResearch(Innovation innovation) {
        if (innovation.isResearched) return false; //already researched

        foreach (int id in innovation.Prerequisites) {
            if (!innovations[id].isResearched) { //INNOVATIONS.JSON MUST BE ORDERED BY ID
                return false;
            }
        }
        return true;
    }

    Sprite GrabIcon(string iconPath) {
        if (Resources.Load<Sprite>(iconPath) == null) {
            Debug.LogError("Icon not found at " + iconPath);
        }

        return Resources.Load<Sprite>(iconPath);
    }
    public void StartResearch(int innovId) {
        Innovation innovation = innovations[innovId];
        if (CanResearch(innovation)) {
            currentResearch = innovation;
            researchProgress = 0;
            progressImg.sprite = GrabIcon(innovation.IconPath);

            Color imageColor = progressImg.color;
            imageColor.a = 0.5f;
            progressImg.color = imageColor;

            progressText.text = (Mathf.Round(researchProgress / (float)currentResearch.Cost * 100)).ToString() + "%";
        }
    }

    public void NextTurn() {
        if (currentResearch != null) {
            researchProgress += yieldManager.sciencePoints;
            progressText.text = (Mathf.Round(researchProgress / (float)currentResearch.Cost * 100)).ToString() + "%";
            if (researchProgress >= currentResearch.Cost) {
                currentResearch.isResearched = true;
                Debug.Log("Research: " + currentResearch.Name + " completed!");
                currentResearch = null;
                researchProgress = 0;
                progressImg.sprite = GrabIcon("Innovations/default");

                Color imageColor = progressImg.color;
                imageColor.a = 1.0f;
                progressImg.color = imageColor;

                progressText.text = "";
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