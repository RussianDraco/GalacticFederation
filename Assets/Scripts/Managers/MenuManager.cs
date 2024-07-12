using System.Collections;
using UnityEngine;

public class MenuManager : MonoBehaviour {
    public GameObject UICanvas;
    public GameObject MenuCanvas;

    public GameObject xButton;

    public ScienceMenu scienceMenu;
    public ResourceMenu resourceMenu;

    void Start()
    {
        scienceMenu.IsActive(false);
        resourceMenu.IsActive(false);

        MenuChange(false);
    }

    void MenuChange(bool isON) {
        UICanvas.SetActive(!isON);
        MenuCanvas.SetActive(isON);
        xButton.SetActive(isON);
    }

    public void XButton() {
        scienceMenu.IsActive(false);
        resourceMenu.IsActive(false);

        MenuChange(false);
    }

    public void ScienceMenuButton() {
        scienceMenu.IsActive(true);

        MenuChange(true);
    }

    public void ResourceMenuButton() {
        resourceMenu.IsActive(true);

        MenuChange(true);
    }
}