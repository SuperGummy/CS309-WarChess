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

	private bool CheckAccessible(Vector3Int position)
	{
		var x = position.x;
		var y = position.y;
		
		if (x < 0 || x >= DataManager.MapSize || y < 0 || y >= DataManager.MapSize)
		{
			return false;
		}

		if (DataManager.Instance.GetCharacterByPosition(position) != null)
		{
			return false;
		}

		if (DataManager.Instance.GetStructureByPosition(position).player.id != DataManager.Instance.currentPlayer.id)
		{
			return false;
		}

		return true;
	}
	
	
	private static bool CheckBound(Vector3Int position)
	{
		var x = position.x;
		var y = position.y;
		return !(x < 0 || x >= DataManager.MapSize || y < 0 || y >= DataManager.MapSize);
	}

	private bool CheckAttack(Vector3Int position)
	{
		if (DataManager.Instance.GetCharacterByPosition(position) != null)
		{
			if (DataManager.Instance.GetCharacterByPosition(position).player.id !=
			    DataManager.Instance.currentPlayer.id)
			{
				return true;
			}
		}

		if (DataManager.Instance.GetStructureByPosition(position) != null)
		{
			if (DataManager.Instance.GetStructureByPosition(position).player == null ||
			    DataManager.Instance.GetStructureByPosition(position).player.id !=
			    DataManager.Instance.currentPlayer.id)
			{
				return true;
			}
		}
		return false;
	}
	
	private List<Vector3Int> GetAttackRange(Vector3Int position)
	{
		var character = DataManager.Instance.GetCharacterByPosition(position);
		if (character == null)
		{
			return default;
		}

		if (character.player.id != DataManager.Instance.currentPlayer.id || character.hp <= 0)
		{
			return default;
		}

		if (character.actionState >= 2)
		{
			// can't attack
			return default;
		}

		var n = DataManager.MapSize;
		var actionRange = character.actionRange;
		var result = new List<Vector3Int>();
		var dis = new int[n,n];
		for (var i = 0; i < n; i++)
		{
			for (var j = 0; j < n; j++)
			{
				dis[i,j] = 0;
			}
		}
		dis[position.x,position.y] = 1;
		for (var t = 1; t < actionRange; t++)
		for (var i = 0; i < n; i++)
		for (var j = 0; j < n; j++)
			if (dis[i, j] == t)
			{
				if (CheckBound(new Vector3Int(i, j + 1, position.z)))
				{
					if (dis[i, j + 1] == 0)
					{
						dis[i, j + 1] = t + 1;
					}
				}

				if (CheckBound(new Vector3Int(i, j - 1, position.z)))
				{
					if (dis[i, j - 1] == 0)
					{
						dis[i, j - 1] = t + 1;
					}
				}

				if (CheckBound(new Vector3Int(i + 1, j, position.z)))
				{
					if (dis[i + 1, j] == 0)
					{
						dis[i + 1, j] = t + 1;
					}
				}

				if (CheckBound(new Vector3Int(i - 1, j, position.z)))
				{
					if (dis[i - 1, j] == 0)
					{
						dis[i - 1, j] = t + 1;
					}
				}


				if (CheckBound(new Vector3Int(i + 1, j - 1, position.z)))
				{
					if (dis[i + 1, j] == 0)
					{
						dis[i + 1, j] = t + 1;
					}
				}

				if (CheckBound(new Vector3Int(i - 1, j - 1, position.z)))
				{
					if (dis[i - 1, j] == 0)
					{
						dis[i - 1, j] = t + 1;
					}
				}
			}

		for (var i = 0; i < n; i++)
		{
			for (var j = 0; j < n; j++)
			{
				if (dis[i, j] != 0 && CheckAttack(new Vector3Int(i,j,position.z)))
				{
					result.Add(new Vector3Int(i,j,position.z));
				}
			}
		}

		return result;
	}
	
	private List<Vector3Int> GetActionRange(Vector3Int position)
	{
		var character = DataManager.Instance.GetCharacterByPosition(position);
		if (character == null)
		{
			return default;
		}

		if (character.player.id != DataManager.Instance.currentPlayer.id || character.hp <= 0)
		{
			return default;
		}

		if (character.actionState >= 1)
		{
			// can't move
			return default;
		}

		var n = DataManager.MapSize;
		var actionRange = character.actionRange;
		var result = new List<Vector3Int>();
		var dis = new int[n,n];
		for (var i = 0; i < n; i++)
		{
			for (var j = 0; j < n; j++)
			{
				dis[i,j] = 0;
			}
		}
		dis[position.x,position.y] = 1;
		for (var t = 1; t < actionRange; t++)
		for (var i = 0; i < n; i++)
		for (var j = 0; j < n; j++) 
			if (dis[i, j] == t)
			{
				if (CheckAccessible(new Vector3Int(i,j+1,position.z)))
				{
					if (dis[i, j + 1] == 0)
					{
						dis[i, j + 1] = t + 1;
					}
				}	
				
				if (CheckAccessible(new Vector3Int(i,j-1,position.z)))
				{
					if (dis[i, j - 1] == 0)
					{
						dis[i, j - 1] = t + 1;
					}
				}	

				if (CheckAccessible(new Vector3Int(i+1,j,position.z)))
				{
					if (dis[i+1, j] == 0)
					{
						dis[i+1, j] = t + 1;
					}
				}	
				
				if (CheckAccessible(new Vector3Int(i-1,j,position.z)))
				{
					if (dis[i-1, j] == 0)
					{
						dis[i-1, j] = t + 1;
					}
				}
				
				
				if (CheckAccessible(new Vector3Int(i+1,j-1,position.z)))
				{
					if (dis[i+1, j] == 0)
					{
						dis[i+1, j] = t + 1;
					}
				}	
				
				if (CheckAccessible(new Vector3Int(i-1,j-1,position.z)))
				{
					if (dis[i-1, j] == 0)
					{
						dis[i-1, j] = t + 1;
					}
				}
			}

		for (var i = 0; i < n; i++)
		{
			for (var j = 0; j < n; j++)
			{
				if (dis[i, j] != 0)
				{
					result.Add(new Vector3Int(i,j,position.z));
				}
			}
		}

		return result;
	}
	
	private List<Vector3Int> GetActionPath(Vector3Int position,Vector3Int target)
	{
		var character = DataManager.Instance.GetCharacterByPosition(position);
		if (!CheckAccessible(target))
		{
			return default;
		}

		var n = DataManager.MapSize;
		var actionRange = character.actionRange;
		var result = new List<Vector3Int>();
		var dis = new int[n,n];
		for (var i = 0; i < n; i++)
		{
			for (var j = 0; j < n; j++)
			{
				dis[i,j] = 0;
			}
		}
		dis[position.x,position.y] = 1;
		for (var t = 1; t < actionRange; t++)
		for (var i = 0; i < n; i++)
		for (var j = 0; j < n; j++) 
			if (dis[i, j] == t)
			{
				if (CheckAccessible(new Vector3Int(i,j+1,position.z)))
				{
					if (dis[i, j + 1] == 0)
					{
						dis[i, j + 1] = t + 1;
					}
				}	
				
				if (CheckAccessible(new Vector3Int(i,j-1,position.z)))
				{
					if (dis[i, j - 1] == 0)
					{
						dis[i, j - 1] = t + 1;
					}
				}	

				if (CheckAccessible(new Vector3Int(i+1,j,position.z)))
				{
					if (dis[i+1, j] == 0)
					{
						dis[i+1, j] = t + 1;
					}
				}	
				
				if (CheckAccessible(new Vector3Int(i-1,j,position.z)))
				{
					if (dis[i-1, j] == 0)
					{
						dis[i-1, j] = t + 1;
					}
				}
				
				
				if (CheckAccessible(new Vector3Int(i+1,j-1,position.z)))
				{
					if (dis[i+1, j] == 0)
					{
						dis[i+1, j] = t + 1;
					}
				}	
				
				if (CheckAccessible(new Vector3Int(i-1,j-1,position.z)))
				{
					if (dis[i-1, j] == 0)
					{
						dis[i-1, j] = t + 1;
					}
				}
			}

		if (dis[target.x, target.y] == 0)
		{
			//can't
			return default;
		}

		var x = target.x;
		var y = target.y;
		result.Add(target);
		for (var t = dis[x, y]-1; t >=1; t--)
		{
			if (dis[x, y - 1] == t)
			{
				y -= 1;
			}
			else if (dis[x, y + 1] == t)
			{
				y += 1;
			}
			else if (dis[x+1, y - 1] == t)
			{
				x += 1;
				y -= 1;
			}
			else if (dis[x-1, y - 1] == t)
			{
				x -= 1;
				y -= 1;
			}
			else if (dis[x+1, y] == t)
			{
				x += 1;
			}
			else if (dis[x-1, y] == t)
			{
				x -= 1;
			}
			result.Add(new Vector3Int(x,y,target.z));
		}
		result.Reverse();

		return result;
	}

	
}
