using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class StructureLevelUpPanel : MonoBehaviour
{
    public static Vector3Int position;

    [SerializeField] private GameObject structureLevelUpPanel;

    [SerializeField] private TextMeshProUGUI promptText;
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
        int star = DataManager.Instance.GetStructureByPosition(position).level * 10;
        promptText.text = "would you like to level up your structure by " + star + " stars?";
    }

    public void OnClickLevelUp()
    {
        GameManager.Instance.UpgradeStructure();
        OnClickCancel();
    }

    public void OnClickCancel()
    {
        structureLevelUpPanel.SetActive(false);
    }
}
