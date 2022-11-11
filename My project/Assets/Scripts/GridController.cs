using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class GridController : MonoBehaviour
{
    private Grid grid;
    [SerializeField] private Tilemap interactiveMap = null;
    [SerializeField] private Tile hoverTile = null;
    private Vector3Int previousMousePos = new Vector3Int();


    // Start is called before the first frame update
    void Start() {
        grid = gameObject.GetComponent<Grid>();
    }
    
    // Update is called once per frame
    void Update() {
        // Mouse over -> highlight tile
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


    Vector3Int GetMousePosition () {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
        Vector3Int position = grid.WorldToCell(worldPoint);
        return position;
    }
}
