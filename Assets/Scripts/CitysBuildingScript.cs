using UnityEngine;

public class CitysBuildingScript : MonoBehaviour
{
    public TMPro.TMP_Text buildingName;
    public UnityEngine.UI.Image buildingIcon;
    public string ToolTipData;

    public void SetBuilding(string buildingName, Sprite buildingIcon, string ToolTipData)
    {
        this.buildingName.text = buildingName;
        this.buildingIcon.sprite = buildingIcon;
        this.ToolTipData = ToolTipData;
    }
}
