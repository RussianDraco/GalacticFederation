using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectEntityScript : MonoBehaviour
{
    public GameObject selectHolder;
    public TMP_Text nameText;
    public TMP_Text descText;
    public Image icon;
    public GameObject actionsHolder;
    public GameObject actionPrefab;
    public TMP_Text pointsText;

    public void Deselect() {
        selectHolder.SetActive(false);
    }

    public void SetCivil(string name, string desc, int movePoints, int actionPoints, Sprite icon, List<CivilAction> actions) {
        selectHolder.SetActive(true);
        nameText.text = name;
        descText.text = desc;
        this.icon.sprite = icon;
        foreach (Transform child in actionsHolder.transform) {
            Destroy(child.gameObject);
        }
        foreach (CivilAction act in actions) {
            GameObject action = Instantiate(actionPrefab, actionsHolder.transform);
            action.GetComponent<ActionOptionScript>().SetAction(act);
            action.transform.SetParent(actionsHolder.transform);
        }
        pointsText.text = movePoints + " MP\n" + actionPoints + " AP";
    }

    public void CivilReload(int movePoints, int actionPoints) {
        pointsText.text = movePoints + " MP\n" + actionPoints + " AP";
    }
}
