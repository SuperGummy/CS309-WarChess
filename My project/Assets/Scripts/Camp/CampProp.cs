using System;
using System.Collections;
using System.Collections.Generic;
using Camp;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CampProp : MonoBehaviour
{
    public TextMeshProUGUI trainingRoundsText;
    public TextMeshProUGUI healthIncreaseText;
    public TextMeshProUGUI strengthIncreaseText;
    public TextMeshProUGUI levelText;
    public GameObject levelUpPanel;
    [SerializeField] private int trainingRounds;
    [SerializeField] private Structure structureInfo;
    [SerializeField] private TextMeshProUGUI levelUpText;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateInfo(Vector3Int position)
    {
        structureInfo = DataManager.Instance.GetStructureByPosition(position);
        healthIncreaseText.text = "+" + structureInfo.level * 2;
        strengthIncreaseText.text = "+" + structureInfo.level * 2;
        trainingRoundsText.text = "Train for " + trainingRounds + " rounds: ";
        levelText.text = "Place for training your characters. \n Current camp level: " + structureInfo.level;
        levelUpPanel.GetComponent<CampLevelUpPanel>().SetInfo(
            structureInfo.level * 2, structureInfo.level * 2);
        levelUpText.text = structureInfo.level * 10 + "$";
    }
}
