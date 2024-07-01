using UnityEngine;

public class InnovationOptionScript : MonoBehaviour {
    public int id;

    public void UpdateOption(bool researched, Sprite icon) {
        GetComponent<SpriteRenderer>().sprite = icon;
    }
}