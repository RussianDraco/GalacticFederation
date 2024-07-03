using UnityEngine;

public class CityOptionScript : MonoBehaviour {
    private object cityOption;
    CityManageController cityManageController;

    public void SetCityOption(CityManageController cmc, object cityOption) {
        cityManageController = cmc;
        this.cityOption = cityOption;
    }

    public void OnClick() {
        cityManageController.CityOptionClicked(cityOption);
    }
}