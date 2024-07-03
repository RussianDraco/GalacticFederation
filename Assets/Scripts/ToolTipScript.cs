using System.Collections;
using UnityEngine;

public class ToolTipScript : MonoBehaviour {
    public GameObject toolTip;
    public TMPro.TMP_Text toolTipText;

    private Vector3 previousMousePosition;
    private float idleTime;

    void Start()
    {
        toolTip.SetActive(false);
    }

    void Update() {
        if (Input.mousePosition != previousMousePosition) {
            idleTime = 0f; // Reset idle time
            HideToolTip();
        }
        else {
            idleTime += Time.deltaTime; // Increment idle time
            if (idleTime >= 1f) {
                DisplayTileTip();
            }
        }

        previousMousePosition = Input.mousePosition; // Update previous mouse position
    }

    void DisplayTileTip() {
        TileMap tilemap = GameObject.Find("MANAGER").GetComponent<GameManager>().tileMap;
        Tile tile;
        try {
            tile = tilemap.Tiles[tilemap.GetGridPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition))];
        } catch {
            HideToolTip();
            return;
        }

        EntityManager entityManager = GameObject.Find("MANAGER").GetComponent<EntityManager>();
        object entity = entityManager.EntityOn(tile.Position);

        string finalToolTip = tile.TerrainType;

        if (tile.ExtraType != null) {
            finalToolTip += "\n" + tile.ExtraType;
        }

        if (entity != null) {
            if (entity is Civil) {
                finalToolTip += "\n" + ((Civil)entityManager.EntityOn(tile.Position)).Name;
            } else {
                finalToolTip += "\n" + ((Milit)entityManager.EntityOn(tile.Position)).Name;
            }
        }

        ShowToolTip(finalToolTip);
    }

    void ShowToolTip(string text) {
        toolTipText.text = text;
        RectTransform toolTipRectTransform = toolTip.GetComponent<RectTransform>();
        toolTipRectTransform.position = Input.mousePosition + new Vector3(toolTipRectTransform.sizeDelta.x / 2, -toolTipRectTransform.sizeDelta.y / 2, 0);
        toolTip.SetActive(true);
    }

    void HideToolTip() {
        toolTip.SetActive(false);
    }
}