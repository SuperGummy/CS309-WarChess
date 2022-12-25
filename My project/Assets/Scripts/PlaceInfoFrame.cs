using System;
using System.Collections;
using System.Collections.Generic;
using Model;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlaceInfoFrame : MonoBehaviour
{
    public Image structureImage;
    public TextMeshProUGUI structureName;
    public TextMeshProUGUI structureInfo;
    public Button addHealth;
    public Button recruit;
    public Button function;
    public Slider hp;
    private Vector3Int _position;
    public GameObject levelUpPanel;
    public GameObject levelUpButton;

    private void OnEnable()
    {
        RenderData(_position);
    }

    private void OnDisable()
    {
        function.onClick.RemoveAllListeners();
        addHealth.onClick.RemoveAllListeners();
        recruit.onClick.RemoveAllListeners();
        
    }

    public void OnClickLevelUp()
    {
        StructureLevelUpPanel.position = _position;
        levelUpPanel.SetActive(true);
    }

    public void Inform(Vector3Int position)
    {
        _position = position;
        recruit.onClick.AddListener(GameManager.Instance.OpenRecruit);
        addHealth.onClick.AddListener(GameManager.Instance.HealStructure);
    }

    public void RenderData(Vector3Int position)
    {
        var structure = DataManager.Instance.GetStructureByPosition(position);
        var character = DataManager.Instance.GetCharacterByPosition(position);
        structureName.text = structure.structureClass.ToString();
        hp.value = structure.hp;
        string side;
        if (DataManager.Instance.CheckStructureSide(structure) == -1)
            side = "blue";
        else if (DataManager.Instance.CheckStructureSide(structure) == 1)
            side = "red";
        else
            side = "middle";
        if(structure.structureClass == StructureClass.MARKET || structure.structureClass == StructureClass.CAMP)
            levelUpButton.SetActive(true);
        structureImage.sprite = RenderManager.Instance.GetStructureImage(structure.structureClass, side);
        recruit.interactable = true;
        function.interactable = true;
        function.onClick.RemoveAllListeners();
        if (DataManager.Instance.currentPlayer.stars >= 50 - structure.hp)
            addHealth.interactable = true;
        else
            addHealth.interactable = false;
        switch (structure.structureClass)
        {
            case StructureClass.CAMP:
                structureInfo.text = "This is a camp. You can train your people here.";
                function.GetComponentInChildren<TextMeshProUGUI>().text = "train";
                function.onClick.AddListener(GameManager.Instance.OpenCamp);
                break;
            case StructureClass.MARKET:
                structureInfo.text = "This is a market. You can earn stars here.";
                function.GetComponentInChildren<TextMeshProUGUI>().text = "earn";
                function.onClick.AddListener(GameManager.Instance.EarnStars);
                break;
            case StructureClass.INSTITUTE:
                structureInfo.text = "This is an institute. You can send scholars to learn new skills here.";
                function.GetComponentInChildren<TextMeshProUGUI>().text = "skill";
                if (character.characterClass == CharacterClass.SCHOLAR)
                {
                    function.interactable = false;
                    function.onClick.AddListener(GameManager.Instance.OpenTechnologies);
                }
                break;
            case StructureClass.VILLAGE:
                structureInfo.text = "This is a village. Upgrade it to advanced buildings.";
                if (side == "middle")
                {
                    function.GetComponentInChildren<TextMeshProUGUI>().text = "waiting...";
                    function.interactable = false;
                    recruit.interactable = false;
                    addHealth.interactable = false;
                }
                else
                {
                    function.GetComponentInChildren<TextMeshProUGUI>().text = "evolve";
                    function.onClick.AddListener(GameManager.Instance.OpenUpgrade);
                }
                break;
            default:
                structureInfo.text = "This is the castle. Protect it until the last soldier falls.";
                function.GetComponentInChildren<TextMeshProUGUI>().text = "nothing";
                function.interactable = false;
                break;
        }
    }
}