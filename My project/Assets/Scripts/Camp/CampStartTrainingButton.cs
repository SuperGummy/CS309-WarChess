using System;
using System.Collections;
using System.Collections.Generic;
using Camp;
using Model;
using TMPro;
using UnityEngine;

public class CampStartTrainingButton : MonoBehaviour
{
    public int updateType;

    [SerializeField] private TextMeshProUGUI promptText;

    [SerializeField] private CampProp campProp;

    [SerializeField] private CampManager campManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        if (updateType == 0)
            promptText.text = campProp.healthIncreaseText.text;
        else
            promptText.text = campProp.strengthIncreaseText.text;
    }

    public void OnClick()
    {
        GameManager.Instance.UpdateCharacterAtCamp(updateType);
        campManager.UpdateInfo();
    }
}
