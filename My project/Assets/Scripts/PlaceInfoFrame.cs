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
    public Button addHealth;
    public Button recruit;
    public Button function;
    public Slider hp;
    private Vector3Int _position;

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

    public void Inform(Vector3Int position)
    {
        _position = position;
        recruit.onClick.AddListener(GameManager.Instance.OpenRecruit);
        addHealth.onClick.AddListener(GameManager.Instance.HealStructure);
    }

    public void RenderData(Vector3Int position)
    {
        var structure = DataManager.Instance.GetStructureByPosition(position);
        structureName.text = structure.structureClass.ToString();
        hp.value = structure.hp;
        string side;
        if (DataManager.Instance.CheckStructureSide(structure) == -1)
            side = "blue";
        else if (DataManager.Instance.CheckStructureSide(structure) == 1)
            side = "red";
        else
            side = "middle";
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
                function.GetComponentInChildren<TextMeshProUGUI>().text = "train";
                function.onClick.AddListener(GameManager.Instance.OpenCamp);
                break;
            case StructureClass.MARKET:
                function.GetComponentInChildren<TextMeshProUGUI>().text = "earn";
                break;
            case StructureClass.INSTITUTE:
                function.GetComponentInChildren<TextMeshProUGUI>().text = "skill";
                function.onClick.AddListener(GameManager.Instance.OpenTechnologies);
                break;
            case StructureClass.VILLAGE:
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
        }
    }
}