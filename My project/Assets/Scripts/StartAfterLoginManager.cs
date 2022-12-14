using System;
using System.Collections;
using System.Collections.Generic;
using Model;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartAfterLoginManager : MonoBehaviour
{
    [SerializeField] private bool isOn;

    [SerializeField] private GameObject levelPanel;

    [SerializeField] private Toggle singlePlayerButton;

    public static string userName = "SuperGummy";
    [SerializeField] private TextMeshProUGUI welcomeText;
    
    // Start is called before the first frame update
    void Start()
    {
        userName = PlayerPrefs.GetString("username");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        welcomeText.text = "Welcome home " + userName + "!";
    }

    public void OnSinglePlayerSelectChange()
    {
        isOn = singlePlayerButton.isOn;
        if(isOn) levelPanel.SetActive(true);
        else levelPanel.SetActive(false);
    }

    public void OnDoublePlayerSelect()
    {
        SceneManager.LoadScene("Game");
    }

    public void OpenPreferenceShop()
    {
        SkinShopManager.userName = userName;
        SkinShopManager.calledFromGame = false;
        SceneManager.LoadScene("Preference Shop");
    }
}