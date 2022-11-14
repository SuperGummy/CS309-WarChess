using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public Grid grid;
	public bool gridEnable;
	public GameObject characterInfoButton;
	public GameObject placeInfoButton;
	
	// click state: 
	// 	0: Click show movement
	//  1: Click revert / move / attack
	//  2: Click move new recruited member to square
	int clickState = 0;
	int lastClickX = 0, lastClickY = 0;

    // Start is called before the first frame update
    void Start()
    {
        gridEnable = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (gridEnable && Input.GetMouseButtonDown(0))
        {
            //grid_ = GetComponent<Grid>();
            Vector3Int cellPosition = GetMousePosition();
			int x = cellPosition.x + 8;
			int y = cellPosition.y + 8;
			Debug.Log(x + " " + y);
			if(clickState == 0) {
				// TODO: check whether is a clickable grid: 
				// 		 1. Has a building of current player
				//       2. Has a character of current player
				lastClickX = x;
				lastClickY = y;
				clickState = 1;
				OnClickGridShow(x, y);
			}
			else if(clickState == 1) {
				if(x == lastClickX && y == lastClickY) {
					OnClickGridDisable();
					clickState = 0;
				}
			}
        }
    }
    Vector3Int GetMousePosition () {
	    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	    Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
	    Vector3Int position = grid.WorldToCell(worldPoint);
	    return position;
    }

	void OnClickGridShow(int x, int y) 
	{
		characterInfoButton.SetActive(true);
		characterInfoButton.GetComponent<CharacterInfoButton>().x = x;
		characterInfoButton.GetComponent<CharacterInfoButton>().y = y;
		placeInfoButton.SetActive(true);
		placeInfoButton.GetComponent<PlaceInfoButton>().x = x;
		placeInfoButton.GetComponent<PlaceInfoButton>().y = y;
	}

	void OnClickGridDisable() 
	{
		characterInfoButton.SetActive(false);
		placeInfoButton.SetActive(false);
	}
}
