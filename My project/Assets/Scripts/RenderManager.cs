using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RenderManager : MonoBehaviour
{
    public static RenderManager renderManager;
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

    public Sprite GetCharacterImage(Model.CharacterClass type, String side)
    {
        return type switch
        {
            Model.CharacterClass.SCHOLAR => side == "blue" ? scholarBlueImage : scholarRedImage,
            Model.CharacterClass.EXPLORER => side == "blue" ? explorerBlueImage : explorerRedImage,
            Model.CharacterClass.WARRIOR => side == "blue" ? fighterBlueImage : fighterRedImage,
            _ => null
        };
    }

    public Sprite GetBackPackImage(String objectName)
    {
        return objectName switch
        {
            "fox" => foxMount,
            "horse" => horseMount,
            "elephant" => elephantMount,
            "arrow" => arrowEquipment,
            "sword" => swordEquipment,
            "shield" => shieldEquipment,
            "cannon" => cannonEquipment,
            "potion" => potionProperty,
            "fish" => fishProperty,
            "beer" => beerProperty,
            _ => null
        };
    }
}
