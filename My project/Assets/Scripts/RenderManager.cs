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
    
    [SerializeField] private Sprite villageImage;
    [SerializeField] private Sprite villageBlueImage;
    [SerializeField] private Sprite villageRedImage;
    [SerializeField] private Sprite campBlueImage;
    [SerializeField] private Sprite campRedImage;
    [SerializeField] private Sprite schoolBlueImage;
    [SerializeField] private Sprite schoolRedImage;
    [SerializeField] private Sprite marketBlueImage;
    [SerializeField] private Sprite marketRedImage;
    
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
        Instance.characterSpeed = 1.3f;
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
    
    public Sprite GetStructureImage(StructureClass type, String side)
    {
        return type switch
        {
            StructureClass.VILLAGE => side == "middle" ? villageImage : 
                side == "blue" ? villageBlueImage : villageRedImage,
            StructureClass.MARKET => side == "blue" ? marketBlueImage : marketRedImage,
            StructureClass.INSTITUTE => side == "blue" ? schoolBlueImage : schoolRedImage,
            StructureClass.CAMP => side == "blue" ? campBlueImage : campRedImage,
            _ => null
        };
    }

    public void PlayCharacterRoute(CharacterObject currentCharacter, List<Vector3Int> route)
    {
        route.RemoveAt(0);
        currentCharacter.SetDestinations(route);
        currentCharacter.SetSpeed(characterSpeed);
    }
}
