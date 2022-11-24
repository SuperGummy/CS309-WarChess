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
    
    [SerializeField] private Animator scholarBlueAnimator;
    [SerializeField] private Animator scholarRedAnimator;
    [SerializeField] private Animator explorerBlueAnimator;
    [SerializeField] private Animator explorerRedAnimator;
    [SerializeField] private Animator fighterBlueAnimator;
    [SerializeField] private Animator fighterRedAnimator;

    [SerializeField] private float characterSpeed;
    [SerializeField] private CharacterObject currentCharacter;
    
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
    
    public Animator GetCharacterAnimator(CharacterClass type, String side)
    {
        return type switch
        {
            CharacterClass.SCHOLAR => side == "blue" ? scholarBlueAnimator : scholarRedAnimator,
            CharacterClass.EXPLORER => side == "blue" ? explorerBlueAnimator : explorerRedAnimator,
            CharacterClass.WARRIOR => side == "blue" ? fighterBlueAnimator : fighterRedAnimator,
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

    public void PlayCharacterRoute(List<Vector3Int> route)
    {
        currentCharacter.SetDestinations(route);
        currentCharacter.SetSpeed(characterSpeed);
    }
    
    //public void 
}
