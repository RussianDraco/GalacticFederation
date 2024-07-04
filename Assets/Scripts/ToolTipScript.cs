using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

//resizeable panel prob. needs to be fixed

public class ToolTipScript : MonoBehaviour
{
    public GameObject toolTip;
    public TMP_Text toolTipText;

    private Vector3 previousMousePosition;
    private float idleTime;

    void Start()
    {
        toolTip.SetActive(false);
    }

    void Update()
    {
        if (Input.mousePosition != previousMousePosition)
        {
            idleTime = 0f; // Reset idle time
            HideToolTip();
        }
        else
        {
            idleTime += Time.deltaTime; // Increment idle time
            if (idleTime >= 1f)
            {
                List<RaycastResult> results = PointerOverUIElements();
                if (results.Count > 0)
                {
                    DisplayUIToolTip(results);
                }
                else
                {
                    DisplayTileTip();
                }
            }
        }

        previousMousePosition = Input.mousePosition; // Update previous mouse position
    }

    void DisplayTileTip()
    {
        GameObject manager = GameObject.Find("MANAGER");
        TileMap tilemap = manager.GetComponent<GameManager>().tileMap;
        Tile tile;
        try
        {
            tile = tilemap.Tiles[tilemap.GetGridPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition))];
        }
        catch
        {
            HideToolTip();
            return;
        }

        EntityManager entityManager = manager.GetComponent<EntityManager>();
        object entity = entityManager.EntityOn(tile.Position);

        string finalToolTip = tile.TerrainType.Replace("basicmars", "Mars Plains");

        if (tile.ExtraType != null)
        {
            if (tile.ExtraType == "City")
            {
                City city = manager.GetComponent<CityManager>().CityOnPosition(tile.Position);
                finalToolTip += "\n" + city.Name + " (" + city.Owner + ")";
                if (city.buildings.Count > 0)
                {
                    finalToolTip += "\n[";
                    foreach (Building building in city.buildings)
                    {
                        finalToolTip += building.Name + ", ";
                    }
                    finalToolTip = finalToolTip.Remove(finalToolTip.Length - 2) + "]";
                }
            }
            else
            {
                finalToolTip += "\n" + tile.ExtraType;
            }
        }

        if (entity != null)
        {
            if (entity is Civil)
            {
                finalToolTip += "\n" + ((Civil)entityManager.EntityOn(tile.Position)).Name + " (" + ((Civil)entityManager.EntityOn(tile.Position)).Owner + ")";
            }
            else
            {
                finalToolTip += "\n" + ((Milit)entityManager.EntityOn(tile.Position)).Name + " (" + ((Milit)entityManager.EntityOn(tile.Position)).Owner + ")";
            }
        }

        ShowToolTip(finalToolTip);
    }

    void DisplayUIToolTip(List<RaycastResult> results)
    {
        foreach (RaycastResult result in results)
        {
            GameObject rgo = result.gameObject;
            if (rgo.GetComponent<CitysBuildingScript>()) {
                ShowToolTip(rgo.GetComponent<CitysBuildingScript>().ToolTipData);
                return;
            } else if (rgo.GetComponent<CityOptionScript>()) {
                ShowToolTip(rgo.GetComponent<CityOptionScript>().ToolTipData);
                return;
            }
        }
    }

    void ShowToolTip(string text)
    {
        toolTipText.text = text;
        RectTransform toolTipRectTransform = toolTip.GetComponent<RectTransform>();
        toolTipRectTransform.position = Input.mousePosition + new Vector3(toolTipRectTransform.sizeDelta.x / 2, -toolTipRectTransform.sizeDelta.y / 2, 0);
        toolTip.SetActive(true);
    }

    void HideToolTip()
    {
        toolTip.SetActive(false);
    }

    List<RaycastResult> PointerOverUIElements()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results;
    }
}
