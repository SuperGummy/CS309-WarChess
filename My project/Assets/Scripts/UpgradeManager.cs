using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager: MonoBehaviour
{
    public Button[] upgradeBtn;
    public GameObject upgradeFrame;
    public Button close;

    public void Start()
    {
        upgradeFrame.SetActive(true);
        foreach (Button btn in upgradeBtn)
        {
            btn.onClick.AddListener(delegate { OnClickUpgrade(btn); });
        }
    }

    public void OnClickUpgrade(Button btn)
    {
        int type = -1;
        switch (btn.name)
        {
            case "UpgradeCollege":
                type = 2;
                break;
            case "UpgradeCamp":
                type = 0;
                break;
            case "UpgradeMarket":
                type = 1;
                break;
            default:
                throw new Exception("Cannot find " + btn.name);
        }
        
        upgradeFrame.SetActive(false);
        
        GameManager.Instance.UpgradeStructure(type);
    }

    public void OnClickClose()
    {
        // GameManager.Instance.close
    }
    
}
