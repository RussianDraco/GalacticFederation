using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScienceMenu : MonoBehaviour
{   
    public GameObject holder;
    public TMP_Text innovationName;
    public TMP_Text innovationDescription;
    public GameObject innovationPropertyPanel;
    [HideInInspector] public int selectedInnovation;
    public ScienceManager scienceManager;

    Sprite GrabIcon(string iconPath) {
        if (Resources.Load<Sprite>(iconPath) == null) {
            Debug.LogError("Icon not found at " + iconPath);
        }

        return Resources.Load<Sprite>(iconPath);
    }

    public void IsActive(bool active)
    {
        selectedInnovation = -1;
        holder.SetActive(active);

        if (active) {
            foreach(Transform child in holder.transform)
            {
                InnovationOptionScript ios = child.GetComponent<InnovationOptionScript>();
                if (ios == null) continue;
                Innovation innovation = scienceManager.innovations[ios.id];
                ios.UpdateOption(
                    innovation.isResearched,
                    GrabIcon(innovation.IconPath),
                    innovation.Id,
                    scienceManager.CanResearch(innovation)
                );
            }
        }
    }

    public void ResearchClicked(int id)
    {
        if (selectedInnovation == id) {
            selectedInnovation = -1;
            innovationName.gameObject.SetActive(false);
            innovationDescription.gameObject.SetActive(false);
            innovationPropertyPanel.SetActive(false);
            scienceManager.StartResearch(id);
            GameObject.Find("MANAGER").GetComponent<MenuManager>().XButton();
            return;
        }

        selectedInnovation = id;
        innovationPropertyPanel.SetActive(true);
        innovationName.gameObject.SetActive(true);
        innovationDescription.gameObject.SetActive(true);
        innovationName.text = scienceManager.innovations[id].Name;
        innovationDescription.text = scienceManager.innovations[id].Description;
    }
}
