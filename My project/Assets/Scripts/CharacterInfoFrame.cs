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
    public Slider hp;
    public TextMeshProUGUI actionRange;
    public Image equipment;
    public Image mount;
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
        var character = DataManager.Instance.GetCharacterByPosition(position);
        characterName.text = character.name;
        type.text = character.characterClass.ToString();
        attack.text = character.attack.ToString();
        defense.text = character.defense.ToString();
        hp.value = character.hp;
        actionRange.text = character.actionRange.ToString();
        equipment.sprite = character.equipment == null
            ? null
            : RenderManager.Instance.GetEquipmentImage(character.equipment.equipmentClass);
        mount.sprite = character.mount == null
            ? null
            : RenderManager.Instance.GetMountImage(character.mount.mountClass);
        characterImage.sprite = RenderManager.Instance.GetCharacterImage(character.characterClass,
            character.player.id == DataManager.Instance.player1.id ? "blue" : "red");
    }
}