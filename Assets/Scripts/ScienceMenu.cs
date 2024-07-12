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
    public RectTransform innovationsHolder;
    private CivilizationManager CM;

    private void Awake() {
        CM = GameObject.Find("MANAGER").GetComponent<CivilizationManager>();
    }

    void Update()
    {
        if (!holder.activeSelf) return;

        float scrollDelta = Input.mouseScrollDelta.y;
        if (scrollDelta > 0 && innovationsHolder.localPosition.x < 20)
        {
            innovationsHolder.localPosition += new Vector3(20, 0, 0);
        } else if (scrollDelta < 0) {
            innovationsHolder.localPosition -= new Vector3(20, 0, 0);
        }
    }

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
            innovationsHolder.localPosition = new Vector3(0, 0, 0);

            foreach(Transform child in innovationsHolder.gameObject.transform)
            {
                InnovationOptionScript ios = child.GetComponent<InnovationOptionScript>();
                if (ios == null) continue;
                Innovation innovation = scienceManager.innovations[ios.id];
                ios.UpdateOption(
                    CM.Player.scienceIdentity.IsResearched(innovation.Id),
                    GrabIcon(innovation.IconPath),
                    innovation.Id,
                    CM.Player.scienceIdentity.CanResearch(innovation)
                );
            }
        }
    }

    private string ResearchPrerequisites(Innovation innovation) {
        string prerequisites = "";
        foreach (int id in innovation.Prerequisites) {
            if (!CM.Player.scienceIdentity.IsResearched(id)) {
                prerequisites += scienceManager.innovations[id].Name + ", ";
            }
        }
        return prerequisites.Substring(0, prerequisites.Length - 2);
    }

    public void ResearchClicked(int id)
    {
        if (selectedInnovation == id && !CM.Player.scienceIdentity.IsResearched(id) && CM.Player.scienceIdentity.CanResearch(scienceManager.innovations[id])) {
            selectedInnovation = -1;
            innovationName.gameObject.SetActive(false);
            innovationDescription.gameObject.SetActive(false);
            innovationPropertyPanel.SetActive(false);
            CM.Player.scienceIdentity.StartResearch(id);
            GameObject.Find("MANAGER").GetComponent<MenuManager>().XButton();
            return;
        }

        selectedInnovation = id;
        innovationPropertyPanel.SetActive(true);
        innovationName.gameObject.SetActive(true);
        innovationDescription.gameObject.SetActive(true);
        if (CM.Player.scienceIdentity.IsResearched(id)) {
            innovationName.text = scienceManager.innovations[id].Name + " (Researched)";
        } else {
            innovationName.text = scienceManager.innovations[id].Name;
        }
        if (CM.Player.scienceIdentity.CanResearch(scienceManager.innovations[id], true)) {
            innovationDescription.text = scienceManager.innovations[id].Description;        
        } else {
            innovationDescription.text = scienceManager.innovations[id].Description + " (Requires: " + ResearchPrerequisites(scienceManager.innovations[id]) + ")";
        }
    }
}
