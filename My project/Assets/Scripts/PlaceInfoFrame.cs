using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlaceInfoFrame : MonoBehaviour
{
    public Image structureImage;
    public TextMeshProUGUI structureName;
    public Slider hp;
    private Vector3Int _position;

    public void Inform(Vector3Int position)
    {
        _position = position;
    }

    public void RenderData(Vector3Int position)
    {
        var structure = DataManager.Instance.GetStructureByPosition(position);
        structureName.text = structure.structureClass.ToString();
        hp.value = structure.hp;
    }
}