using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Model;
using SaveLoad;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartAfterLoginManager : MonoBehaviour
{
    [SerializeField] private bool isOn;

    [SerializeField] private GameObject levelPanel;

    [SerializeField] private Toggle singlePlayerButton;

    [SerializeField] private TextMeshProUGUI welcomeText;
    public static StartAfterLoginManager Instance;
    public string userName;

    
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        userName = PlayerPrefs.GetString("username", "123");
        GameManager.Load = false;
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
        PVP();
    }

    public void OpenLoadGame()
    {
        SceneController.LoadSaveLoad(SaveOrLoad.LOAD);
    }

    public void OpenPreferenceShop()
    {
        SkinShopManager.userName = userName;
        SkinShopManager.calledFromGame = false;
        SceneManager.LoadScene("Preference Shop");
    }

    public void LoadArchive(string path)
    {
        SceneManager.LoadSceneAsync("Game");
        SceneManager.LoadScene("Loading", LoadSceneMode.Additive);
        GameManager.Load = true;
        GameManager.LoadPath = path;
    }

    public void PVP()
    {
        SceneManager.LoadSceneAsync("Game");
        SceneManager.LoadScene("Loading", LoadSceneMode.Additive);
        GameManager.pvp = true;
    }

    public void EasyAI()
    {
        SceneManager.LoadSceneAsync("Game");
        SceneManager.LoadScene("Loading", LoadSceneMode.Additive);
        GameManager.pvp = false;
        GameManager.AI = AI.GetAI(false);
    }

    public void HardAI()
    {
        SceneManager.LoadSceneAsync("Game");
        SceneManager.LoadScene("Loading", LoadSceneMode.Additive);
        GameManager.pvp = false;
        GameManager.AI = AI.GetAI(true);
    }
}