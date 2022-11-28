using System;
using System.Collections;
using System.Collections.Generic;
using Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlaceInfoFrame : MonoBehaviour
{
    public Image structureImage;
    public TextMeshProUGUI structureName;
    public Button function;
    public Slider hp;
    private Vector3Int _position;

    private void OnEnable()
    {
        RenderData(_position);
    }

    public void Inform(Vector3Int position)
    {
        _position = position;
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
        function.interactable = true;
        switch (structure.structureClass)
        {
            case StructureClass.CAMP:
                function.GetComponent<TextMeshProUGUI>().text= "train";
                function.onClick.AddListener(GameManager.Instance.OpenCamp);
                break;
            case StructureClass.MARKET:
                function.GetComponent<TextMeshProUGUI>().text = "shop";
                function.onClick.AddListener(GameManager.Instance.OpenShop);
                break;
            case StructureClass.INSTITUTE:
                function.GetComponent<TextMeshProUGUI>().text = "Tech";
                function.onClick.AddListener(GameManager.Instance.OpenTechnologies);
                break;
            case StructureClass.VILLAGE:
                function.interactable = false;
                break;
        }
    }
}