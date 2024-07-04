using UnityEngine;

public class CityOptionScript : MonoBehaviour {
    string cityOption;
    CityManager cityManager;

    public void SetCityOption(CityManager cityManager, string cityOption) {
        this.cityManager = cityManager;
        this.cityOption = cityOption;
    }

    public void OnClick() {
        cityManager.CityOptionFunction(cityOption);
    }
}