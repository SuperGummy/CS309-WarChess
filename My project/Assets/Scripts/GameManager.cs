using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Audio;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private string _username1;
    private string _username2;
    public Grid grid;

    public Button backpackButton;
    public Button finish;
    public Button shopButton;
    public GameObject playerInfoBar;
    public GameObject characterInfoButton;
    public GameObject characterInfoFrame;
    public GameObject placeInfoButton;
    public GameObject placeInfoFrame;

    public bool error;
    public bool func;
    public bool recruit;
    public bool characterInfo;
    public bool structureInfo;
    public bool nextRound;
    public bool stepBack;
    public bool current;
    public bool aiTurn;
    public static bool Load;
    public static String LoadPath;
    public static bool pvp;
    public static AI AI;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (GridController.Instance.gridEnable)
        {
            if (Input.GetMouseButtonDown(0) && (!aiTurn || pvp))
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

    public void OpenOptions()
    {
        if (func) return;
        func = true;
        SceneController.Instance.LoadOptions();
        DisableBackground();
    }

    public void CloseOptions()
    {
        if (!func) return;
        func = false;
        SceneController.Instance.UnloadOptions();
        EnableBackground();
    }

    public void OpenBackPack()
    {
        if (func) return;
        func = true;
        SceneController.Instance.LoadBackPack();
        DisableBackground();
    }

    public void CloseBackPack()
    {
        if (!func) return;
        func = false;
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
        Skilltree.position = _previousPosition;
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
        EquipManager.position = _previousPosition;
        SceneController.Instance.LoadEquip();
        DisableBackground();
    }

    public void CloseEquip()
    {
        if (!func) return;
        func = false;
        SceneController.Instance.UnloadEquip();
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
        _characterAvailablePosition = GameUtils.Instance.GetEmptyPosition(_previousPosition);
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
        shopButton.enabled = true;
    }

    private void DisableBackground()
    {
        GridController.Instance.gridEnable = false;
        CloseCharacterInfoButton();
        CloseStructureInfoButton();
        finish.enabled = false;
        backpackButton.enabled = false;
        shopButton.enabled = false;
    }

    private void ShowActionRange(Vector3Int position)
    {
        _characterActionRange = GameUtils.Instance.GetActionRange(position);
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
        _characterAttackRange = GameUtils.Instance.GetAttackRange(position);
        if (_characterAttackRange == null) return;
        GridController.Instance.SetAttackableHighlight(_characterAttackRange, true);
    }

    private void CloseAttackRange()
    {
        if (_characterAttackRange == null) return;
        GridController.Instance.SetAttackableHighlight(_characterAttackRange, false);
        _characterAttackRange = null;
    }

    public async void EndTurn()
    {
        await NextRound();
    }

    public async Task NextRound()
    {
        AudioManager.Instance.Play(2);
        if (nextRound) return;
        nextRound = true;
        CloseCharacterInfo();
        CloseCharacterInfoButton();
        CloseStructureInfo();
        CloseStructureInfoButton();
        CloseActionRange();
        CloseAttackRange();
        await DataManager.Instance.Update(DataManager.Instance.currentPlayer.id);
        playerInfoBar.GetComponent<PlayerInfoBar>().RenderData();
        nextRound = false;
        current = false;
        if (!pvp) aiTurn = !aiTurn;
        if (aiTurn) await AI.Run();
        else UIManager.Instance.ShowRoundChange();
    }

    public async void Initiate()
    {
        Progress<ProgressReportModel> progress = new Progress<ProgressReportModel>();
        progress.ProgressChanged += ReportProgress;
        if (!Load)
        {
            await DataManager.Instance.Play(PlayerPrefs.GetString("username", "123"), null, progress);
            var pos1 = new Vector3Int(0, 16, 0);
            var pos2 = new Vector3Int(16, 0, 0);
            GridController.Instance.CreateCharacter(pos1);
            GridController.Instance.CreateCharacter(pos2);
            playerInfoBar.GetComponent<PlayerInfoBar>().RenderData();
        }
        else
        {
            await DataManager.Instance.LoadArchive(LoadPath, progress);
        }
    }
    
    private void ReportProgress(object sender, ProgressReportModel report)
    {
        ProgressRenderer.Instance.SetSliderValue(report.ProgressValue);
    }

    public async void StepBack()
    {
        if (stepBack) return;
        stepBack = true;
        await DataManager.Instance.StepBack(current);
        playerInfoBar.GetComponent<PlayerInfoBar>().RenderData();
        stepBack = false;
        current = false;
    }

    public async Task AttackPosition(Vector3Int positionAttack, Vector3Int positionAttacked)
    {
        if (DataManager.Instance.GetCharacterByPosition(positionAttacked) != null)
        {
            await DataManager.Instance.AttackCharacter(positionAttack, positionAttacked);
            GridController.Instance.ShowDamageText(positionAttacked,
                DataManager.Instance.GetCharacterByPosition(positionAttack).attack -
                DataManager.Instance.GetCharacterByPosition(positionAttacked).defense);
            AudioManager.Instance.Play(1);
            current = true;
        }
        else if (DataManager.Instance.GetStructureByPosition(positionAttacked) != null)
        {
            await DataManager.Instance.AttackStructure(positionAttack, positionAttacked);
            GridController.Instance.ShowDamageText(positionAttacked,
                DataManager.Instance.GetCharacterByPosition(positionAttack).attack);
            if (DataManager.Instance.GetStructureByPosition(positionAttacked).player?.id ==
                DataManager.Instance.currentPlayer.id)
            {
                GridController.Instance.SetStructure(positionAttacked);
                GridController.Instance.ShowConquerText(positionAttacked);
            }

            AudioManager.Instance.Play(1);
            current = true;
        }
    }

    public async Task MoveCharacter(Vector3Int position, Vector3Int newPosition)
    {
        var path = GameUtils.Instance.GetActionPath(position, newPosition);
        await DataManager.Instance.MoveCharacter(position, newPosition);
        GridController.Instance.PlayCharacterRoute(path);
        AudioManager.Instance.Play(3);
        current = true;
    }

    public void Discharge()
    {
        DismissCharacter(_previousPosition);
    }

    public async void DismissCharacter(Vector3Int position)
    {
        await DataManager.Instance.DismissCharacter(position);
        GridController.Instance.DeleteCharacter(position);
        CloseCharacterInfo();
        playerInfoBar.GetComponent<PlayerInfoBar>().RenderData();
        current = true;
    }

    public async Task UpdateTech(int t)
    {
        await DataManager.Instance.UpdateTechnologies(_previousPosition, t);

        //render
        //scene back to main scene 
        current = true;
    }

    public void UpdateCharacterAtCamp(int option)
    {
        Task task = Task.Run(async () => { await DataManager.Instance.UpdateCharacter(_previousPosition, option);});
        task.Wait();
        current = true;
    }

    public void EarnStars()
    {
        Task task = Task.Run(async () => { await DataManager.Instance.EarnStars(_previousPosition);});
        task.Wait();
        current = true;
    }
    
    public async Task BuyEquipment(int shopId)
    {
        await DataManager.Instance.BuyEquipments(shopId);
        current = true;
    }

    public async Task BuyMount(int shopId)
    {
        await DataManager.Instance.BuyMounts(shopId);
        current = true;
    }

    public async Task BuyItem(int shopId)
    {
        await DataManager.Instance.BuyItems(shopId);
        current = true;
    }

    public void ChooseNewCharacterPosition(int id, int type)
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
        current = true;
    }

    public async Task UseItem(int itemid)
    {
        await DataManager.Instance.UpdateItem(_previousPosition, itemid);
        characterInfoFrame.GetComponent<CharacterInfoFrame>().RenderData(_previousPosition);
        current = true;
    }

    public async Task ChangeEquipment(int equipmentid, bool off)
    {
        await DataManager.Instance.UpdateEquipment(_previousPosition, equipmentid, off);
        characterInfoFrame.GetComponent<CharacterInfoFrame>().RenderData(_previousPosition);
        current = true;
    }

    public async Task ChangeMount(int mountid, bool off)
    {
        await DataManager.Instance.UpdateMount(_previousPosition, mountid, off);
        characterInfoFrame.GetComponent<CharacterInfoFrame>().RenderData(_previousPosition);
        current = true;
    }

    public async Task UpgradeStructure(int type = -1)
    {
        await DataManager.Instance.UpdateStructure(_previousPosition, type);
        GridController.Instance.SetStructure(_previousPosition);
        placeInfoFrame.GetComponent<PlaceInfoFrame>().RenderData(_previousPosition);
        playerInfoBar.GetComponent<PlayerInfoBar>().RenderData();
        current = true;
    }

    public async void UpgradeStructure(Vector3Int position, int type = -1)
    {
        await DataManager.Instance.UpdateStructure(position, type);
        GridController.Instance.SetStructure(position);
        CloseStructureInfo();
        //placeInfoFrame.GetComponent<PlaceInfoFrame>().RenderData(position);
        current = true;
    }

    public async void HealStructure()
    {
        var hpOld = DataManager.Instance.GetStructureByPosition(_previousPosition).hp;
        await DataManager.Instance.HealStructure(_previousPosition);
        GridController.Instance.ShowAddHealthText(_previousPosition,
            DataManager.Instance.GetStructureByPosition(_previousPosition).hp - hpOld);
        playerInfoBar.GetComponent<PlayerInfoBar>().RenderData();
        placeInfoFrame.GetComponent<PlaceInfoFrame>().RenderData(_previousPosition);
        current = true;
    }

    public void SaveArchive()
    {
        DataManager.Instance.SaveArchive();
    }

    public async void LoadArchive(string path)
    {
        await DataManager.Instance.LoadArchive(path);
    }

}