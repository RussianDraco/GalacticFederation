//Need to be derived from CivilScript.cs

using UnityEngine;

public class MilitScript : MonoBehaviour {
    [HideInInspector] public Milit milit;

    public void SetCivil(Milit milit, Sprite icon) {
        this.milit = milit;
        this.gameObject.name = milit.Name;
        this.gameObject.GetComponent<SpriteRenderer>().sprite = icon;
    }
}