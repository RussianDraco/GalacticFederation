using System.Collections;
using UnityEngine;

public class YieldManager : MonoBehaviour {
    [HideInInspector] public int sciencePoints = 1;
    [HideInInspector] public int goldPoints = 1;

    private CityManager cityManager;

    public void RecalculateYields() {
        sciencePoints = 1;
        goldPoints = 1;

        cityManager.AddCityYields();
    }
}