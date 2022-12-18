using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Audio;
using Model;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using Quaternion = UnityEngine.Quaternion;
using Random = System.Random;
using Vector3 = UnityEngine.Vector3;

public enum AnimatedTextType
{
    HEALTH,
    STRENGTH,
    DAMAGE,
    LEVEL_UP,
    CONQUER
}

public class GridController : MonoBehaviour
{
    public static GridController Instance;
    private Grid grid;
    public bool gridEnable;
    [SerializeField] private Tilemap interactiveMap;
    [SerializeField] private Tilemap buildingMap;
    [SerializeField] private Tilemap movableMap;
    [SerializeField] private Tilemap landMap;
    [SerializeField] private Tilemap colorLandMap;
    [SerializeField] private Tilemap landObjectMap;
    [SerializeField] private Tilemap mountainMap;
    [SerializeField] private Tilemap borderMap;
    [SerializeField] private Tile hoverTile;
    [SerializeField] private Tile blueLeftCastle;
    [SerializeField] private Tile redLeftCastle;
    [SerializeField] private Tile blueRightCastle;
    [SerializeField] private Tile redRightCastle;
    [SerializeField] private Tile village1;
    [SerializeField] private Tile village2;
    [SerializeField] private Tile blueVillage;
    [SerializeField] private Tile redVillage;
    [SerializeField] private Tile blueVillage2;
    [SerializeField] private Tile redVillage2;
    [SerializeField] private Tile relic1;
    [SerializeField] private Tile relic2;
    [SerializeField] private Tile blueCamp;
    [SerializeField] private Tile redCamp;
    [SerializeField] private Tile blueMarket;
    [SerializeField] private Tile redMarket;
    [SerializeField] private Tile blueInstitute;
    [SerializeField] private Tile redInstitute;
    [SerializeField] private Tile movableTile;
    [SerializeField] private Tile attackableTile;
    [SerializeField] private Tile normalLand;
    [SerializeField] private Tile greenLand;
    [SerializeField] private Tile lightGreenLand;
    [SerializeField] private Tile yellowLand;
    [SerializeField] private Tile greenTree;
    [SerializeField] private Tile greenTrees;
    [SerializeField] private Tile pinkTree;
    [SerializeField] private Tile pinkTrees;
    [SerializeField] private Tile redTree;
    [SerializeField] private Tile redTrees;
    [SerializeField] private Tile highMountain;
    [SerializeField] private Tile lowMountain;
    [SerializeField] private Tile emptyTile;
    [SerializeField] private Tile borderTile;
    [SerializeField] private GameObject characterHolder;
    [SerializeField] private GameObject characterPrefab;
    private Vector3Int previousMousePos;
    public const int MapSize = 17;
    private CharacterObject[] _characters = new CharacterObject[MapSize * MapSize];
    public GameObject[] characterObjects = new GameObject[MapSize * MapSize];

    public static int explorerBluePickedId, explorerRedPickedId;
    public static int scholarBluePickedId, scholarRedPickedId;
    public static int fighterBluePickedId, fighterRedPickedId;
    
    [SerializeField] private CharacterRenderer[] explorerBlue;
    [SerializeField] private CharacterRenderer[] explorerRed;
    [SerializeField] private CharacterRenderer[] scholarBlue;
    [SerializeField] private CharacterRenderer[] scholarRed;
    [SerializeField] private CharacterRenderer[] fighterBlue;
    [SerializeField] private CharacterRenderer[] fighterRed;
    [SerializeField] private GameObject animatedTextPrefab;
    [SerializeField] private GameObject textHolder;
    [SerializeField] private GameObject animatedMoneyPrefab;
    [SerializeField] private GameObject animatedMoneyHolder;

    [SerializeField] private float textShift;

    private void Awake()
    {
        Debug.Log("I woke up!");
        gridEnable = true;
        Instance = this;
        UpdateRenderManager(false);
    }

    public void RefreshGameCharacters()
    {
        int cnt = 0;
        for (int i = 0; i < MapSize; i++)
            for(int j = 0; j < MapSize; j ++)
            {
                if (characterObjects[cnt] != null)
                {
                    Vector3Int position = new Vector3Int(i, j, 0);
                    DeleteCharacter(position);
                    CreateCharacter(position);
                }

                cnt++;
            }
    }

    public void UpdateRenderManager(bool refreshGame)
    {
        RenderManager renderManager = RenderManager.Instance;
        renderManager.explorerBlueController = explorerBlue[explorerBluePickedId].animator.runtimeAnimatorController;
        renderManager.explorerBlueImage = explorerBlue[explorerBluePickedId].spriteRenderer.sprite;
        
        renderManager.explorerRedController = explorerRed[explorerRedPickedId].animator.runtimeAnimatorController;
        renderManager.explorerRedImage = explorerRed[explorerRedPickedId].spriteRenderer.sprite;
        
        renderManager.scholarBlueController = scholarBlue[scholarBluePickedId].animator.runtimeAnimatorController;
        renderManager.scholarBlueImage = scholarBlue[scholarBluePickedId].spriteRenderer.sprite;
        
        renderManager.scholarRedController = scholarRed[scholarRedPickedId].animator.runtimeAnimatorController;
        renderManager.scholarRedImage = scholarRed[scholarRedPickedId].spriteRenderer.sprite;
        
        renderManager.fighterBlueController = fighterBlue[fighterBluePickedId].animator.runtimeAnimatorController;
        renderManager.fighterBlueImage = fighterBlue[fighterBluePickedId].spriteRenderer.sprite;
        
        renderManager.fighterRedController = fighterRed[fighterRedPickedId].animator.runtimeAnimatorController;
        renderManager.fighterRedImage = fighterRed[fighterRedPickedId].spriteRenderer.sprite;

        if (refreshGame)
        {
            RefreshGameCharacters();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        grid = gameObject.GetComponent<Grid>();
    }

    public CharacterObject GetCharacter(Vector3Int position)
    {
        return _characters[GetIndex(position)];
    }

    // Update is called once per frame
    void Update()
    {
        // Mouse over -> highlight tile
        if (gridEnable)
        {
            Vector3Int mousePos = GetMousePosition();
            if (!mousePos.Equals(previousMousePos))
            {
                AudioManager.Instance.Play(4);
                interactiveMap.SetTile(previousMousePos, null); // Remove old hoverTile
                if (Math.Abs(mousePos.x) <= 8 && Math.Abs(mousePos.y) <= 8)
                {
                    interactiveMap.SetTile(mousePos, hoverTile);
                }

                previousMousePos = mousePos;
            }
        }
    }

    public void AddVillageAndRelic(Vector3Int position, int type)
    {
        var random = new Random();
        if (type == 0)
            buildingMap.SetTile(position, random.Next(2) == 0 ? village1 : village2);
        else
            buildingMap.SetTile(position, random.Next(2) == 0 ? relic1 : relic2);
    }

    public void SetMap(Vector3Int position, int value,int landColor=0,int treeColor=0)
    {
        var random = new Random();
        position.x -= 8;
        position.y -= 8;
        landMap.SetTile(position,emptyTile);
        colorLandMap.SetTile(position,emptyTile);
        landObjectMap.SetTile(position,emptyTile);
        mountainMap.SetTile(position,emptyTile);
        borderMap.SetTile(position,borderTile);
        switch (value)
        {
            case 0:
                landMap.SetTile(position, normalLand);
                switch (landColor)
                {
                    case 0:
                        break;
                    case 1:
                        colorLandMap.SetTile(position,greenLand);
                        break;
                    case 2:
                        colorLandMap.SetTile(position,lightGreenLand);
                        break;
                    case 3:
                        colorLandMap.SetTile(position,yellowLand);
                        break;
                }
                break;
            case 1:
                landMap.SetTile(position, normalLand);
                switch (landColor)
                {
                    case 0:
                        break;
                    case 1:
                        colorLandMap.SetTile(position,greenLand);
                        break;
                    case 2:
                        colorLandMap.SetTile(position,lightGreenLand);
                        break;
                    case 3:
                        colorLandMap.SetTile(position,yellowLand);
                        break;
                }
                mountainMap.SetTile(position,random.Next(2)==0 ? lowMountain : highMountain);
                break;
            case 2:
                borderMap.SetTile(position,emptyTile);
                break;
            case 3:
                landMap.SetTile(position, normalLand);
                switch (landColor)
                {
                    case 0:
                        break;
                    case 1:
                        colorLandMap.SetTile(position,greenLand);
                        break;
                    case 2:
                        colorLandMap.SetTile(position,lightGreenLand);
                        break;
                    case 3:
                        colorLandMap.SetTile(position, yellowLand);
                        break;
                }
                
                switch (treeColor)
                {
                    case 0:
                        landObjectMap.SetTile(position,random.Next(2)==0?greenTree:greenTrees);
                        break;
                    case 1:
                        landObjectMap.SetTile(position,random.Next(2)==0?pinkTree:pinkTrees);
                        break;
                    case 2:
                        landObjectMap.SetTile(position,random.Next(2)==0?redTree:redTrees);
                        break;
                }
                break;
            default:
                break;
        }
    }
    public void SetStructure(Vector3Int position)
    {
        var structure = DataManager.Instance.GetStructureByPosition(position);
        if (structure == null) return;
        string side;
        if (DataManager.Instance.CheckStructureSide(structure) == -1)
            side = "blue";
        else if (DataManager.Instance.CheckStructureSide(structure) == 1)
            side = "red";
        else
            side = "middle";
        var newPosition = new Vector3Int(position.x - 8, position.y - 8, 0);
        switch (structure.structureClass)
        {
            case StructureClass.CAMP:
                buildingMap.SetTile(newPosition, side == "blue" ? blueCamp : redCamp);
                break;
            case StructureClass.MARKET:
                buildingMap.SetTile(newPosition, side == "blue" ? blueMarket : redMarket);
                break;
            case StructureClass.INSTITUTE:
                buildingMap.SetTile(newPosition, side == "blue" ? blueInstitute : redInstitute);
                break;
            case StructureClass.VILLAGE:
                if(buildingMap.GetTile(newPosition) == village1)
                    buildingMap.SetTile(newPosition, side == "blue" ? blueVillage : redVillage);
                else buildingMap.SetTile(newPosition, side == "blue" ? blueVillage2 : redVillage2);
                break;
            default:
                Tile tile = null;
                if (position.x == 0 && position.y == DataManager.MapSize - 1)
                    tile = side == "blue" ? blueLeftCastle : redLeftCastle;
                if (position.x == DataManager.MapSize - 1 && position.y == 0)
                    tile = side == "blue" ? blueRightCastle : redRightCastle;
                buildingMap.SetTile(newPosition, tile);
                break;
        }
    }

    Vector3Int GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
        Vector3Int position = grid.WorldToCell(worldPoint);
        return position;
    }

    public void SetMovableHighlight(List<Vector3Int> movableList, bool draw)
    {
        foreach (var position in movableList)
        {
            movableMap.SetTile(new Vector3Int(position.x - 8, position.y - 8, position.z),
                draw ? movableTile : null);
        }
    }

    public void SetAttackableHighlight(List<Vector3Int> attackableList, bool draw)
    {
        foreach (var position in attackableList)
        {
            movableMap.SetTile(new Vector3Int(position.x - 8, position.y - 8, position.z)
                , draw ? attackableTile : null);
        }
    }

    public int GetIndex(Vector3Int position)
    {
        return position.x * MapSize + position.y;
    }

    public void DeleteCharacter(Vector3Int position)
    {
        int arrayPosition = GetIndex(position);
        _characters[arrayPosition] = null;
        Destroy(characterObjects[arrayPosition]);
        characterObjects[arrayPosition] = null;
    }

    public void PlayCharacterRoute(List<Vector3Int> route)
    {
        int startPosition = GetIndex(route[0]);
        int endPosition = GetIndex(route[^1]);
        _characters[endPosition] = _characters[startPosition];
        _characters[startPosition] = null;
        characterObjects[endPosition] = characterObjects[startPosition];
        characterObjects[startPosition] = null;
        RenderManager.Instance.PlayCharacterRoute(_characters[endPosition], route);
    }

    public Vector3 GetMapPosition(Vector3Int position)
    {
        Vector3 cellToWorld = grid.CellToWorld(new Vector3Int(position.x - 8,
            position.y - 8, position.z));
        cellToWorld.y += 0.24f;
        return cellToWorld;
    }

    public void ShowDamageText(Vector3Int position, int damage)
    {
        var mapPosition = GetMapPosition(position);
        mapPosition.y += textShift;
        var stringBuilder = "-" + damage;
        CreateText(mapPosition, stringBuilder, AnimatedTextType.DAMAGE);
    }

    public void ShowAddHealthText(Vector3Int position, int health)
    {
        var mapPosition = GetMapPosition(position);
        mapPosition.y += textShift;
        var stringBuilder = "+" + health;
        CreateText(mapPosition, stringBuilder, AnimatedTextType.HEALTH);
    }

    public void ShowCharacterAddStrengthText(Vector3Int position, int strength)
    {
        var mapPosition = GetMapPosition(position);
        mapPosition.y += textShift;
        var stringBuilder = "+" + strength;
        CreateText(mapPosition, stringBuilder, AnimatedTextType.STRENGTH);
    }

    public void ShowConquerText(Vector3Int position)
    {
        var mapPosition = GetMapPosition(position);
        mapPosition.y += textShift;
        var stringBuilder = "CONQUERED";
        CreateText(mapPosition, stringBuilder, AnimatedTextType.CONQUER);
    }

    public void CreateCharacter(Vector3Int position)
    {
        int arrayPosition = GetIndex(position);
        Vector3 characterMapPosition = GetMapPosition(position);
        GameObject _character = Instantiate(characterPrefab, characterMapPosition, Quaternion.identity);
        _character.transform.parent = characterHolder.transform;
        characterObjects[arrayPosition] = _character;

        CharacterObject character = _character.GetComponent<CharacterObject>();
        character.grid = grid;
        Character characterInfo = DataManager.Instance.GetCharacterByPosition(position);
        // TODO: instantiate object and concrete by characterInfo
        String side = DataManager.Instance.CheckCharacterSide(characterInfo) == -1 ? "blue" : "red";
        character.Concrete(characterInfo.characterClass, side);
        //character.Concrete(CharacterClass.SCHOLAR, "blue");
        _characters[arrayPosition] = character;
    }

    public void CreateText(Vector3 position, String text, AnimatedTextType type)
    {
        var animatedTextObject = Instantiate(animatedTextPrefab, position, Quaternion.identity);
        animatedTextObject.transform.parent = textHolder.transform;
        TextMeshPro animatedText = animatedTextObject.GetComponent<TextMeshPro>();
        animatedText.text = text;
        switch (type)
        {
            case AnimatedTextType.STRENGTH:
                animatedText.color = Color.blue;
                break;
            case AnimatedTextType.HEALTH:
                animatedText.color = Color.green;
                break;
            case AnimatedTextType.LEVEL_UP:
                animatedText.color = Color.yellow;
                break;
            case AnimatedTextType.CONQUER:
                animatedText.fontSize = 3.2f;
                animatedText.color = Color.yellow;
                break;
            case AnimatedTextType.DAMAGE:
                animatedText.color = Color.red;
                break;
        }
    }
    
    public void CreateMoneyAnimation(Vector3 position)
    {
        var animatedObject = Instantiate(animatedMoneyPrefab, position, Quaternion.identity);
        animatedObject.transform.parent = animatedMoneyHolder.transform;
    }
}