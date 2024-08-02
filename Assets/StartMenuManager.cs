using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    public void StartGame() {
        SceneManager.LoadScene("ActualGame");
    }

    public void QuitGame() {
        Application.Quit();
    }
}
