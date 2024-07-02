using System.Collections;
using UnityEngine;

public class MenuManager : MonoBehaviour {
    public GameObject UICanvas;
    public GameObject MenuCanvas;

    public GameObject xButton;

    bool scienceMenuOpen = false;

    public ScienceMenu scienceMenu;

    void Start()
    {
        scienceMenuOpen = false;
        scienceMenu.IsActive(false);


        MenuChange(false);
    }

    void MenuChange(bool isON) {
        UICanvas.SetActive(!isON);
        MenuCanvas.SetActive(isON);
        xButton.SetActive(isON);
    }

    public void XButton() {
        scienceMenuOpen = false;
        scienceMenu.IsActive(false);

        MenuChange(false);
    }

    public void ScienceMenuButton() {
        scienceMenuOpen = true;
        scienceMenu.IsActive(true);

        MenuChange(true);
    }
}