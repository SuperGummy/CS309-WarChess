using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CampLevelUpPanel : MonoBehaviour
{
    private int campLevel;
    public TextMeshProUGUI HealthIncreaseCurrentLevelText, StrengthIncreaseCurrentLevelText;
    public TextMeshProUGUI HealthIncreaseNextLevelText, StrengthIncreaseNextLevelText;

    public int healthIncreaseCurrentLevelValue, strengthIncreaseCurrentLevelValue;
    public int healthIncreaseNextLevelValue, strengthIncreaseNextLevelValue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetInfo(int campLevel, int healthIncrease, int strengthIncrease)
    {
        this.campLevel = campLevel;
        // Query next level infomation
        HealthIncreaseCurrentLevelText.text = "+" + healthIncrease;
        StrengthIncreaseCurrentLevelText.text = "+" + strengthIncrease;
        HealthIncreaseNextLevelText.text = "+" + healthIncreaseNextLevelValue;
        StrengthIncreaseNextLevelText.text = "+" + strengthIncreaseNextLevelValue;
    }
}
