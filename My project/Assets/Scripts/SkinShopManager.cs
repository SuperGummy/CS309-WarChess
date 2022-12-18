using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SkinShopManager : MonoBehaviour
{
    public static SkinShopManager Instance;
    
    [SerializeField] private GameObject shopPanel;
    public static string userName;
    public static bool calledFromGame;
    [SerializeField] private TextMeshProUGUI welcomeText;

    [SerializeField] private Toggle[] fighterBlueToggles;
    [SerializeField] private Toggle[] explorerBlueToggles;
    [SerializeField] private Toggle[] scholarBlueToggles;
    [SerializeField] private Toggle[] fighterRedToggles;
    [SerializeField] private Toggle[] explorerRedToggles;
    [SerializeField] private Toggle[] scholarRedToggles;

    private static int _fighterBlueId, _fighterRedId, _explorerBlueId, _explorerRedId;
    private static int _scholarBlueId, _scholarRedId;

    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject eventSystem;

    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        welcomeText.text = "Welcome home " + userName + "!";

        if (calledFromGame)
        {
            mainCamera.SetActive(false);
            eventSystem.SetActive(false);
        }
        else
        {
            mainCamera.SetActive(true);
            eventSystem.SetActive(true);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(shopPanel.GetComponent<RectTransform>());
        shopPanel.SetActive(true);
    }

    public void Start()
    {
        userName = PlayerPrefs.GetString("username");
        explorerBlueToggles[_explorerBlueId].isOn = true;
        explorerRedToggles[_explorerRedId].isOn = true;
        fighterBlueToggles[_fighterBlueId].isOn = true;
        fighterRedToggles[_fighterRedId].isOn = true;
        scholarBlueToggles[_scholarBlueId].isOn = true;
        scholarRedToggles[_scholarRedId].isOn = true;
    }

    public void ReturnToMenu()
    {
        if(!calledFromGame)
            SceneManager.LoadScene("Start After Login");
        else
        {
            SceneManager.UnloadSceneAsync("Preference Shop");
        }
    }

    public void Show()
    {
        Debug.Log("Check: " + _fighterBlueId + " " + fighterBlueToggles[_fighterBlueId].isOn);
    }
    
    public void OnConfirm()
    {
        for(int i = 0; i < explorerBlueToggles.Length; i ++)
            if (explorerBlueToggles[i].isOn)
            {
                _explorerBlueId = i;
                GridController.explorerBluePickedId = i;
                break;
            }
        for(int i = 0; i < explorerRedToggles.Length; i ++)
            if (explorerRedToggles[i].isOn)
            {
                _explorerRedId = i;
                GridController.explorerRedPickedId = i;
                break;
            }
        for(int i = 0; i < scholarBlueToggles.Length; i ++)
            if (scholarBlueToggles[i].isOn)
            {
                _scholarBlueId = i;
                GridController.scholarBluePickedId = i;
                break;
            }
        for(int i = 0; i < scholarRedToggles.Length; i ++)
            if (scholarRedToggles[i].isOn)
            {
                _scholarRedId = i;
                GridController.scholarRedPickedId = i;
                break;
            }
        for(int i = 0; i < fighterBlueToggles.Length; i ++)
            if (fighterBlueToggles[i].isOn)
            {
                _fighterBlueId = i;
                GridController.fighterBluePickedId = i;
                break;
            }
        for(int i = 0; i < fighterRedToggles.Length; i ++)
            if (fighterRedToggles[i].isOn)
            {
                _fighterRedId = i;
                GridController.fighterRedPickedId = i;
                break;
            }

        GridController.Instance.UpdateRenderManager(calledFromGame);
    }
}
