using System.Collections;
using UnityEngine;

public class CityManageController : MonoBehaviour {
    [HideInInspector] public City selectedCity;
    public TMPro.TMP_Text cityName;

    public TMPro.TMP_Text productionText;
    public TMPro.TMP_Text housingText;
    public TMPro.TMP_Text populationText;
    public TMPro.TMP_Text foodText;
    public TMPro.TMP_Text productionText;
    public TMPro.TMP_Text goldText;
    public TMPro.TMP_Text scienceText;
    
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
        Deselect();

        selectedCity = city;
        cityName.text = city.Name;

        if (city.Production == null)
            productionText.text = "Producing: None";
        else
            productionText.text = "Producing: " + city.Production.Name + " (" + Mathf.Round(((float)city.ProductionProgress / (float)city.Production.Cost) * 100) + "%)";

        foreach (Building building in city.buildings)
        {
            GameObject buildingObj = Instantiate(buildingPrefab, buildingList);
            GetChild(buildingObj).GetComponent<TMPro.TMP_Text>().text = building.Name;
        }
        foreach (string cityOption in cityManager.CityOptions(city))
        {
            GameObject cityOptionObj = Instantiate(cityOptionPrefab, cityOptions);
            cityOptionObj.GetComponent<CityOptionScript>().SetCityOption(cityManager, cityOption);
            GetChild(cityOptionObj).GetComponent<TMPro.TMP_Text>().text = cityManager.ParseLiteralCityOption(cityOption);
        }
        productionText.text = city.Yields.ProductionPoints.ToString();
        

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