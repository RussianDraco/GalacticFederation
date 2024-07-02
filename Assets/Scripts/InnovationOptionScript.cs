using UnityEngine;
using UnityEngine.UI;

public class InnovationOptionScript : MonoBehaviour {
    public int id;

    public void UpdateOption(bool researched, Sprite icon, int id) {
        GetComponent<Image>().sprite = icon;
        this.id = id;
    }

    public void WasClicked() {
        GameObject.Find("ScienceMenu").GetComponent<ScienceMenu>().ResearchClicked(id);
    }
}