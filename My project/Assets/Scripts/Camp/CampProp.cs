using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CampProp : MonoBehaviour
{
    public TextMeshProUGUI trainingRoundsText;
    public TextMeshProUGUI HealthIncreaseText;
    public TextMeshProUGUI StrengthIncreaseText;
    public TextMeshProUGUI LevelText;
    public GameObject levelUpPanel;
    
    private int currentx, currenty;

    public int campLevel, healthIncreaseValue, strengthIcreaseValue;
    private int trainingRounds = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Inform(int x, int y)
    {
        currentx = x;
        currenty = y;
        UpdateInfo();
    }

    void GetInfo()
    {
        // TODO: get current camp level, etc
    }

    public void UpdateInfo()
    {
        GetInfo();
        HealthIncreaseText.text = "+" + healthIncreaseValue;
        StrengthIncreaseText.text = "+" + strengthIcreaseValue;
        trainingRoundsText.text = "Train for " + trainingRounds + " rounds: ";
        LevelText.text = "Place for training your characters. \n Current camp level: " + campLevel;
        levelUpPanel.GetComponent<CampLevelUpPanel>().SetInfo(campLevel, healthIncreaseValue, strengthIcreaseValue);
    }

    private void OnEnable()
    {
        UpdateInfo();
    }
}
