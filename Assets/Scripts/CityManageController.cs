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

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && selectedCity != null)
            Deselect();
    }

    GameObject GetChild(GameObject parent) {
        foreach (Transform child in parent.transform) {
            return child.gameObject;
        }
        return null;
    }

    public void SelectCity(City city) {
        selectedCity = city;
        cityName.text = city.Name;
        foreach (Building building in city.buildings)
        {
            GameObject buildingObj = Instantiate(buildingPrefab, buildingList);
            GetChild(buildingObj).GetComponent<TMPro.TMP_Text>().text = building.Name;
        }
        foreach (string cityOption in cityManager.CityOptions())
        {
            GameObject cityOptionObj = Instantiate(cityManager, cityOptions);
            cityOptionObj.GetComponent<CityOptionScript>().SetCityOption(cityManager, cityOption);
            GetChild(cityOptionObj).GetComponent<TMPro.TMP_Text>().text = cityManager.ParseLiteralCityOption(cityOption);
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
}