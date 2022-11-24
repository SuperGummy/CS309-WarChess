using System.Collections.Generic;
using Cinemachine.Utility;
using Model;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Grid grid;

    public Button backpackButton;
    public Button finish;
    public GameObject playerInfoBar;
    public GameObject characterInfoButton;
    public GameObject characterInfoFrame;
    public GameObject placeInfoButton;
    public GameObject placeInfoFrame;

    public bool error;
    public bool backpack;
    public bool shop;
    public bool characterInfo;
    public bool structureInfo;
    public bool nextRound;

    private Vector3Int previousPosition = Vector3Int.back;
    private List<Vector3Int> characterActionRange;
    private List<Vector3Int> characterAttackRange;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        backpackButton = backpackButton.GetComponent<Button>();
        finish = finish.GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GridController.Instance.gridEnable && Input.GetMouseButtonDown(0))
        {
            var cellPosition = GetMousePosition();
            var x = cellPosition.x + 8;
            var y = cellPosition.y + 8;
            var position = new Vector3Int(x, y, cellPosition.z);
            if (x < 0 || x >= DataManager.MapSize || y < 0 || y >= DataManager.MapSize)
            {
                return;
            }

            Debug.Log(x + " " + y + " " + cellPosition.z);

            if (characterActionRange != null)
            {
                if (characterActionRange.Exists(c => c.x == x && c.y == y))
                {
                    MoveCharacter(previousPosition, position);
                }
            }

            if (characterAttackRange != null)
            {
                if (characterAttackRange.Exists(c => c.x == x && c.y == y))
                {
                    AttackPosition(previousPosition, position);
                }
            }

            UpdatePosition(position);
            ShowCharacterInfoButton(position);
            ShowStructureInfoButton(position);
        }
    }

    private void UpdatePosition(Vector3Int position)
    {
        CloseActionRange();
        CloseAttackRange();
        previousPosition = position;
        ShowActionRange(position);
        ShowAttackRange(position);
    }

    private Vector3Int GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
        Vector3Int position = grid.WorldToCell(worldPoint);
        return position;
    }

    private void ShowCharacterInfoButton(Vector3Int position)
    {
        var character = DataManager.Instance.GetCharacterByPosition(position);
        if (character?.player?.id != DataManager.Instance.currentPlayer.id)
        {
            CloseCharacterInfoButton();
        }
        else
        {
            if (characterInfo)
            {
                return;
            }

            characterInfo = true;
            characterInfoButton.SetActive(true);
        }
    }

    private void CloseCharacterInfoButton()
    {
        if (!characterInfo)
        {
            return;
        }

        characterInfo = false;
        characterInfoButton.SetActive(false);
    }

    private void ShowStructureInfoButton(Vector3Int position)
    {
        var structure = DataManager.Instance.GetStructureByPosition(position);
        if (structure?.player?.id != DataManager.Instance.currentPlayer.id)
        {
            CloseStructureInfoButton();
        }
        else
        {
            if (structureInfo)
            {
                return;
            }

            structureInfo = true;
            placeInfoButton.SetActive(true);
        }
    }

    private void CloseStructureInfoButton()
    {
        if (!structureInfo)
        {
            return;
        }

        structureInfo = false;
        placeInfoButton.SetActive(false);
    }

    // backpack
    public async void OpenBackPack()
    {
        if (backpack)
        {
            return;
        }

        backpack = true;
        // var task1 = DataManager.Instance.GetEquipments();
        // var task2 = DataManager.Instance.GetItem();
        // var task3 = DataManager.Instance.GetMount();
        // await Task.WhenAll(task1, task2, task3).ContinueWith(task =>
        // {
        //     if (task.IsFaulted)
        //     {
        //         error = true;
        //         backpack = false;
        //         GridController.Instance.gridEnable = true;
        //         finish.enabled = true;
        //         Debug.Log(task.Exception?.Message);
        //     }
        // });

        if (error)
        {
            backpack = false;
            GridController.Instance.gridEnable = true;
            finish.enabled = true;
        }
        else
        {
            SceneController.Instance.LoadBackPack();
            GridController.Instance.gridEnable = false;
            CloseCharacterInfoButton();
            CloseStructureInfoButton();
            finish.enabled = false;
            backpackButton.enabled = false;
        }

        error = false;
    }

    public void CloseBackPack()
    {
        if (!backpack)
        {
            return;
        }

        backpack = false;
        SceneController.Instance.UnloadBackPack();
        GridController.Instance.gridEnable = true;
        ShowCharacterInfoButton(previousPosition);
        ShowStructureInfoButton(previousPosition);
        finish.enabled = true;
        backpackButton.enabled = true;
    }

    // show character
    public void ShowCharacterInfo()
    {
        if (!characterInfo) return;
        characterInfoFrame.GetComponent<CharacterInfoFrame>().Inform(previousPosition);
        characterInfoFrame.SetActive(true);
        GridController.Instance.gridEnable = false;
        CloseCharacterInfoButton();
        CloseStructureInfoButton();
        finish.enabled = false;
        backpackButton.enabled = false;
    }

    public void CloseCharacterInfo()
    {
        GridController.Instance.gridEnable = true;
        ShowCharacterInfoButton(previousPosition);
        ShowStructureInfoButton(previousPosition);
        finish.enabled = true;
        backpackButton.enabled = true;
    }

    public void ShowStructureInfo()
    {
        if (!structureInfo) return;
        placeInfoFrame.GetComponent<PlaceInfoFrame>().Inform(previousPosition);
        placeInfoFrame.SetActive(true);

        GridController.Instance.gridEnable = false;
        CloseCharacterInfoButton();
        CloseStructureInfoButton();
        finish.enabled = false;
        backpackButton.enabled = false;
    }

    public void CloseStructureInfo()
    {
        GridController.Instance.gridEnable = true;
        ShowCharacterInfoButton(previousPosition);
        ShowStructureInfoButton(previousPosition);
        finish.enabled = true;
        backpackButton.enabled = true;
    }

    public void OpenShop()
    {
        if (shop)
        {
            return;
        }

        shop = true;
        var task1 = DataManager.Instance.currentPlayer.shop;
        //need get data to render
        GridController.Instance.gridEnable = false;
        CloseCharacterInfoButton();
        CloseStructureInfoButton();
        finish.enabled = false;
        backpackButton.enabled = false;


        //render shop frame 
    }

    public void CloseShop()
    {
        if (!shop)
        {
            return;
        }

        shop = false;
        GridController.Instance.gridEnable = true;
        ShowCharacterInfoButton(previousPosition);
        ShowStructureInfoButton(previousPosition);
        finish.enabled = true;
        backpackButton.enabled = true;

        //close shop frame
    }

    private void ShowActionRange(Vector3Int position)
    {
        characterActionRange = GetActionRange(position);
        if (characterActionRange == null)
        {
            return;
        }
        // render range
    }

    private void CloseActionRange()
    {
        characterActionRange = null;
        // render
    }

    private void ShowAttackRange(Vector3Int position)
    {
        characterAttackRange = GetAttackRange(position);
        if (characterAttackRange == null)
        {
            return;
        }

        // render range
    }

    private void CloseAttackRange()
    {
        characterAttackRange = null;
        //render
    }

    public void GetCharacter(int id)
    {
        

        //render the character at position ?
    }

    public async void AttackPosition(Vector3Int positionAttack, Vector3Int positionAttacked)
    {
        // animation of attack

        if (DataManager.Instance.GetCharacterByPosition(positionAttacked) != null)
        {
            await DataManager.Instance.AttackCharacter(positionAttack, positionAttacked);
        }
        else if (DataManager.Instance.GetStructureByPosition(positionAttacked) != null)
        {
            await DataManager.Instance.AttackStructure(positionAttack, positionAttacked);
            if (DataManager.Instance.GetStructureByPosition(positionAttacked).player?.id ==
                DataManager.Instance.currentPlayer.id)
            {
                if (DataManager.Instance.currentPlayer.id == DataManager.Instance.player1.id)
                {
                    switch (DataManager.Instance.GetStructureByPosition(positionAttacked).structureClass)
                    {
                        case StructureClass.CAMP:
                            GridController.Instance.SetBlueCamp(positionAttacked);
                            break;
                        case StructureClass.MARKET:
                            GridController.Instance.SetBlueMarket(positionAttacked);
                            break;
                        case StructureClass.INSTITUTE:
                            GridController.Instance.SetBlueInstitute(positionAttacked);
                            break;
                        default:
                            return;
                    }
                }
                else
                {
                    switch (DataManager.Instance.GetStructureByPosition(positionAttacked).structureClass)
                    {
                        case StructureClass.CAMP:
                            GridController.Instance.SetRedCamp(positionAttacked);
                            break;
                        case StructureClass.MARKET:
                            GridController.Instance.SetRedMarket(positionAttacked);
                            break;
                        case StructureClass.INSTITUTE:
                            GridController.Instance.SetRedInstitute(positionAttacked);
                            break;
                        default:
                            return;
                    }
                }
            }
        }
    }

    public async void MoveCharacter(Vector3Int position, Vector3Int newPosition)
    {
        var path = GetActionPath(previousPosition, position);
        DataManager.Instance.MoveCharacter(position, newPosition);


        // render
        // add character to new position   delete character at position
    }

    public async void DismissCharacter(Vector3Int position)
    {
        // api
        var character = DataManager.Instance.GetCharacterByPosition(position);
        await DataManager.Instance.DismissCharacter(position);
        //need new data to render

        // render 
        // update position character
    }

    public void TransferToTechnologiesScene()
    {
        //api
        DataManager.Instance.GetTechnologies();

        //render
        //change to tech scene and it use data DataManager.instance._tech
    }


    public void UpdateTech(int t)
    {
        DataManager.Instance.UpdateTechnologies(previousPosition, t);
        //do not need data to render

        //render
        //scene back to main scene 
    }

    public void UpdateCharacterAtCamp(int option, GameObject t)
    {
        DataManager.Instance.UpdateCharacter(previousPosition, option);

        if (t != null)
        {
            //close select window
            t.GetComponent<Camp.CampCloseParentPanelButton>().OnClick();
        }
    }

    public async void BuyEquipment(int shopId)
    {
        //api
        await DataManager.Instance.BuyEquipments(shopId);

        //render     refresh shop frame (delete one equipment)
    }

    public async void BuyMount(int shopId)
    {
        //api
        await DataManager.Instance.BuyMounts(shopId);

        //render     refresh shop frame (delete one mount)
    }

    public async void BuyItem(int shopId)
    {
        //api
        await DataManager.Instance.BuyItems(shopId);

        //render    refresh shop frame
    }

    public async void BuyCharacter(int id, int x, int y, int type)
    {
        await DataManager.Instance.BuyCharacters(previousPosition, id, x, y, type);
        //need character data ro render

        //render
        //update character at (x,y)
    }

    public void UseItem(int itemid)
    {
        DataManager.Instance.UpdateItem(previousPosition, itemid);
    }

    public void ChangeEquipment(int equipmentid, bool off)
    {
        DataManager.Instance.UpdateEquipment(previousPosition, equipmentid, off);
        //no need to render
    }

    public void ChangeMount(int mountid, bool off)
    {
        DataManager.Instance.UpdateMount(previousPosition, mountid, off);
    }

    public async void UpgradeStructure(int type = 0)
    {
        await DataManager.Instance.UpdateStructure(previousPosition, type);

        //render    may change structure
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

        if (x == 0 && y == DataManager.MapSize - 1)
        {
            return false;
        }

        if (x == DataManager.MapSize - 1 && y == 0)
        {
            return false;
        }
        

        if (DataManager.Instance.GetStructureByPosition(position) != null)
        {
            if (DataManager.Instance.GetStructureByPosition(position)?.player.id !=
                DataManager.Instance.currentPlayer.id)
            {
                return false;
            }
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
            if (DataManager.Instance.GetStructureByPosition(position)?.player.id !=
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
        var dis = new int[n, n];
        for (var i = 0; i < n; i++)
        {
            for (var j = 0; j < n; j++)
            {
                dis[i, j] = 0;
            }
        }

        dis[position.x, position.y] = 1;
        for (var t = 1; t < actionRange; t++)
        for (var i = 0; i < n; i++)
        for (var j = 0; j < n; j++)
            if (dis[i, j] == t)
            {
                var temp = new Vector3Int(i, j + 1);
                if (CheckAccessible(temp))
                {
                    if (dis[i, j + 1] == 0)
                    {
                        dis[i, j + 1] = t + 1;
                    }
                }

                temp = new Vector3Int(i, j - 1);
                if (CheckAccessible(temp))
                {
                    if (dis[i, j - 1] == 0)
                    {
                        dis[i, j - 1] = t + 1;
                    }
                }

                temp = new Vector3Int(i + 1, j);
                if (CheckAccessible(temp))
                {
                    if (dis[i + 1, j] == 0)
                    {
                        dis[i + 1, j] = t + 1;
                    }
                }

                temp = new Vector3Int(i - 1, j);
                if (CheckAccessible(temp))
                {
                    if (dis[i - 1, j] == 0)
                    {
                        dis[i - 1, j] = t + 1;
                    }
                }

                temp = new Vector3Int(i + 1, j - 1);
                if (CheckAccessible(temp))
                {
                    if (dis[i + 1, j] == 0)
                    {
                        dis[i + 1, j] = t + 1;
                    }
                }

                temp = new Vector3Int(i - 1, j - 1);
                if (CheckAccessible(temp))
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
                var temp = new Vector3Int(i, j);
                if (dis[i, j] != 0 && CheckAttack(temp))
                {
                    result.Add(temp);
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
        var dis = new int[n, n];
        for (var i = 0; i < n; i++)
        {
            for (var j = 0; j < n; j++)
            {
                dis[i, j] = 0;
            }
        }

        dis[position.x, position.y] = 1;
        for (var t = 1; t < actionRange; t++)
        for (var i = 0; i < n; i++)
        for (var j = 0; j < n; j++)
            if (dis[i, j] == t)
            {
                var temp = new Vector3Int(i, j + 1);
                if (CheckAccessible(temp))
                {
                    if (dis[i, j + 1] == 0)
                    {
                        dis[i, j + 1] = t + 1;
                    }
                }

                temp = new Vector3Int(i, j - 1);
                if (CheckAccessible(temp))
                {
                    if (dis[i, j - 1] == 0)
                    {
                        dis[i, j - 1] = t + 1;
                    }
                }

                temp = new Vector3Int(i + 1, j);
                if (CheckAccessible(temp))
                {
                    if (dis[i + 1, j] == 0)
                    {
                        dis[i + 1, j] = t + 1;
                    }
                }

                temp = new Vector3Int(i - 1, j);
                if (CheckAccessible(temp))
                {
                    if (dis[i - 1, j] == 0)
                    {
                        dis[i - 1, j] = t + 1;
                    }
                }

                temp = new Vector3Int(i + 1, j - 1);
                if (CheckAccessible(temp))
                {
                    if (dis[i + 1, j] == 0)
                    {
                        dis[i + 1, j] = t + 1;
                    }
                }

                temp = new Vector3Int(i - 1, j - 1);
                if (CheckAccessible(temp))
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
                if (dis[i, j] != 0)
                {
                    var temp = new Vector3Int(i, j);
                    result.Add(temp);
                }
            }
        }

        return result;
    }

    private List<Vector3Int> GetActionPath(Vector3Int position, Vector3Int target)
    {
        var character = DataManager.Instance.GetCharacterByPosition(position);
        if (!CheckAccessible(target))
        {
            return default;
        }

        var n = DataManager.MapSize;
        var actionRange = character.actionRange;
        var result = new List<Vector3Int>();
        var dis = new int[n, n];
        for (var i = 0; i < n; i++)
        {
            for (var j = 0; j < n; j++)
            {
                dis[i, j] = 0;
            }
        }

        dis[position.x, position.y] = 1;
        for (var t = 1; t < actionRange; t++)
        for (var i = 0; i < n; i++)
        for (var j = 0; j < n; j++)
            if (dis[i, j] == t)
            {
                if (CheckAccessible(new Vector3Int(i, j + 1, position.z)))
                {
                    if (dis[i, j + 1] == 0)
                    {
                        dis[i, j + 1] = t + 1;
                    }
                }

                if (CheckAccessible(new Vector3Int(i, j - 1, position.z)))
                {
                    if (dis[i, j - 1] == 0)
                    {
                        dis[i, j - 1] = t + 1;
                    }
                }

                if (CheckAccessible(new Vector3Int(i + 1, j, position.z)))
                {
                    if (dis[i + 1, j] == 0)
                    {
                        dis[i + 1, j] = t + 1;
                    }
                }

                if (CheckAccessible(new Vector3Int(i - 1, j, position.z)))
                {
                    if (dis[i - 1, j] == 0)
                    {
                        dis[i - 1, j] = t + 1;
                    }
                }


                if (CheckAccessible(new Vector3Int(i + 1, j - 1, position.z)))
                {
                    if (dis[i + 1, j] == 0)
                    {
                        dis[i + 1, j] = t + 1;
                    }
                }

                if (CheckAccessible(new Vector3Int(i - 1, j - 1, position.z)))
                {
                    if (dis[i - 1, j] == 0)
                    {
                        dis[i - 1, j] = t + 1;
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
        for (var t = dis[x, y] - 1; t >= 1; t--)
        {
            if (dis[x, y - 1] == t)
            {
                y -= 1;
            }
            else if (dis[x, y + 1] == t)
            {
                y += 1;
            }
            else if (dis[x + 1, y - 1] == t)
            {
                x += 1;
                y -= 1;
            }
            else if (dis[x - 1, y - 1] == t)
            {
                x -= 1;
                y -= 1;
            }
            else if (dis[x + 1, y] == t)
            {
                x += 1;
            }
            else if (dis[x - 1, y] == t)
            {
                x -= 1;
            }

            result.Add(new Vector3Int(x, y, target.z));
        }

        result.Reverse();

        return result;
    }
}