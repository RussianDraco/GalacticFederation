using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScienceManager : MonoBehaviour {
    [HideInInspector] public List<Innovation> innovations = new List<Innovation>(); //starts with id 0; should be ordered by id
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
        progressImg.sprite = GrabIcon("Innovations/default");

        Color imageColor = progressImg.color;
        imageColor.a = 1.0f;
        progressImg.color = imageColor;
        
        progressText.text = "";
    }

    public Sprite GrabIcon(string iconPath) {
        if (Resources.Load<Sprite>(iconPath) == null) {
            Debug.LogError("Icon not found at " + iconPath);
        }

        return Resources.Load<Sprite>(iconPath);
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
    public List<int> Prerequisites; // List of Innovation Ids

    public Innovation(string Name, string Description, int Id, string IconPath, int Cost, List<int> Prerequisites) {
        this.Name = Name;
        this.Description = Description;
        this.Id = Id;
        this.IconPath = IconPath;
        this.Cost = Cost;
        this.Prerequisites = Prerequisites;
    }
}