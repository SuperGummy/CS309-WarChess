using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class GridController : MonoBehaviour
{
    public static GridController Instance;
    private Grid grid;
    public bool gridEnable;
    [SerializeField] private Tilemap interactiveMap;
    [SerializeField] private Tilemap buildingMap;
    [SerializeField] private Tile hoverTile;
    [SerializeField] private Tile village;
    [SerializeField] private Tile relic;
    [SerializeField] private Tile blueCamp;
    [SerializeField] private Tile redCamp;
    [SerializeField] private Tile blueMarket;
    [SerializeField] private Tile redMarket;
    [SerializeField] private Tile blueInstitute;
    [SerializeField] private Tile redInstitute;
    private Vector3Int previousMousePos;


    private void Awake()
    {
        gridEnable = true;
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        grid = gameObject.GetComponent<Grid>();
    }
    
    // Update is called once per frame
    void Update() {
        // Mouse over -> highlight tile
        if (gridEnable)
        {
            Vector3Int mousePos = GetMousePosition();
            if (!mousePos.Equals(previousMousePos)) {
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
        if (type == 0)
        {
            buildingMap.SetTile(position, village);
        }
        else
        {
            buildingMap.SetTile(position, relic);
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
    
    public void SetBluInstitute(Vector3Int position)
    {
        buildingMap.SetTile(position, blueInstitute);
    }
    
    public void SetRedInstitute(Vector3Int position)
    {
        buildingMap.SetTile(position, redInstitute);
    }

    Vector3Int GetMousePosition () {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
        Vector3Int position = grid.WorldToCell(worldPoint);
        return position;
    }
}
