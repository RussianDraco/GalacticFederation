using System.Collections;
using UnityEngine;

public class CityManageController : MonoBehaviour {
    [HideInInspector] public City selectedCity;
    public TMPro.TMP_Text cityName;
    public TMPro.TMP_Text productionText;

    public TMPro.TMP_Text housingText;
    public TMPro.TMP_Text populationText;
    public TMPro.TMP_Text foodText;
    public TMPro.TMP_Text productionPointsText;
    public TMPro.TMP_Text goldText;
    public TMPro.TMP_Text scienceText;
    
    public Transform buildingList;
    public Transform cityOptions;
    public GameObject cityControllerHolder;
    public GameObject buildingPrefab;
    public GameObject cityOptionPrefab;

    private CityManager cityManager;
    private BuildingManager buildingManager;
    private EntityManager entityManager;

    void Start()
    {
        cityManager = GameObject.Find("MANAGER").GetComponent<CityManager>();
        buildingManager = GameObject.Find("MANAGER").GetComponent<BuildingManager>();
        entityManager = GameObject.Find("MANAGER").GetComponent<EntityManager>();
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

    Sprite GrabIcon(string iconPath) {
        if (Resources.Load<Sprite>(iconPath) == null) {
            Debug.LogError("Icon not found at " + iconPath);
        }

        return Resources.Load<Sprite>(iconPath);
    }

    string GetCustomToolTip(string cityOption) {
        string[] optionParts = cityOption.Split('-');

        switch (optionParts[0]) {
            case "B":
                return buildingManager.BuildingToolTip(buildingManager.GetBuilding(optionParts[1]));
            case "CU":
                return entityManager.UnitToolTip(entityManager.GetCivil(optionParts[1]));    
            case "MU":
                return entityManager.UnitToolTip(entityManager.GetMilit(optionParts[1]));
            default:
                return "No tooltip available";
        }
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
            buildingObj.GetComponent<CitysBuildingScript>().SetBuilding(building.Name, GrabIcon(building.IconPath), buildingManager.BuildingToolTip(building));
        }
        foreach (string cityOption in cityManager.CityOptions(city, -1))
        {
            GameObject cityOptionObj = Instantiate(cityOptionPrefab, cityOptions);
            cityOptionObj.GetComponent<CityOptionScript>().SetCityOption(cityManager, cityOption, cityManager.ParseLiteralCityOption(cityOption), cityManager.CityOptionIcon(cityOption), GetCustomToolTip(cityOption));
        }

        housingText.text = city.Yields.Housing.ToString();
        populationText.text = Mathf.Floor(city.Yields.Population).ToString();
        foodText.text = city.Yields.Food.ToString();
        goldText.text = city.Yields.Gold.ToString();
        productionPointsText.text = city.Yields.ProductionPoints.ToString();
        scienceText.text = city.Yields.Science.ToString();

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