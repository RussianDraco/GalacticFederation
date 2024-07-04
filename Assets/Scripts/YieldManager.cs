using System.Collections;
using UnityEngine;

public class YieldManager : MonoBehaviour {
    [HideInInspector] public float Gold = 0f;

    [HideInInspector] public float sciencePoints = 1f;
    [HideInInspector] public float goldPoints = 1f;

    public TMPro.TMP_Text goldText;
    public TMPro.TMP_Text scienceText;

    private CityManager cityManager;

    private void Start() {
        cityManager = GetComponent<CityManager>();

        RecalculateYields(false);
    }

    void CollectYields() {
        Gold += goldPoints;
    }

    public void RecalculateYields(bool collect = true) {    
        sciencePoints = 1f;
        goldPoints = 1f;

        cityManager.AddCityYields();

        if (collect)
            CollectYields();

        goldText.text = Gold.ToString("F1") + " (+" + goldPoints.ToString("F1") + ")";
        scienceText.text = "+" + sciencePoints.ToString("F1");
    }

    public void NextTurn() {
        RecalculateYields();
    }
}