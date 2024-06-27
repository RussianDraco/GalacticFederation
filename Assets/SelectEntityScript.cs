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

    public void Deselect() {
        selectHolder.SetActive(false);
    }

    public void SetCivil(string name, string desc, Sprite icon, List<CivilAction> actions) {
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
    }
}
