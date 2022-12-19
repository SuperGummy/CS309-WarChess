using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    [SerializeField] private TextMeshProUGUI welcomeText;
    
    public string userName;

    
    // Start is called before the first frame update
    void Start()
    {
        userName = PlayerPrefs.GetString("username", "123");
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
    
    private async Task Initiate()
    {
        SceneManager.LoadScene("Loading", LoadSceneMode.Additive);
        Progress<ProgressReportModel> progress = new Progress<ProgressReportModel>();
        progress.ProgressChanged += ReportProgress;
        await DataManager.Instance.Play(userName, null, progress);
        var pos1 = new Vector3Int(0, 16, 0);
        var pos2 = new Vector3Int(16, 0, 0);
        GridController.Instance.CreateCharacter(pos1);
        GridController.Instance.CreateCharacter(pos2);
    }
    
    private void ReportProgress(object sender, ProgressReportModel report)
    {
        ProgressRenderer.Instance.SetSliderValue(report.ProgressValue);
    }
    
    public async void LoadArchive()
    {
        SceneManager.LoadScene("Loading", LoadSceneMode.Additive);
        Progress<ProgressReportModel> progress = new Progress<ProgressReportModel>();
        progress.ProgressChanged += ReportProgress;
        await DataManager.Instance.LoadArchive();
        for (int i = 0; i < DataManager.MapSize; i++)
        {
            for (int j = 0; j < DataManager.MapSize; j++)
            {
                if (GridController.Instance.characterObjects[i * DataManager.MapSize + j] != null)
                    GridController.Instance.DeleteCharacter(new Vector3Int(i, j));
                GridController.Instance.SetStructure(new Vector3Int(i, j));
                if (DataManager.Instance.GetCharacterByPosition(new Vector3Int(i, j)) != null)
                {
                    GridController.Instance.CreateCharacter(new Vector3Int(i, j));
                }
            }
        }
    }
}