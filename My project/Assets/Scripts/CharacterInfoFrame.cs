using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoFrame : MonoBehaviour
{
    public Image characterImage;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI type;
    public TextMeshProUGUI attack;
    public TextMeshProUGUI defense;
    public TextMeshProUGUI hp;
    public TextMeshProUGUI actionRange;
    public Image equipment;
    public Image mount;
    public Button equip;
    public Button discharge;
    private Vector3Int _position;

    private void OnEnable()
    {
        RenderData(_position);
        equip.onClick.AddListener(GameManager.Instance.OpenEquip);
    }

    public void Inform(Vector3Int position)
    {
        _position = position;
    }

    public void RenderData(Vector3Int position)
    {
        var character = DataManager.Instance.GetCharacterByPosition(position);
        characterName.text = character.name;
        type.text = character.characterClass.ToString();
        attack.text = character.attack.ToString();
        defense.text = character.defense.ToString();
        hp.text = character.hp.ToString();
        actionRange.text = character.actionRange.ToString();
        equipment.sprite = character.equipment == null
            ? RenderManager.Instance.GetEmpty32()
            : RenderManager.Instance.GetEquipmentImage(character.equipment.equipmentClass);
        mount.sprite = character.mount == null
            ? RenderManager.Instance.GetEmpty32()
            : RenderManager.Instance.GetMountImage(character.mount.mountClass);
        characterImage.sprite = RenderManager.Instance.GetCharacterImage(character.characterClass,
            character.player.id == DataManager.Instance.player1.id ? "blue" : "red");
    }
}