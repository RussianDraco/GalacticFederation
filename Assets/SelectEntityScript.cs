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

    public void Deselect() {
        selectHolder.SetActive(false);
    }

    public void SetCivil(string name, string desc, Sprite icon) {
        selectHolder.SetActive(true);
        nameText.text = name;
        descText.text = desc;
        this.icon.sprite = icon;
    }
}
