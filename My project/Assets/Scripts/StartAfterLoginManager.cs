using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartAfterLoginManager : MonoBehaviour
{
    [SerializeField] private bool isOn;

    [SerializeField] private GameObject levelPanel;

    [SerializeField] private Toggle singlePlayerButton;

    private string _username;
    // Start is called before the first frame update
    void Start()
    {
        _username = PlayerPrefs.GetString("username");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSinglePlayerSelectChange()
    {
        isOn = singlePlayerButton.isOn;
        if(isOn) levelPanel.SetActive(true);
        else levelPanel.SetActive(false);
    }

    public void OpenPreferenceShop()
    {
        SceneManager.LoadScene("Preference Shop");
    }
}
