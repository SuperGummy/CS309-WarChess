using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Model;

public class RenderManager : MonoBehaviour
{
    public static RenderManager Instance;
    [SerializeField] private Sprite scholarBlueImage;
    [SerializeField] private Sprite scholarRedImage;
    [SerializeField] private Sprite explorerBlueImage;
    [SerializeField] private Sprite explorerRedImage;
    [SerializeField] private Sprite fighterBlueImage;
    [SerializeField] private Sprite fighterRedImage;

    [SerializeField] private Sprite foxMount;
    [SerializeField] private Sprite elephantMount;
    [SerializeField] private Sprite horseMount;

    [SerializeField] private Sprite potionProperty;
    [SerializeField] private Sprite beerProperty;
    [SerializeField] private Sprite fishProperty;

    [SerializeField] private Sprite arrowEquipment;
    [SerializeField] private Sprite cannonEquipment;
    [SerializeField] private Sprite swordEquipment;
    [SerializeField] private Sprite shieldEquipment;

    private void Awake()
    {
        Instance = this;
    }

    public Sprite GetCharacterImage(CharacterClass type, String side)
    {
        return type switch
        {
            CharacterClass.SCHOLAR => side == "blue" ? scholarBlueImage : scholarRedImage,
            CharacterClass.EXPLORER => side == "blue" ? explorerBlueImage : explorerRedImage,
            CharacterClass.WARRIOR => side == "blue" ? fighterBlueImage : fighterRedImage,
            _ => null
        };
    }

    public Sprite GetItemImage(ItemClass type)
    {
        return type switch
        {
            ItemClass.BEER => beerProperty,
            ItemClass.POTION => potionProperty,
            ItemClass.FISH => fishProperty,
            _ => null
        };
    }

    public Sprite GetMountImage(MountClass type)
    {
        return type switch
        {
            MountClass.FOX => foxMount,
            MountClass.HORSE => horseMount,
            MountClass.ELEPHANT => elephantMount,
            _ => null
        };
    }

    public Sprite GetEquipmentImage(EquipmentClass type)
    {
        return type switch
        {
            EquipmentClass.ARROW => arrowEquipment,
            EquipmentClass.SWORD => swordEquipment,
            EquipmentClass.SHIELD => shieldEquipment,
            EquipmentClass.CANNON => cannonEquipment,
            _ => null
        };
    }
}
