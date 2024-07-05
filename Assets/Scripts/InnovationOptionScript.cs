using UnityEngine;
using UnityEngine.UI;

public class InnovationOptionScript : MonoBehaviour {
    public int id;

    public void UpdateOption(bool researched, Sprite icon, int id, bool canResearch) {
        GetComponent<Image>().sprite = icon;
        this.id = id;
        if (researched) {
            GetComponent<Image>().color = new Color(0.5f, 1, 0.5f);
        } else if (canResearch) {
            GetComponent<Image>().color = new Color(1, 1, 1);
        } else {
            GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
        }
    }

    public void WasClicked() {
        GameObject.Find("ScienceMenu").GetComponent<ScienceMenu>().ResearchClicked(id);
    }
}