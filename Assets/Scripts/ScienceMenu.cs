using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScienceMenu : MonoBehaviour
{   
    public GameObject holder;

    Sprite GrabIcon(string iconPath) {
        if (Resources.Load<Sprite>(iconPath) == null) {
            Debug.LogError("Icon not found at " + iconPath);
        }

        return Resources.Load<Sprite>(iconPath);
    }

    public void IsActive(bool active)
    {
        holder.SetActive(active);

        if (active) {
            ScienceManager sm = GameObject.Find("GameManager").GetComponent<ScienceManager>();
            foreach(Transform child in holder.transform)
            {
                InnovationOptionScript ios = child.GetComponent<InnovationOptionScript>();
                Innovation innovation = sm.innovations[ios.id];
                ios.UpdateOption(
                    innovation.isResearched,
                    GrabIcon(innovation.IconPath)
                );
            }
        }
    }

    public void ResearchClicked(int id)
    {
        // do something
    }
}
