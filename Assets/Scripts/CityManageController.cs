using System.Collections;
using UnityEngine;

public class CityManageController : MonoBehaviour {
    [HideInInspector] public City selectedCity;
    public TMPro.TMP_Text cityName;
    public Transform buildingList;
    public Transform cityOptions;
    public GameObject cityControllerHolder;
    public GameObject buildingPrefab;
    public GameObject cityOptionPrefab;
    private CityManager cityManager;

    void Start()
    {
        cityManager = GameObject.Find("MANAGER").GetComponent<CityManager>();
    }

    public void SelectCity(City city) {
        selectedCity = city;
        cityName.text = city.Name;
        foreach (Building building in city.buildings)
        {
            GameObject buildingObj = Instantiate(buildingPrefab, buildingList);
            buildingObj.GetComponent<TMPro.TMP_Text>().text = building.Name;
        }
        foreach (object cityOption in cityManager.CityOptions())
        {
            GameObject cityOptionObj = Instantiate(cityOptionPrefab, cityOptions);

            cityOptionObj.GetComponent<TMPro.TMP_Text>().text = cityOption.ToString();
        }

        cityControllerHolder.SetActive(true);
    }

    public void Deselect() {
        selectedCity = null;
        cityName.text = "";
        foreach (Transform child in buildingList)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in cityOptions)
        {
            Destroy(child.gameObject);
        }
        cityControllerHolder.SetActive(false);
    }

    public void CityOptionClicked(object cityOption) {
        //handle city option click
    }
}