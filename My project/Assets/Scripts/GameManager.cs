using System.Collections.Generic;
using System.Net.Http;
using Audio;
using Cinemachine.Utility;
using Model;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private string _username1;
    private string _username2;
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
    public bool func;
    public bool recruit;
    public bool characterInfo;
    public bool structureInfo;
    public bool nextRound;

    private Vector3Int _previousPosition = Vector3Int.back;
    private List<Vector3Int> _characterActionRange;
    private List<Vector3Int> _characterAttackRange;
    private List<Vector3Int> _characterAvailablePosition;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Initiate();
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
            if (x < 0 || x >= DataManager.MapSize || y < 0 || y >= DataManager.MapSize) return;
            Debug.Log(x + " " + y + " " + cellPosition.z);

            if (_characterActionRange != null)
                if (_characterActionRange.Exists(c => c.x == x && c.y == y))
                    MoveCharacter(_previousPosition, position);

            if (_characterAttackRange != null)
                if (_characterAttackRange.Exists(c => c.x == x && c.y == y))
                    AttackPosition(_previousPosition, position);

            if (recruit)
                if (_characterAvailablePosition.Exists(c => c.x == x && c.y == y))
                    BuyCharacter(position);

            UpdatePosition(position);
            ShowCharacterInfoButton(position);
            ShowStructureInfoButton(position);
        }
    }

    private void UpdatePosition(Vector3Int position)
    {
        CloseActionRange();
        CloseAttackRange();
        if (recruit)
        {
            GridController.Instance.SetMovableHighlight(_characterAvailablePosition, false);
            recruit = false;
        }

        _previousPosition = position;
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
            if (characterInfo) return;
            characterInfo = true;
            characterInfoButton.SetActive(true);
        }
    }

    private void CloseCharacterInfoButton()
    {
        if (!characterInfo) return;
        characterInfo = false;
        characterInfoButton.SetActive(false);
    }

    private void ShowStructureInfoButton(Vector3Int position)
    {
        var structure = DataManager.Instance.GetStructureByPosition(position);
        if (structure == null) CloseStructureInfoButton();
        else if (structure.player == null || structure.player.id == DataManager.Instance.currentPlayer.id)
        {
            if (structureInfo) return;
            structureInfo = true;
            placeInfoButton.SetActive(true);
        }
        else CloseStructureInfoButton();
    }

    private void CloseStructureInfoButton()
    {
        if (!structureInfo) return;
        structureInfo = false;
        placeInfoButton.SetActive(false);
    }

    private async void Initiate()
    {
        await DataManager.Instance.Play("123", _username2);
        var pos1 = new Vector3Int(0, 16, 0);
        var pos2 = new Vector3Int(16, 0, 0);
        GridController.Instance.CreateCharacter(pos1);
        GridController.Instance.CreateCharacter(pos2);
        playerInfoBar.GetComponent<PlayerInfoBar>().RenderData();
    }

    public async void NextRound()
    {
        AudioManager.Instance.Play(2);
        if (nextRound) return;
        nextRound = true;
        await DataManager.Instance.Update(DataManager.Instance.currentPlayer.id);
        playerInfoBar.GetComponent<PlayerInfoBar>().RenderData();
        nextRound = false;
    }

    public void OpenBackPack()
    {
        if (backpack) return;
        backpack = true;
        SceneController.Instance.LoadBackPack();
        DisableBackground();
    }

    public void CloseBackPack()
    {
        if (!backpack) return;
        backpack = false;
        SceneController.Instance.UnloadBackPack();
        EnableBackground();
    }

    public void OpenShop()
    {
        if (func) return;
        func = true;
        SceneController.Instance.LoadShop();
        DisableBackground();
    }

    public void CloseShop()
    {
        if (!func) return;
        func = false;
        SceneController.Instance.UnloadShop();
        EnableBackground();
        playerInfoBar.GetComponent<PlayerInfoBar>().RenderData();
    }

    public void OpenTechnologies()
    {
        if (func) return;
        func = true;
        SceneController.Instance.LoadTechTree();
        DisableBackground();
    }

    public void CloseTechnologies()
    {
        if (!func) return;
        func = false;
        SceneController.Instance.UnloadTechTree();
        EnableBackground();
    }

    public void OpenEquip()
    {
        if (func) return;
        func = true;
        SceneController.Instance.LoadEquip();
        DisableBackground();
    }

    public void OpenUpgrade()
    {
        if (func) return;
        func = true;
        SceneController.Instance.LoadUpgrade();
        DisableBackground();
    }

    public void CloseUpgrade()
    {
        if (!func) return;
        func = false;
        SceneController.Instance.UnloadUpgrade();
        EnableBackground();
    }

    public void OpenCamp()
    {
        if (func) return;
        func = true;
        CampManager.position = _previousPosition;
        SceneController.Instance.LoadCamp();
        DisableBackground();
    }

    public void CloseCamp()
    {
        if (!func) return;
        func = false;
        SceneController.Instance.UnloadCamp();
        EnableBackground();
    }

    public void OpenRecruit()
    {
        if (func) return;
        func = true;
        _characterAvailablePosition = GetEmptyPosition(_previousPosition);
        RecruitManager.Position = _previousPosition;
        RecruitManager.Place = _characterAvailablePosition != null;
        SceneController.Instance.LoadRecruit();
        placeInfoFrame.SetActive(false);
        DisableBackground();
    }

    public void CloseRecruit()
    {
        if (!func) return;
        func = false;
        SceneController.Instance.UnloadRecruit();
        EnableBackground();
    }

    public void ShowCharacterInfo()
    {
        if (!characterInfo) return;
        characterInfoFrame.GetComponent<CharacterInfoFrame>().Inform(_previousPosition);
        characterInfoFrame.SetActive(true);
        DisableBackground();
    }

    public void CloseCharacterInfo()
    {
        characterInfoFrame.SetActive(false);
        EnableBackground();
    }

    public void ShowStructureInfo()
    {
        if (!structureInfo) return;
        placeInfoFrame.GetComponent<PlaceInfoFrame>().Inform(_previousPosition);
        placeInfoFrame.SetActive(true);
        DisableBackground();
    }

    public void CloseStructureInfo()
    {
        placeInfoFrame.SetActive(false);
        EnableBackground();
    }

    private void EnableBackground()
    {
        GridController.Instance.gridEnable = true;
        ShowCharacterInfoButton(_previousPosition);
        ShowStructureInfoButton(_previousPosition);
        finish.enabled = true;
        backpackButton.enabled = true;
    }

    private void DisableBackground()
    {
        GridController.Instance.gridEnable = false;
        CloseCharacterInfoButton();
        CloseStructureInfoButton();
        finish.enabled = false;
        backpackButton.enabled = false;
    }

    private void ShowActionRange(Vector3Int position)
    {
        _characterActionRange = GetActionRange(position);
        if (_characterActionRange == null) return;
        GridController.Instance.SetMovableHighlight(_characterActionRange, true);
    }

    private void CloseActionRange()
    {
        if (_characterActionRange == null) return;
        GridController.Instance.SetMovableHighlight(_characterActionRange, false);
        _characterActionRange = null;
    }

    private void ShowAttackRange(Vector3Int position)
    {
        _characterAttackRange = GetAttackRange(position);
        if (_characterAttackRange == null) return;
        GridController.Instance.SetAttackableHighlight(_characterAttackRange, true);
    }

    private void CloseAttackRange()
    {
        if (_characterAttackRange == null) return;
        GridController.Instance.SetAttackableHighlight(_characterAttackRange, false);
        _characterAttackRange = null;
    }

    public async void AttackPosition(Vector3Int positionAttack, Vector3Int positionAttacked)
    {
        if (DataManager.Instance.GetCharacterByPosition(positionAttacked) != null)
        {
            GridController.Instance.ShowDamageText(positionAttacked,
                DataManager.Instance.GetCharacterByPosition(positionAttack).attack -
                DataManager.Instance.GetCharacterByPosition(positionAttacked).defense);
            await DataManager.Instance.AttackCharacter(positionAttack, positionAttacked);
        }
        else if (DataManager.Instance.GetStructureByPosition(positionAttacked) != null)
        {
            GridController.Instance.ShowDamageText(positionAttacked,
                DataManager.Instance.GetCharacterByPosition(positionAttack).attack);
            await DataManager.Instance.AttackStructure(positionAttack, positionAttacked);
            if (DataManager.Instance.GetStructureByPosition(positionAttacked).player?.id ==
                DataManager.Instance.currentPlayer.id)
            {
                GridController.Instance.SetStructure(positionAttacked);
                GridController.Instance.ShowConquerText(positionAttacked);
            }
        }
        
        AudioManager.Instance.Play(1);
    }

    public void MoveCharacter(Vector3Int position, Vector3Int newPosition)
    {
        DataManager.Instance.MoveCharacter(position, newPosition);
        var path = GetActionPath(_previousPosition, newPosition);
        GridController.Instance.PlayCharacterRoute(path);
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

    public void UpdateTech(int t)
    {
        DataManager.Instance.UpdateTechnologies(_previousPosition, t);
        //do not need data to render

        //render
        //scene back to main scene 
    }

    public void UpdateCharacterAtCamp(int option)
    {
        DataManager.Instance.UpdateCharacter(_previousPosition, option);
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

    public void chooseNewCharacterposition(int id, int type)
    {
        if (recruit) return;
        recruit = true;
        CloseRecruit();
        DataManager.Instance.purchasingIndex = id;
        DataManager.Instance.purchasingType = type;
        GridController.Instance.SetMovableHighlight(_characterAvailablePosition, true);
    }

    public async void BuyCharacter(Vector3Int position)
    {
        await DataManager.Instance.BuyCharacters(_previousPosition, DataManager.Instance.purchasingIndex, position.x,
            position.y, DataManager.Instance.purchasingType);
        GridController.Instance.CreateCharacter(position);
        playerInfoBar.GetComponent<PlayerInfoBar>().RenderData();
    }

    public void UseItem(int itemid)
    {
        DataManager.Instance.UpdateItem(_previousPosition, itemid);
    }

    public void ChangeEquipment(int equipmentid, bool off)
    {
        DataManager.Instance.UpdateEquipment(_previousPosition, equipmentid, off);
        //no need to render
    }

    public void ChangeMount(int mountid, bool off)
    {
        DataManager.Instance.UpdateMount(_previousPosition, mountid, off);
    }

    public async void UpgradeStructure(int type = 0)
    {
        await DataManager.Instance.UpdateStructure(_previousPosition, type);
        GridController.Instance.SetStructure(_previousPosition);
        placeInfoFrame.GetComponent<PlaceInfoFrame>().RenderData(_previousPosition);
    }

    private bool CheckAccessible(Vector3Int position,bool type)
    {
        var x = position.x;
        var y = position.y;

        if (x < 0 || x >= DataManager.MapSize || y < 0 || y >= DataManager.MapSize)
        {
            return false;
        }

        if (y % 2 == 1 && x == DataManager.MapSize - 1)
        {
            return false;
        }

        var pos = new Vector3Int(position.y, position.x);
        if (DataManager.Instance.GetMapByPosition(pos) == 1 ||
                              (DataManager.Instance.GetMapByPosition(pos) == 2 && (!type)))
        {
            return false;
        }
        if (DataManager.Instance.GetCharacterByPosition(position) != null)
        {
            return false;
        }

        var structure = DataManager.Instance.GetStructureByPosition(position);
        if (structure != null)
        {
            if (structure.player == null) return false;
            if (structure.player.id != DataManager.Instance.currentPlayer.id) return false;
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

        var structure = DataManager.Instance.GetStructureByPosition(position);
        if (structure != null)
        {
            if (structure.player?.id != DataManager.Instance.currentPlayer.id) return true;
        }

        return false;
    }

    private List<Vector3Int> GetActionPath(Vector3Int position, Vector3Int target)
    {
        var character = DataManager.Instance.GetCharacterByPosition(position);
        bool type = character.characterClass == CharacterClass.EXPLORER;
        if (!CheckAccessible(target,type))
        {
            return default;
        }

        var n = DataManager.MapSize;
        var actionRange = character.actionRange + 1;
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
                var op = 1;
                if (j % 2 == 0) op = -1;
                var temp = new Vector3Int(i + op, j + 1);
                if (CheckAccessible(temp,type))
                {
                    if (dis[i + op, j + 1] == 0)
                    {
                        dis[i + op, j + 1] = t + 1;
                    }
                }

                temp = new Vector3Int(i + 1, j);
                if (CheckAccessible(temp,type))
                {
                    if (dis[i + 1, j] == 0)
                    {
                        dis[i + 1, j] = t + 1;
                    }
                }

                temp = new Vector3Int(i + op, j - 1);
                if (CheckAccessible(temp,type))
                {
                    if (dis[i + op, j - 1] == 0)
                    {
                        dis[i + op, j - 1] = t + 1;
                    }
                }

                temp = new Vector3Int(i, j + 1);
                if (CheckAccessible(temp,type))
                {
                    if (dis[i, j + 1] == 0)
                    {
                        dis[i, j + 1] = t + 1;
                    }
                }

                temp = new Vector3Int(i, j - 1);
                if (CheckAccessible(temp,type))
                {
                    if (dis[i, j - 1] == 0)
                    {
                        dis[i, j - 1] = t + 1;
                    }
                }

                temp = new Vector3Int(i - 1, j);
                if (CheckAccessible(temp,type))
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
        for (var t = dis[x, y] - 1; t >= 2; t--)
        {
            var op = 1;
            if (y % 2 == 0) op = -1;
            var temp = new Vector3Int(x + op, y + 1);
            if (CheckAccessible(temp,type))
            {
                if (dis[x + op, y + 1] == t)
                {
                    result.Add(temp);
                    x += op;
                    y += 1;
                    continue;
                }
            }

            temp = new Vector3Int(x + 1, y);
            if (CheckAccessible(temp,type))
            {
                if (dis[x + 1, y] == t)
                {
                    result.Add(temp);
                    x += 1;
                    continue;
                }
            }

            temp = new Vector3Int(x + op, y - 1);
            if (CheckAccessible(temp,type))
            {
                if (dis[x + op, y - 1] == t)
                {
                    result.Add(temp);
                    x += op;
                    y -= 1;
                    continue;
                }
            }

            temp = new Vector3Int(x, y + 1);
            if (CheckAccessible(temp,type))
            {
                if (dis[x, y + 1] == t)
                {
                    result.Add(temp);
                    y += 1;
                    continue;
                }
            }

            temp = new Vector3Int(x, y - 1);
            if (CheckAccessible(temp,type))
            {
                if (dis[x, y - 1] == t)
                {
                    result.Add(temp);
                    y -= 1;
                    continue;
                }
            }

            temp = new Vector3Int(x - 1, y);
            if (CheckAccessible(temp,type))
            {
                if (dis[x - 1, y] == t)
                {
                    result.Add(temp);
                    x -= 1;
                }
            }
        }

        result.Add(position);
        result.Reverse();
        // Debug.Log(result.Count);
        // for (var i=0;i<result.Count;i+=1)
        // {
        //     Debug.Log(result[i].x.ToString()+" "+result[i].y.ToString());
        // }
        return result;
    }

    private List<Vector3Int> GetActionRange(Vector3Int position)
    {
        var character = DataManager.Instance.GetCharacterByPosition(position);
        if (character == null)
        {
            return default;
        }
        bool type = character.characterClass == CharacterClass.EXPLORER;
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
        var actionRange = character.actionRange + 1;
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
                var op = 1;
                if (j % 2 == 0) op = -1;
                var temp = new Vector3Int(i + op, j + 1);
                if (CheckAccessible(temp,type))
                {
                    if (dis[i + op, j + 1] == 0)
                    {
                        dis[i + op, j + 1] = t + 1;
                    }
                }

                temp = new Vector3Int(i + 1, j);
                if (CheckAccessible(temp,type))
                {
                    if (dis[i + 1, j] == 0)
                    {
                        dis[i + 1, j] = t + 1;
                    }
                }

                temp = new Vector3Int(i + op, j - 1);
                if (CheckAccessible(temp,type))
                {
                    if (dis[i + op, j - 1] == 0)
                    {
                        dis[i + op, j - 1] = t + 1;
                    }
                }

                temp = new Vector3Int(i, j + 1);
                if (CheckAccessible(temp,type))
                {
                    if (dis[i, j + 1] == 0)
                    {
                        dis[i, j + 1] = t + 1;
                    }
                }

                temp = new Vector3Int(i, j - 1);
                if (CheckAccessible(temp,type))
                {
                    if (dis[i, j - 1] == 0)
                    {
                        dis[i, j - 1] = t + 1;
                    }
                }

                temp = new Vector3Int(i - 1, j);
                if (CheckAccessible(temp,type))
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
                if (dis[i, j] > 1)
                {
                    var temp = new Vector3Int(i, j);
                    result.Add(temp);
                }
            }
        }

        return result;
    }

    private List<Vector3Int> GetEmptyPosition(Vector3Int position)
    {
        var structure = DataManager.Instance.GetStructureByPosition(position);
        if (structure?.player == null || structure.player.id != DataManager.Instance.currentPlayer.id)
            return default;
        var type = false;

        var res = new List<Vector3Int>();
        var i = position.x;
        var j = position.y;
        var op = 1;
        if (j % 2 == 0) op = -1;
        var temp = new Vector3Int(i, j);
        if (CheckAccessible(temp,type))
        {
            res.Add(temp);
        }

        temp = new Vector3Int(i + op, j + 1);
        if (CheckAccessible(temp,type))
        {
            res.Add(temp);
        }

        temp = new Vector3Int(i + 1, j);
        if (CheckAccessible(temp,type))
        {
            res.Add(temp);
        }

        temp = new Vector3Int(i + op, j - 1);

        if (CheckAccessible(temp,type))
        {
            res.Add(temp);
        }

        temp = new Vector3Int(i, j + 1);

        if (CheckAccessible(temp,type))
        {
            res.Add(temp);
        }

        temp = new Vector3Int(i, j - 1);

        if (CheckAccessible(temp,type))
        {
            res.Add(temp);
        }

        temp = new Vector3Int(i - 1, j);

        if (CheckAccessible(temp,type))
        {
            res.Add(temp);
        }

        return res;
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
            return default;
        }

        var n = DataManager.MapSize;
        var attackRange = character.actionRange;
        if (character.equipment == null)
            attackRange += 1;
        else
            attackRange += character.equipment.attackRange + 1;
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
        for (var t = 1; t < attackRange; t++)
        for (var i = 0; i < n; i++)
        for (var j = 0; j < n; j++)
            if (dis[i, j] == t)
            {
                var op = 1;
                if (j % 2 == 0) op = -1;
                var temp = new Vector3Int(i + op, j + 1);
                if (CheckBound(temp))
                {
                    if (dis[i + op, j + 1] == 0)
                    {
                        dis[i + op, j + 1] = t + 1;
                    }
                }

                temp = new Vector3Int(i + 1, j);
                if (CheckBound(temp))
                {
                    if (dis[i + 1, j] == 0)
                    {
                        dis[i + 1, j] = t + 1;
                    }
                }

                temp = new Vector3Int(i + op, j - 1);
                if (CheckBound(temp))
                {
                    if (dis[i + op, j - 1] == 0)
                    {
                        dis[i + op, j - 1] = t + 1;
                    }
                }

                temp = new Vector3Int(i, j + 1);
                if (CheckBound(temp))
                {
                    if (dis[i, j + 1] == 0)
                    {
                        dis[i, j + 1] = t + 1;
                    }
                }

                temp = new Vector3Int(i, j - 1);
                if (CheckBound(temp))
                {
                    if (dis[i, j - 1] == 0)
                    {
                        dis[i, j - 1] = t + 1;
                    }
                }

                temp = new Vector3Int(i - 1, j);
                if (CheckBound(temp))
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
}