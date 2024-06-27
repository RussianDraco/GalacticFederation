using UnityEngine;

public class CivilScript : MonoBehaviour {
    public Civil civil;

    public void SetCivil(Civil civil, Sprite icon) {
        this.civil = civil;
        this.gameObject.name = civil.Name;
        this.gameObject.GetComponent<SpriteRenderer>().sprite = icon;
    }
}