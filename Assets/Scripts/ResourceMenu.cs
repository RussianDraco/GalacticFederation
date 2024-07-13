using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceMenu : MonoBehaviour {
    public GameObject holder;
    public GameObject resourcePrefab;
    public Transform iconsHolder;

    private CivilizationManager CM;
    private ResourceManager resourceManager;

    private void Start() {
        CM = GameObject.Find("MANAGER").GetComponent<CivilizationManager>();
        resourceManager = GameObject.Find("MANAGER").GetComponent<ResourceManager>();
    }

    Sprite GrabIcon(string iconPath) {
        if (Resources.Load<Sprite>(iconPath) == null) {
            Debug.LogError("Icon not found at " + iconPath);
        }

        return Resources.Load<Sprite>(iconPath);
    }

    public void IsActive(bool active)
    {
        holder.SetActive(active);

        //can make more efficent later by stopping deletion every time
        if (active) {
            foreach (Transform child in iconsHolder)
            {
                Destroy(child.gameObject);
            }

            Dictionary<string, int> playerResources = CM.Player.resourceIdentity.resources;
            foreach (string resourceName in playerResources.Keys)
            {
                GameObject resourceIcon = Instantiate(resourcePrefab, iconsHolder);
                resourceIcon.GetComponent<ResourceIconScript>().SetResource(GrabIcon(resourceManager.GetResource(resourceName).IconPath), playerResources[resourceName], resourceName);
            }
        }
    }
}