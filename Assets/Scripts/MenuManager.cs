using System.Collections;
using UnityEngine;

public class MenuManager : MonoBehaviour {
    public GameObject xButton;

    bool scienceMenuOpen = false;

    public ScienceMenu scienceMenu;

    public void XButton() {
        scienceMenuOpen = false;
        scienceMenu.IsActive(false);

        xButton.IsActive(false);
    }

    public void ScienceMenuButton() {
        scienceMenuOpen = true;
        scienceMenu.IsActive(true);
        xButton.SetActive(true);
    }
}