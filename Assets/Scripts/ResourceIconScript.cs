using UnityEngine;

public class ResourceIconScript : MonoBehaviour {
    public UnityEngine.UI.Image icon;
    public TMPro.TextMeshProUGUI amount;

    public void SetResource(Sprite icon, int amount) {
        this.icon.sprite = icon;
        this.amount.text = amount.ToString();
    }
}