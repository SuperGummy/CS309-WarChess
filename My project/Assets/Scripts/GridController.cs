using System;
using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using Random = System.Random;

public class GridController : MonoBehaviour
{
    public static GridController Instance;
    private Grid grid;
    public bool gridEnable;
    [SerializeField] private Tilemap interactiveMap;
    [SerializeField] private Tilemap buildingMap;
    [SerializeField] private Tilemap movableMap;
    [SerializeField] private Tile hoverTile;
    [SerializeField] private Tile village1;
    [SerializeField] private Tile village2;
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
    [SerializeField] private GameObject characterHolder;
    [SerializeField] private GameObject characterPrefab;
    private Vector3Int previousMousePos;
    public const int MapSize = 17;
    private CharacterObject[] _characters = new CharacterObject[MapSize*MapSize];
    private GameObject[] characterObjects = new GameObject[MapSize*MapSize];
    
    [SerializeField] private CharacterRenderer explorerBlue;
    [SerializeField] private CharacterRenderer explorerRed;
    [SerializeField] private CharacterRenderer scholarBlue;
    [SerializeField] private CharacterRenderer scholarRed;
    [SerializeField] private CharacterRenderer fighterBlue;
    [SerializeField] private CharacterRenderer fighterRed;
    private void Awake()
    {
        gridEnable = true;
        Instance = this;
        RenderManager.Instance.explorerBlueController = explorerBlue.animator.runtimeAnimatorController;
        RenderManager.Instance.explorerRedController = explorerRed.animator.runtimeAnimatorController;
        RenderManager.Instance.scholarBlueController = scholarBlue.animator.runtimeAnimatorController;
        RenderManager.Instance.scholarRedController = scholarRed.animator.runtimeAnimatorController;
        RenderManager.Instance.fighterBlueController = fighterBlue.animator.runtimeAnimatorController;
        RenderManager.Instance.fighterRedController = fighterRed.animator.runtimeAnimatorController;
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
                interactiveMap.SetTile(previousMousePos, null); // Remove old hoverTile
                if (Math.Abs(mousePos.x) <= 8 && Math.Abs(mousePos.y) <= 8)
                {
                    interactiveMap.SetTile(mousePos, hoverTile);
                }

                previousMousePos = mousePos;
            }
        }
    }

    public void AddStructure(Vector3Int position, int type)
    {
        var random = new Random();
        if (type == 0)
        {
            buildingMap.SetTile(position, random.Next(2) == 0 ? village1 : village2);
        }
        else
        {
            buildingMap.SetTile(position, random.Next(2) == 0 ? relic1 : relic2);
        }
    }

    public void SetBlueCamp(Vector3Int position)
    {
        buildingMap.SetTile(position, blueCamp);
    }

    public void SetRedCamp(Vector3Int position)
    {
        buildingMap.SetTile(position, redCamp);
    }

    public void SetBlueMarket(Vector3Int position)
    {
        buildingMap.SetTile(position, blueMarket);
    }

    public void SetRedMarket(Vector3Int position)
    {
        buildingMap.SetTile(position, redMarket);
    }

    public void SetBlueInstitute(Vector3Int position)
    {
        buildingMap.SetTile(position, blueInstitute);
    }

    public void SetRedInstitute(Vector3Int position)
    {
        buildingMap.SetTile(position, redInstitute);
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

    public void CreateCharacter(Vector3Int position)
    {
        int arrayPosition = GetIndex(position);
        Vector3 characterMapPosition = grid.CellToWorld(new Vector3Int(position.x - 8,
            position.y - 8, position.z));
        characterMapPosition.y += 0.24f;
        GameObject _character = Instantiate(characterPrefab, characterMapPosition, Quaternion.identity);
        _character.transform.parent=characterHolder.transform;
        characterObjects[arrayPosition] = _character;

        CharacterObject character =  _character.GetComponent<CharacterObject>();
        character.grid = grid;
        Character characterInfo = DataManager.Instance.GetCharacterByPosition(position);
        // TODO: instantiate object and concrete by characterInfo
        String side = DataManager.Instance.CheckCharacterSide(characterInfo) == -1 ? "blue" : "red";
        character.Concrete(characterInfo.characterClass, side);
        _characters[arrayPosition] = character;
    }
}

