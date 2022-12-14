using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Model;

public enum StatType{
    HEALTH,
    STRENGTH
}

public class RenderManager : MonoBehaviour
{
    public static RenderManager Instance;
    [SerializeField] public Sprite scholarBlueImage;
    [SerializeField] public Sprite scholarRedImage;
    [SerializeField] public Sprite explorerBlueImage;
    [SerializeField] public Sprite explorerRedImage;
    [SerializeField] public Sprite fighterBlueImage;
    [SerializeField] public Sprite fighterRedImage;

    [SerializeField] private Sprite foxMount;
    [SerializeField] private Sprite elephantMount;
    [SerializeField] private Sprite horseMount;
    [SerializeField] private Sprite rabbitMount;

    [SerializeField] private Sprite potionProperty;
    [SerializeField] private Sprite beerProperty;
    [SerializeField] private Sprite fishProperty;
    [SerializeField] private Sprite appleProperty;

    [SerializeField] private Sprite arrowEquipment;
    [SerializeField] private Sprite cannonEquipment;
    [SerializeField] private Sprite swordEquipment;
    [SerializeField] private Sprite shieldEquipment;
    [SerializeField] private Sprite stickEquipment;
    
    [SerializeField] private Sprite castleBlueImage;
    [SerializeField] private Sprite castleRedImage;
    [SerializeField] private Sprite villageImage;
    [SerializeField] private Sprite villageBlueImage;
    [SerializeField] private Sprite villageRedImage;
    [SerializeField] private Sprite campBlueImage;
    [SerializeField] private Sprite campRedImage;
    [SerializeField] private Sprite schoolBlueImage;
    [SerializeField] private Sprite schoolRedImage;
    [SerializeField] private Sprite marketBlueImage;
    [SerializeField] private Sprite marketRedImage;
    
    [SerializeField] private Sprite healthIcon;
    [SerializeField] private Sprite strengthIcon;
    [SerializeField] private Sprite trans32;
    [SerializeField] private Sprite trans144;
    [SerializeField] private Sprite empty32;
    
    public RuntimeAnimatorController scholarBlueController;
    public RuntimeAnimatorController scholarRedController;
    public RuntimeAnimatorController explorerBlueController;
    public RuntimeAnimatorController explorerRedController;
    public RuntimeAnimatorController fighterBlueController;
    public RuntimeAnimatorController fighterRedController;

    [SerializeField] private float characterSpeed;
    
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
    
    public RuntimeAnimatorController GetCharacterController(CharacterClass type, String side)
    {
        return type switch
        {
            CharacterClass.SCHOLAR => side == "blue" ? scholarBlueController : scholarRedController,
            CharacterClass.EXPLORER => side == "blue" ? explorerBlueController : explorerRedController,
            CharacterClass.WARRIOR => side == "blue" ? fighterBlueController : fighterRedController,
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
            ItemClass.BASIC => appleProperty,
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
            MountClass.BASIC => rabbitMount,
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
            EquipmentClass.BASIC => stickEquipment,
            _ => null
        };
    }
    
    public Sprite GetStatIcon(StatType statType)
    {
        return statType switch
        {
            StatType.HEALTH => healthIcon,
            StatType.STRENGTH => strengthIcon,
            _ => null
        };
    }
    
    public Sprite GetStructureImage(StructureClass type, String side)
    {
        return type switch
        {
            StructureClass.VILLAGE => side == "middle" ? villageImage : 
                side == "blue" ? villageBlueImage : villageRedImage,
            StructureClass.MARKET => side == "blue" ? marketBlueImage : marketRedImage,
            StructureClass.INSTITUTE => side == "blue" ? schoolBlueImage : schoolRedImage,
            StructureClass.CAMP => side == "blue" ? campBlueImage : campRedImage,
            _ => side == "blue" ? castleBlueImage : castleRedImage
        };
    }

    public void PlayCharacterRoute(CharacterObject currentCharacter, List<Vector3Int> route)
    {
        route.RemoveAt(0);
        currentCharacter.SetDestinations(route);
        currentCharacter.SetSpeed(characterSpeed);
    }
    
    public Sprite GetTrans32()
    {
        return trans32;
    }
    
    public Sprite GetTrans144()
    {
        return trans144;
    }
    
    public Sprite GetEmpty32()
    {
        return empty32;
    }
}
