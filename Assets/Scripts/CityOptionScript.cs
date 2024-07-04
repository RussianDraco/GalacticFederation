using UnityEngine;

public class CityOptionScript : MonoBehaviour {
    string cityOption;
    CityManager cityManager;
    public TMPro.TMP_Text optionText;
    public UnityEngine.UI.Image buildingIcon;
    public string ToolTipData;

    public void SetCityOption(CityManager cityManager, string cityOption, string textToSet, Sprite buildingIcon, string ToolTipData) {
        this.cityManager = cityManager;
        this.cityOption = cityOption;
        optionText.text = textToSet;
        this.buildingIcon.sprite = buildingIcon;
        this.ToolTipData = ToolTipData;
    }

    public void OnClick() {
        cityManager.CityOptionFunction(cityOption);
    }
}