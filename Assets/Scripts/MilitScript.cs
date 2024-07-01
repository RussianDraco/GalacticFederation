using UnityEngine;

public class MilitScript : MonoBehaviour {
    [HideInInspector] public Milit milit;

    public void SetMilit(Milit milit, Sprite icon) {
        this.milit = milit;
        this.gameObject.name = milit.Name;
        this.gameObject.GetComponent<SpriteRenderer>().sprite = icon;
    }

    private void OnMouseDown() {
        GameObject.Find("MANAGER").GetComponent<ActionManager>().SelectMilit(milit);
    }
}