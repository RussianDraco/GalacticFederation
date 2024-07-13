using UnityEngine;

public class ResourceIconScript : MonoBehaviour {
    public UnityEngine.UI.Image icon;
    public TMPro.TextMeshProUGUI amount;
    public string ToolTipData;

    public void SetResource(Sprite icon, int amount, string resourceName) {
        this.icon.sprite = icon;
        this.amount.text = amount.ToString();
        ToolTipData = resourceName;
    }
}