using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainmenuManager : MonoBehaviour
{
    public void quitClicked()
    {
        Debug.Log("quit!");
        Application.Quit();
    }

    public void playClicked()
    {
        SceneManager.LoadScene("Login");
    }
}
