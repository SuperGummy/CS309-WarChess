using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using api = API.Service;
using Model;
using Newtonsoft.Json;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.XR.LegacyInputHelpers;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using File = UnityEngine.Windows.File;
using Random = System.Random;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    public const int MapSize = 17;
    public const int TechSize = 11;
    public int gameID;
    public int round;
    public Player currentPlayer;
    public Player player1;
    public Player player2;
    private int[] _map = new int[MapSize * MapSize];
    private int[] _realMap = new int[MapSize * MapSize];
    private Character[] characters = new Character[MapSize * MapSize];
    private Structure[] structures = new Structure[MapSize * MapSize];

    public int purchasingIndex;
    public int purchasingType;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SaveArchive(string path)
    {
        var archive = CreateArchiveObject();
        var JsonString = JsonUtility.ToJson(archive, true);
        Debug.Log("-------save json string--------");
        Debug.Log(JsonString);
        StreamWriter sw = new StreamWriter(path);
        sw.Write(JsonString);
        sw.Close();
    }

    private Model.Player ChangePlayerModel(Player player, Model.Character[] character, Model.Structure[] structure)
    {
        Model.Player result = new Model.Player();
        result.id = player.id;
        result.stars = player.id;
        result.prosperityDegree = player.prosperityDegree;
        result.peaceDegree = player.peaceDegree;
        result.equipments = player.equipments;
        result.mounts = player.mounts;
        result.items = player.items;
        result.characters = character;
        result.structures = structure;
        return result;
    }

    private Model.Character ChangeCharacterModel(Character character, int x = 0, int y = 0)
    {
        var result = new Model.Character();
        result.id = character.id;
        result.name = character.name;
        result.level = character.level;
        result.attack = character.attack;
        result.defense = character.defense;
        result.hp = character.hp;
        result.x = x;
        result.y = y;
        result.equipment = character.equipment;
        result.mount = character.mount;
        result.characterClass = character.characterClass;
        result.actionRange = character.actionRange;
        result.actionState = character.actionState;
        return result;
    }

    private Model.Character[] UnionCharacters(Model.Character[] character, int[] characterPlayer, int op = -1)
    {
        int length = 0;
        for (int i = 0; i < character.Length; i++)
        {
            if (op == -1 || characterPlayer[i] == op)
            {
                length++;
            }
        }

        var result = new Model.Character[length];
        length = 0;
        for (int i = 0; i < character.Length; i++)
        {
            if (op == -1 || characterPlayer[i] == op)
            {
                result[length] = character[i];
                length++;
            }
        }

        return result;
    }

    private Model.Structure[] UnionStructures(Model.Structure[] structure,
        int[] structureCharacterCount, Character[] structureCharacters, int[] structuresPlayer, int op = -1)
    {
        int length = 0;
        for (int i = 0; i < structure.Length; i++)
        {
            if (op == -1 || structuresPlayer[i] == op)
            {
                length++;
            }
        }

        var result = new Model.Structure[length];
        var tmp = 0;
        length = 0;
        for (int i = 0; i < structure.Length; i++)
        {
            if (op == -1 || structuresPlayer[i] == op)
            {
                var character = new Model.Character[structureCharacterCount[i]];
                for (int j = 0; j < structureCharacterCount[i]; j++)
                {
                    character[j] = ChangeCharacterModel(structureCharacters[tmp]);
                    tmp++;
                }

                result[length] = structure[i];
                result[length].characters = character;
                length++;
            }
        }

        return result;
    }

    public async Task LoadArchive(string path, IProgress<ProgressReportModel> progress = null)
    {
        if (!File.Exists(path))
        {
            Debug.Log("NOT FIND ARCHIVE");
            return;
        }

        Debug.Log("-------LOAD-------");
        ProgressReportModel report = new ProgressReportModel();
        StreamReader sr = new StreamReader(path);
        string JsonString = await sr.ReadToEndAsync();
        sr.Close();

        report.ProgressValue = 15;
        if (progress is not null)
            progress.Report(report);

        Sl a = new Sl();
        Sl archive = JsonUtility.FromJson<Sl>(JsonString);
        Debug.Log("-------LOAD success-------");

        Debug.Log("------- create Game object ---------");
        var game = new Game();
        game.id = archive.gameID;
        game.round = archive.round;
        game.currentPlayer = archive.currentPlayer;

        var structures1 = UnionStructures(archive.structures, archive.structureCharacterCount,
            archive.structureCharacters, archive.structurePlayer, archive.player1.id);
        var structures2 = UnionStructures(archive.structures, archive.structureCharacterCount,
            archive.structureCharacters, archive.structurePlayer, archive.player2.id);
        var characters1 = UnionCharacters(archive.characters, archive.characterPlayer, archive.player1.id);
        var characters2 = UnionCharacters(archive.characters, archive.characterPlayer, archive.player2.id);

        game.player1 = ChangePlayerModel(archive.player1, characters1, structures1);
        game.player2 = ChangePlayerModel(archive.player2, characters2, structures2);
        game.shop = game.currentPlayer ? archive.player2.shop : archive.player1.shop;

        var allStructure = UnionStructures(archive.structures, archive.structureCharacterCount,
            archive.structureCharacters, archive.structurePlayer);
        game.structures = allStructure;
        int[,] map = new int[MapSize, MapSize];
        for (int i = 0; i < MapSize; i++)
        {
            for (int j = 0; j < MapSize; j++)
            {
                map[i, j] = archive.map[i * MapSize + j];
            }
        }

        game.map = map;
        Debug.Log("------- create Game object success ---------");

        var res = await api.PUT(
            url: api.Copy,
            param: new Dictionary<string, string>
            {
                { "game", JsonUtility.ToJson(game) },
            }
        );
        game = GetModel<Model.Game>(res);
        if (game == null) return;
        InitiateData(game);
        SetMap(game.map);
        foreach (var structure in game.structures)
        {
            UpdateStructureAttribute(structure, false);
            GridController.Instance.AddVillageAndRelic(new Vector3Int(structure.x - 8, structure.y - 8, 0),
                structure.structureClass == StructureClass.VILLAGE ? 0 : 1);
        }

        foreach (var structure in game.player1.structures)
        {
            UpdateStructureAttribute(structure, false);
            structures[structure.x * MapSize + structure.y].player = player1;
        }

        foreach (var structure in game.player2.structures)
        {
            UpdateStructureAttribute(structure, false);
            structures[structure.x * MapSize + structure.y].player = player2;
        }

        for (int i = 0; i < MapSize; i++)
        {
            for (int j = 0; j < MapSize; j++)
            {
                GridController.Instance.SetStructure(new Vector3Int(i, j));
            }
        }

        characters = new Character[MapSize * MapSize];
        foreach (var character in game.player1.characters)
        {
            UpdateCharacterAttribute(character, false);
            characters[character.x * MapSize + character.y].player = player1;
            GridController.Instance.CreateCharacter(new Vector3Int(character.x, character.y));
        }

        foreach (var character in game.player2.characters)
        {
            UpdateCharacterAttribute(character, false);
            characters[character.x * MapSize + character.y].player = player2;
            GridController.Instance.CreateCharacter(new Vector3Int(character.x, character.y));
        }

        player1.shop = archive.player1.shop;
        player2.shop = archive.player2.shop;
        report.ProgressValue = 100;
        if (progress is not null)
            progress.Report(report);
    }

    private Sl CreateArchiveObject()
    {
        var archive = new Sl();

        archive.gameID = gameID;
        archive.round = round;
        archive.currentPlayer = (player2.id == currentPlayer.id);
        archive.player1 = player1;
        archive.player2 = player2;
        archive.map = _realMap;

        var charactersLength = 0;
        var structuresLength = 0;
        var structuresCharactersCount = 0;
        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i] != null && characters[i].id != 0)
            {
                charactersLength++;
            }
        }

        for (int i = 0; i < structures.Length; i++)
        {
            if (structures[i] != null && structures[i].id != 0)
            {
                structuresLength++;
                if (structures[i].characters != null)
                {
                    structuresCharactersCount += structures[i].characters.Length;
                }
            }
        }

        Model.Character[] saveCharacters = new Model.Character[charactersLength];
        Model.Structure[] saveStructures = new Model.Structure[structuresLength];
        archive.characterPlayer = new int[charactersLength];
        archive.structurePlayer = new int[structuresLength];
        archive.structureCharacterCount = new int[structuresLength];
        archive.structureCharacters = new Character[structuresCharactersCount];
        structuresCharactersCount = 0;
        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i] != null && characters[i].id != 0)
            {
                charactersLength--;
                saveCharacters[charactersLength] = new Model.Character();
                saveCharacters[charactersLength].id = characters[i].id;
                saveCharacters[charactersLength].hp = characters[i].hp;
                saveCharacters[charactersLength].name = characters[i].name;
                saveCharacters[charactersLength].level = characters[i].level;
                saveCharacters[charactersLength].mount = characters[i].mount;
                saveCharacters[charactersLength].attack = characters[i].attack;
                saveCharacters[charactersLength].defense = characters[i].defense;
                saveCharacters[charactersLength].equipment = characters[i].equipment;
                saveCharacters[charactersLength].actionRange = characters[i].actionRange;
                saveCharacters[charactersLength].actionState = characters[i].actionState;
                saveCharacters[charactersLength].characterClass = characters[i].characterClass;

                saveCharacters[charactersLength].x = i / MapSize;
                saveCharacters[charactersLength].y = i % MapSize;

                archive.characterPlayer[charactersLength] = characters[i].id;
            }
        }

        for (int i = 0; i < structures.Length; i++)
        {
            if (structures[i] != null && structures[i].id != 0)
            {
                structuresLength--;
                saveStructures[structuresLength] = new Model.Structure();
                saveStructures[structuresLength].id = structures[i].id;
                saveStructures[structuresLength].structureClass = structures[i].structureClass;
                saveStructures[structuresLength].level = structures[i].level;
                saveStructures[structuresLength].hp = structures[i].hp;
                saveStructures[structuresLength].remainingRound = structures[i].remainingRound;
                saveStructures[structuresLength].value = structures[i].value;
                saveStructures[structuresLength].characters = null;
                saveStructures[structuresLength].x = i / MapSize;
                saveStructures[structuresLength].y = i % MapSize;

                archive.structureCharacterCount[structuresLength] = 0;
                if (structures[i].characters != null)
                {
                    archive.structureCharacterCount[structuresLength] = structures[i].characters.Length;
                    for (int j = 0; j < structures[i].characters.Length; j++)
                    {
                        archive.structureCharacters[structuresCharactersCount] = structures[i].characters[j];
                        structuresCharactersCount++;
                    }
                }

                archive.structurePlayer[structuresLength] = structures[i].id;
            }
        }

        archive.characters = saveCharacters;
        archive.structures = saveStructures;
        return archive;
    }

    public List<Character> CharactersOfPlayer(int id)
    {
        List<Character> ch = new List<Character>();
        for (int i = 0; i < MapSize * MapSize; i++)
        {
            Character character = characters[i];
            if (character != null && character.player != null && id == character.player.id)
            {
                ch.Add(character);
            }
        }

        return ch;
    }

    public List<Vector3Int> GetCharacterPosByPlayer(int id)
    {
        List<Vector3Int> positions = new List<Vector3Int>();
        for (int i = 0; i < MapSize * MapSize; i++)
        {
            if (characters[i] != null && characters[i].player != null && characters[i].player.id == id)
            {
                Vector3Int pos = new Vector3Int(i / MapSize, i % MapSize);
                positions.Add(pos);
            }
        }

        return positions;
    }

    public List<Vector3Int> GetStructurePosByPlayer(int id)
    {
        List<Vector3Int> positions = new List<Vector3Int>();
        for (int i = 0; i < MapSize * MapSize; i++)
        {
            if (structures[i] != null && structures[i].player != null && structures[i].player.id == id)
            {
                Vector3Int pos = new Vector3Int(i / MapSize, i % MapSize);
                positions.Add(pos);
            }
        }

        return positions;
    }

    public List<Structure> GetStructureByPlayer(int id)
    {
        List<Structure> structs = new List<Structure>();
        foreach (Structure structure in structures)
        {
            if (structure != null && structure.player != null && id == structure.player.id)
            {
                structs.Add(structure);
            }
        }

        return structs;
    }

    public int GetMapByPosition(Vector3Int vector3Int)
    {
        return _map[vector3Int.x * MapSize + vector3Int.y];
    }

    public Character GetCharacterByPosition(Vector3Int vector3Int)
    {
        return characters[vector3Int.x * MapSize + vector3Int.y];
    }

    public Structure GetStructureByPosition(Vector3Int vector3Int)
    {
        return structures[vector3Int.x * MapSize + vector3Int.y];
    }

    public async Task UpdatePassword(string username, string oldPassword, string newPassword)
    {
        var res = await api.POST(
            url: api.UpdatePassword,
            param: new Dictionary<string, string>
            {
                { "username", username },
                { "old_password", oldPassword },
                { "new_password", newPassword }
            });

        var account = GetModel<Account>(res);
        if (account == null)
        {
            EditorUtility.DisplayDialog("Errors", "Update Password failed", "ok");
        }
        else
        {
            EditorUtility.DisplayDialog("Congratulations", "Update Password successfully", "ok");
        }
    }

    public async Task Play(string username1, string username2, IProgress<ProgressReportModel> progress = null)
    {
        var report = new ProgressReportModel();
        var res = await api.POST(
            url: api.Play,
            param: new Dictionary<string, string>
            {
                { "username1", username1 },
                { "username2", username2 }
            });
        report.ProgressValue = 15;
        if (progress is not null)
            progress.Report(report);
        var game = GetModel<Game>(res, progress);
        if (game == null)
        {
            return;
        }

        SetData(game, progress);
        report.ProgressValue = 100;
        if (progress is not null)
            progress.Report(report);
    }

    public async Task Update(int playerId)
    {
        var res = await api.PUT(
            url: api.Play,
            param: new Dictionary<string, string>
            {
                { "playerid", playerId.ToString() }
            });

        var game = GetModel<Game>(res);
        if (game == null)
        {
            return;
        }

        UpdateData(game);
    }

    public async Task StepBack(bool current)
    {
        var res = await api.DELETE(
            url: api.Back,
            param: new Dictionary<string, string>
            {
                { "playerid", currentPlayer.id.ToString() },
                { "current", current.ToString() }
            });

        var game = GetModel<Game>(res);
        if (game == null)
        {
            return;
        }

        InitiateData(game);
        foreach (var structure in game.structures)
            UpdateStructureAttribute(structure, false);
        foreach (var structure in game.player1.structures)
        {
            UpdateStructureAttribute(structure, false);
            structures[structure.x * MapSize + structure.y].player = player1;
        }

        foreach (var structure in game.player2.structures)
        {
            UpdateStructureAttribute(structure, false);
            structures[structure.x * MapSize + structure.y].player = player2;
        }

        for (int i = 0; i < MapSize; i++)
        {
            for (int j = 0; j < MapSize; j++)
            {
                GridController.Instance.SetStructure(new Vector3Int(i, j));
            }
        }

        for (int i = 0; i < MapSize; i++)
        {
            for (int j = 0; j < MapSize; j++)
            {
                if (GridController.Instance.characterObjects[i * MapSize + j] != null)
                    GridController.Instance.DeleteCharacter(new Vector3Int(i, j));
            }
        }

        characters = new Character[MapSize * MapSize];
        foreach (var character in game.player1.characters)
        {
            UpdateCharacterAttribute(character, false);
            characters[character.x * MapSize + character.y].player = player1;
            GridController.Instance.CreateCharacter(new Vector3Int(character.x, character.y));
        }

        foreach (var character in game.player2.characters)
        {
            UpdateCharacterAttribute(character, false);
            characters[character.x * MapSize + character.y].player = player2;
            GridController.Instance.CreateCharacter(new Vector3Int(character.x, character.y));
        }
    }

    /* Player Controller */
    public async Task GetAll(int playerId)
    {
        var res = await api.GET(
            url: api.Player + "/" + currentPlayer.id,
            param: null
        );
        var player = GetModel<Model.Player>(res);
        if (player == null)
        {
            return;
        }

        currentPlayer.prosperityDegree = player.prosperityDegree;
        currentPlayer.peaceDegree = player.peaceDegree;
        currentPlayer.equipments = player.equipments;
        currentPlayer.mounts = player.mounts;
        currentPlayer.items = player.items;
        currentPlayer.stars = player.stars;
    }

    public async Task UpdateItem(Vector3Int position, int itemId)
    {
        if (GetCharacterByPosition(position) == null) return;
        var characterId = GetCharacterByPosition(position).id;
        var res = await api.PUT(
            url: api.Player + "/" + currentPlayer.id + "/item",
            param: new Dictionary<string, string>
            {
                { "characterid", characterId.ToString() },
                { "itemid", itemId.ToString() }
            });

        var character = GetModel<Model.Character>(res);
        if (character == null)
        {
            return;
        }

        UpdateCharacterAttribute(character, false);
        for (int i = 0; i < currentPlayer.items.Length; i++)
        {
            if (currentPlayer.items[i].id == itemId)
            {
                Item[] newItems = new Item[currentPlayer.items.Length - 1];
                for (int j = 0; j < i; j++) newItems[j] = currentPlayer.items[j];
                for (int j = i; j < newItems.Length; j++) newItems[j] = currentPlayer.items[j + 1];
                currentPlayer.items = newItems;
                //currentPlayer.items[i] = null;
                break;
            }
        }
    }

    public async Task UpdateEquipment(Vector3Int position, int equipmentId, bool off)
    {
        if (GetCharacterByPosition(position) == null) return;
        var characterId = GetCharacterByPosition(position).id;
        var res = await api.PUT(
            url: api.Player + "/" + currentPlayer.id + "/equip",
            param: new Dictionary<string, string>
            {
                { "characterid", characterId.ToString() },
                { "equipmentid", equipmentId.ToString() },
                { "off", off.ToString() }
            }
        );
        var character = GetModel<Model.Character>(res);
        if (character == null)
        {
            return;
        }

        UpdateCharacterAttribute(character, false);
        if (off)
        {
        }
        else
        {
            //wear equipment
            for (int i = 0; i < currentPlayer.equipments.Length; i++)
            {
                if (currentPlayer.equipments[i].id == equipmentId)
                {
                    Equipment[] newEquipments = new Equipment[currentPlayer.equipments.Length - 1];
                    for (int j = 0; j < i; j++) newEquipments[j] = currentPlayer.equipments[j];
                    for (int j = i; j < newEquipments.Length; j++) newEquipments[j] = currentPlayer.equipments[j + 1];
                    currentPlayer.equipments = newEquipments;
                    //currentPlayer.equipments[i] = null;
                    break;
                }
            }
        }
    }

    public async Task UpdateMount(Vector3Int position, int mountId, bool off)
    {
        if (GetCharacterByPosition(position) == null) return;
        var characterId = GetCharacterByPosition(position).id;
        var res = await api.PUT(
            url: api.Player + "/" + currentPlayer.id + "/mount",
            param: new Dictionary<string, string>
            {
                { "characterid", characterId.ToString() },
                { "mountid", mountId.ToString() },
                { "off", off.ToString() }
            }
        );
        var character = GetModel<Model.Character>(res);
        if (character == null)
        {
            return;
        }

        UpdateCharacterAttribute(character, false);
        if (off)
        {
        }
        else
        {
            //wear equipment
            for (int i = 0; i < currentPlayer.mounts.Length; i++)
            {
                if (currentPlayer.mounts[i].id == mountId)
                {
                    Mount[] newMount = new Mount[currentPlayer.mounts.Length - 1];
                    for (int j = 0; j < i; j++) newMount[j] = currentPlayer.mounts[j];
                    for (int j = i; j < newMount.Length; j++) newMount[j] = currentPlayer.mounts[j + 1];
                    currentPlayer.mounts = newMount;
                    //currentPlayer.mounts[i] = null;
                    break;
                }
            }
        }
    }


    public async Task GetTechnologies()
    {
        var res = await api.GET(
            url: api.Player + "/" + currentPlayer.id + "/tech",
            param: null);
        var tech = GetModel<int[,]>(res);
        if (tech == null) return;
        for (int i = 0; i < tech.GetLength(0); i++)
        {
            for (int j = 0; j < tech.GetLength(1); j++)
            {
                currentPlayer.tech[i * tech.GetLength(1) + j] = tech[i, j];
            }
        }
    }

    public async Task GetEquipments()
    {
        var res = await api.GET(
            url: api.Player + "/" + currentPlayer.id + "/equip",
            param: null);
        var equipments = GetModel<Equipment[]>(res);
        if (equipments == null) return;
        currentPlayer.equipments = equipments;
    }

    public async Task GetMount()
    {
        var res = await api.GET(
            url: api.Player + "/" + currentPlayer.id + "/mount",
            param: null
        );
        var mounts = GetModel<Mount[]>(res);
        if (mounts == null) return;
        currentPlayer.mounts = mounts;
    }

    public async Task GetItem()
    {
        var res = await api.GET(
            url: api.Player + "/" + currentPlayer.id + "/item",
            param: null
        );
        var items = GetModel<Model.Item[]>(res);
        if (items == null) return;
        currentPlayer.items = items;
    }


    public async Task BuyEquipments(int shopid)
    {
        var res = await api.POST(
            url: api.Player + "/" + currentPlayer.id + "/equip",
            param: new Dictionary<string, string>
            {
                { "shopid", shopid.ToString() }
            }
        );
        var equipment = GetModel<Model.Equipment>(res);
        if (equipment == null) return;
        currentPlayer.stars -= 7;
        var newequip = new Equipment[currentPlayer.equipments.Length + 1];
        for (int i = 0; i < newequip.Length - 1; i++) newequip[i] = currentPlayer.equipments[i];
        newequip[newequip.Length - 1] = equipment;
        currentPlayer.equipments = newequip;
        //currentPlayer.equipments.AddRange(new Equipment[] { equipment });
        for (int i = 0; i < currentPlayer.shop.equipments.Length; i++)
        {
            if (currentPlayer.shop.index[0, i] == shopid)
            {
                currentPlayer.shop.equipments[i] = null;
                break;
            }
        }
    }

    public async Task BuyMounts(int shopid)
    {
        var res = await api.POST(
            url: api.Player + "/" + currentPlayer.id + "/mount",
            param: new Dictionary<string, string>
            {
                { "shopid", shopid.ToString() }
            }
        );
        var mount = GetModel<Mount>(res);
        if (mount == null) return;
        currentPlayer.stars -= 7;
        var newmount = new Mount[currentPlayer.mounts.Length + 1];
        for (int i = 0; i < newmount.Length - 1; i++) newmount[i] = currentPlayer.mounts[i];
        newmount[newmount.Length - 1] = mount;
        currentPlayer.mounts = newmount;
        //currentPlayer.mounts.AddRange(new Mount[] { mount });
        for (int i = 0; i < currentPlayer.shop.mounts.Length; i++)
        {
            if (currentPlayer.shop.index[2, i] == shopid)
            {
                currentPlayer.shop.mounts[i] = null;
                break;
            }
        }
    }


    public async Task BuyItems(int shopid)
    {
        var res = await api.POST(
            url: api.Player + "/" + currentPlayer.id + "/item",
            param: new Dictionary<string, string>
            {
                { "shopid", shopid.ToString() }
            }
        );
        var item = GetModel<Model.Item>(res);
        if (item == null) return;
        //currentPlayer.items.AddRange(new Model.Item[] { item });
        currentPlayer.stars -= 7;
        var newitem = new Item[currentPlayer.items.Length + 1];
        for (int i = 0; i < newitem.Length - 1; i++) newitem[i] = currentPlayer.items[i];
        newitem[newitem.Length - 1] = item;
        currentPlayer.items = newitem;
        for (int i = 0; i < currentPlayer.shop.items.Length; i++)
        {
            if (currentPlayer.shop.index[1, i] == shopid)
            {
                currentPlayer.shop.items[i] = null;
                break;
            }
        }
    }

    /* Character Controller*/


    public async Task GetCharacter(int id)
    {
        var res = await api.GET(
            url: api.Character + "/" + id,
            param: null
        );
        var character = GetModel<Model.Character>(res);
        if (character == null) return;
        UpdateCharacterAttribute(character, true);
    }

    public async Task DismissCharacter(Vector3Int position)
    {
        var characterOld = GetCharacterByPosition(position);
        if (characterOld == null) return;
        var res = await api.PUT(
            url: api.Character + "/" + characterOld + "/dismiss",
            param: null
        );
        var character = GetModel<Model.Character>(res);
        if (character == null) return;
        characters[character.x * MapSize + character.y] = null;
    }

    public async Task MoveCharacter(Vector3Int oldPosition, Vector3Int newPosition)
    {
        var characterOld = GetCharacterByPosition(oldPosition);
        if (characterOld == null) return;
        var res = await api.PUT(
            url: api.Character + "/" + characterOld.id + "/move",
            param: new Dictionary<string, string>
            {
                { "x", newPosition.x.ToString() },
                { "y", newPosition.y.ToString() }
            }
        );
        var character = GetModel<Model.Character>(res);
        if (character == null) return;
        characters[oldPosition.x * MapSize + oldPosition.y] = null;
        UpdateCharacterAttribute(character, true);
    }

    public async Task AttackCharacter(Vector3Int oldPosition, Vector3Int newPosition)
    {
        var characterAttack = GetCharacterByPosition(oldPosition);
        var characterAttacked = GetCharacterByPosition(newPosition);
        var res = await api.PUT(
            url: api.Character + "/" + characterAttack.id + "/char",
            param: new Dictionary<string, string>
            {
                { "attackid", characterAttacked.id.ToString() }
            }
        );
        var character = GetModel<Model.Character>(res);
        if (character == null) return;
        UpdateCharacterAttribute(character, false);

        characters[oldPosition.x * MapSize + oldPosition.y].actionState = 2;

        if (character.hp <= 0)
        {
            GetStructure(new Vector3Int(character.x, character.y));
        }
    }

    public async Task AttackStructure(Vector3Int oldPosition, Vector3Int newPosition)
    {
        var characterAttack = GetCharacterByPosition(oldPosition);
        var structureAttacked = GetStructureByPosition(newPosition);
        var res = await api.PUT(
            url: api.Character + "/" + characterAttack.id + "/structure",
            param: new Dictionary<string, string>
            {
                { "attackid", structureAttacked.id.ToString() }
            }
        );
        var structure = GetModel<Model.Structure>(res);
        if (structure == null) return;
        if (structureAttacked.hp - characterAttack.attack != structure.hp)
        {
            UpdateStructurePlayer(structure);
        }

        UpdateStructureAttribute(structure, false);
        characters[oldPosition.x * MapSize + oldPosition.y].actionState = 2;
    }

    /*structure controller*/
    public async Task GetStructure(Vector3Int position)
    {
        var structureOld = GetStructureByPosition(position);
        var res = await api.GET(
            url: api.Structure + "/" + structureOld.id,
            param: null
        );
        var structure = GetModel<Model.Structure>(res);
        if (structure == null) return;
        UpdateStructureAttribute(structure, false);
    }

    public async Task BuyCharacters(Vector3Int position, int id, int x, int y, int type)
    {
        var structureOld = GetStructureByPosition(position);
        var res = await api.POST(
            url: api.Structure + "/" + structureOld.id + "/char",
            param: new Dictionary<string, string>
            {
                { "playerid", currentPlayer.id.ToString() },
                { "id", id.ToString() },
                { "x", x.ToString() },
                { "y", y.ToString() },
                { "type", type.ToString() }
            }
        );
        var structure = GetModel<Model.Structure>(res);
        if (structure == null) return;
        UpdateStructureAttribute(structure, false);
        await GetCharacter(id);
        currentPlayer.stars -= 3;
    }

    public async Task UpdateCharacter(Vector3Int position, int option)
    {
        var structureOld = GetStructureByPosition(position);
        var character = GetCharacterByPosition(position);
        if (structureOld == null) return;
        if (character == null) return;
        var res = await api.PUT(
            url: api.Structure + "/" + structureOld.id + "/camp",
            param: new Dictionary<string, string>
            {
                { "characterid", character.id.ToString() },
                { "option", option.ToString() }
            }
        );
        var structure = GetModel<Model.Structure>(res);
        if (structure == null) return;
        UpdateStructureAttribute(structure, false);
        characters[position.x * MapSize + position.y].actionState = 2;
    }

    public async Task EarnStars(Vector3Int position)
    {
        var structureOld = GetStructureByPosition(position);
        var character = GetCharacterByPosition(position);
        if (structureOld == null) return;
        if (character == null) return;
        var res = await api.PUT(
            url: api.Structure + "/" + structureOld.id + "/market",
            param: new Dictionary<string, string>
            {
                { "characterid", character.id.ToString() },
            }
        );
        var structure = GetModel<Model.Structure>(res);
        if (structure == null) return;
        UpdateStructureAttribute(structure, false);
        characters[position.x * MapSize + position.y].actionState = 2;
    }

    public async Task UpdateTechnologies(Vector3Int position, int option)
    {
        var structureOld = GetStructureByPosition(position);
        var character = GetCharacterByPosition(position);
        if (structureOld == null) return;
        if (character == null) return;
        var res = await api.PUT(
            url: api.Structure + "/" + structureOld.id + "/insititude",
            param: new Dictionary<string, string>
            {
                { "characterid", character.id.ToString() },
                { "option", option.ToString() }
            }
        );
        var structure = GetModel<Model.Structure>(res);
        if (structure == null) return;
        if (option == 0) currentPlayer.stars -= 0;
        else if (option <= 3) currentPlayer.stars -= 4;
        else if (option <= 9) currentPlayer.stars -= 10;
        else currentPlayer.stars -= 20;

        UpdateStructureAttribute(structure, false);
        characters[position.x * MapSize + position.y].actionState = 2;
    }

    public async Task UpdateStructure(Vector3Int position, int option)
    {
        var structureOld = GetStructureByPosition(position);
        if (structureOld == null) return;
        var res = await api.PUT(
            url: api.Structure + "/" + structureOld.id + "/upd",
            param: new Dictionary<string, string>
            {
                { "option", option.ToString() }
            }
        );
        var structure = GetModel<Model.Structure>(res);
        if (structure == null) return;
        currentPlayer.stars -= structureOld.level * 10;
        UpdateStructureAttribute(structure, false);
    }

    public async Task HealStructure(Vector3Int position)
    {
        var structureOld = GetStructureByPosition(position);
        if (structureOld == null) return;
        var hpOld = structureOld.hp;
        var res = await api.PUT(
            url: api.Structure + "/" + structureOld.id + "/heal",
            param: null
        );
        var structure = GetModel<Model.Structure>(res);
        if (structure == null) return;
        var hpNew = structure.hp;
        UpdateStructureAttribute(structure, false);
        currentPlayer.stars -= hpNew - hpOld;
    }

    private T GetModel<T>(HttpResponseMessage res, IProgress<ProgressReportModel> progress = null)
    {
        if (res == null)
        {
            EditorUtility.DisplayDialog("Unknown errors", "Check your network", "ok");
            GameManager.Instance.error = true;
            return default;
        }

        if (res.StatusCode != HttpStatusCode.OK)
        {
            EditorUtility.DisplayDialog("Errors", res.ReasonPhrase, "ok");
            GameManager.Instance.error = true;
            return default;
        }

        var content = res.Content.ReadAsStringAsync().Result;
        var model = JsonConvert.DeserializeObject<Model<T>>(content);

        if (model == null)
        {
            GameManager.Instance.error = true;
            return default;
        }

        if (model.code != 200)
        {
            EditorUtility.DisplayDialog("Bad Request", model.msg, "ok");
            GameManager.Instance.error = true;
            return default;
        }

        if (progress is not null)
        {
            ProgressReportModel report = new ProgressReportModel();
            report.ProgressValue = 30;
            progress.Report(report);
        }

        return model.data;
    }

    public int CheckCharacterSide(Character character)
    {
        if (character.player == null) return 0;
        if (character.player.id == player1.id) return -1;
        return 1;
    }

    public int CheckStructureSide(Structure structure)
    {
        if (structure.player == null) return 0;
        if (structure.player.id == player1.id) return -1;
        return 1;
    }

    private void SetData(Game game, IProgress<ProgressReportModel> progress = null)
    {
        ProgressReportModel report = new ProgressReportModel();
        gameID = game.id;
        InitiateData(game);
        if (progress is not null)
        {
            report.ProgressValue = 50;
            progress.Report(report);
        }

        SetMap(game.map);
        if (progress is not null)
        {
            report.ProgressValue = 65;
            progress.Report(report);
        }

        SetStructure(game.structures, game.player1.structures[0], game.player2.structures[0]);
        if (progress is not null)
        {
            report.ProgressValue = 80;
            progress.Report(report);
        }

        SetCharacter(game.player1.characters[0], game.player2.characters[0]);
        if (progress is not null)
        {
            report.ProgressValue = 90;
            progress.Report(report);
        }
    }

    private void InitiateData(Game game)
    {
        round = game.round / 2;
        SetPlayer(player1, game.player1);
        SetPlayer(player2, game.player2);
        currentPlayer = game.currentPlayer ? player2 : player1;
        if (AI.player == null)
            AI.player = game.currentPlayer ? player1 : player2;
    }

    private void UpdateData(Game game)
    {
        InitiateData(game);
        foreach (var structure in game.structures)
            UpdateStructureAttribute(structure, false);
        if (game.currentPlayer)
        {
            foreach (var character in game.player1.characters)
                UpdateCharacterAttribute(character, false);
            foreach (var structure in game.player1.structures)
                UpdateStructureAttribute(structure, false);
        }
        else
        {
            foreach (var character in game.player2.characters)
                UpdateCharacterAttribute(character, false);
            foreach (var structure in game.player2.structures)
                UpdateStructureAttribute(structure, false);
        }
    }

    private void UpdateCharacterAttribute(Model.Character character, bool flag)
    {
        if (character.hp <= 0)
        {
            characters[character.x * MapSize + character.y] = null;
            return;
        }

        if (characters[character.x * MapSize + character.y] == null)
        {
            characters[character.x * MapSize + character.y] = new Character();
        }


        characters[character.x * MapSize + character.y].id = character.id;
        characters[character.x * MapSize + character.y].name = character.name;
        characters[character.x * MapSize + character.y].characterClass = character.characterClass;
        characters[character.x * MapSize + character.y].actionRange = character.actionRange;
        characters[character.x * MapSize + character.y].actionState = character.actionState;
        characters[character.x * MapSize + character.y].attack = character.attack;
        characters[character.x * MapSize + character.y].defense = character.defense;
        characters[character.x * MapSize + character.y].hp = character.hp;
        characters[character.x * MapSize + character.y].level = character.level;
        if (character.equipment == null || character.equipment.id == 0)
        {
            characters[character.x * MapSize + character.y].equipment = null;
        }
        else
        {
            characters[character.x * MapSize + character.y].equipment = character.equipment;
        }

        if (character.mount == null || character.mount.id == 0)
        {
            characters[character.x * MapSize + character.y].mount = null;
        }
        else
        {
            characters[character.x * MapSize + character.y].mount = character.mount;
        }

        if (flag)
        {
            characters[character.x * MapSize + character.y].player = currentPlayer;
        }
    }

    private void SetCharacter(Model.Character character1, Model.Character character2)
    {
        UpdateCharacterAttribute(character1, false);
        characters[character1.x * MapSize + character1.y].player = player1;
        UpdateCharacterAttribute(character2, false);
        characters[character2.x * MapSize + character2.y].player = player2;
    }

    private void UpdateStructureAttribute(Model.Structure structure, bool startIndex)
    {
        if (structures[structure.x * MapSize + structure.y] == null)
        {
            structures[structure.x * MapSize + structure.y] = new Structure();
        }

        structures[structure.x * MapSize + structure.y].id = structure.id;
        structures[structure.x * MapSize + structure.y].structureClass = structure.structureClass;
        structures[structure.x * MapSize + structure.y].level = structure.level;
        structures[structure.x * MapSize + structure.y].hp = structure.hp;
        structures[structure.x * MapSize + structure.y].remainingRound = structure.remainingRound;
        structures[structure.x * MapSize + structure.y].value = structure.value;
        if (structure.characters == null)
        {
            return;
        }

        if (startIndex)
        {
            structures[structure.x * MapSize + structure.y].startIndex = structure.characters[0].id;
        }

        structures[structure.x * MapSize + structure.y].characters = new Character[3];

        foreach (var character in structure.characters)
        {
            for (var i = 0; i < 3; i++)
            {
                if (character.id == structures[structure.x * MapSize + structure.y].startIndex + i)
                {
                    structures[structure.x * MapSize + structure.y].characters[i] = new Character();
                    structures[structure.x * MapSize + structure.y].characters[i].id = character.id;
                    structures[structure.x * MapSize + structure.y].characters[i].name = character.name;
                    structures[structure.x * MapSize + structure.y].characters[i].actionRange =
                        character.actionRange;
                    structures[structure.x * MapSize + structure.y].characters[i].attack = character.attack;
                    structures[structure.x * MapSize + structure.y].characters[i].defense = character.defense;
                    structures[structure.x * MapSize + structure.y].characters[i].hp = character.hp;
                    structures[structure.x * MapSize + structure.y].characters[i].level = character.level;
                }
            }
        }
    }

    private void UpdateStructurePlayer(Model.Structure structure)
    {
        if (structures[structure.x * MapSize + structure.y] == null) return;
        structures[structure.x * MapSize + structure.y].player = currentPlayer;
    }

    private void SetStructure(Model.Structure[] structure, Model.Structure structure1, Model.Structure structure2)
    {
        foreach (var s in structure)
        {
            UpdateStructureAttribute(s, true);
            GridController.Instance.AddVillageAndRelic(new Vector3Int(s.x - 8, s.y - 8, 0),
                s.structureClass == StructureClass.VILLAGE ? 0 : 1);
        }

        UpdateStructureAttribute(structure1, false);
        UpdateStructureAttribute(structure2, false);
        structures[structure1.x * MapSize + structure1.y].player = player1;
        structures[structure2.x * MapSize + structure2.y].player = player2;
        GridController.Instance.SetStructure(new Vector3Int(structure1.x, structure1.y, 0));
        GridController.Instance.SetStructure(new Vector3Int(structure2.x, structure2.y, 0));
    }

    private void SetPlayer(Player destinationPlayer, Model.Player sourcePlayer)
    {
        destinationPlayer.id = sourcePlayer.id;
        destinationPlayer.stars = sourcePlayer.stars;
        destinationPlayer.prosperityDegree = sourcePlayer.prosperityDegree;
        destinationPlayer.peaceDegree = sourcePlayer.peaceDegree;
        destinationPlayer.equipments = sourcePlayer.equipments;
        destinationPlayer.mounts = sourcePlayer.mounts;
        destinationPlayer.items = sourcePlayer.items;
        destinationPlayer.tech = new int[2 * sourcePlayer.technologyTree.GetLength(1)];
        for (int i = 0; i < sourcePlayer.technologyTree.GetLength(0); i++)
        {
            for (int j = 0; j < sourcePlayer.technologyTree.GetLength(1); j++)
            {
                destinationPlayer.tech[i * sourcePlayer.technologyTree.GetLength(1) + j] =
                    sourcePlayer.technologyTree[i, j];
            }
        }
    }

    private static bool CheckBound(int x, int y)
    {
        return !(x < 0 || x >= MapSize || y < 0 || y >= MapSize);
    }

    private int[,] ColorTree(int[,] sourceMap)
    {
        var random = new Random();
        var treeColor = new int[MapSize, MapSize];
        for (int i = 0; i < MapSize; i++)
        {
            for (int j = 0; j < MapSize; j++)
            {
                if (sourceMap[i, j] == 3 && treeColor[i, j] == 0)
                {
                    //tree
                    var result = new List<Vector3Int>();
                    treeColor[i, j] = random.Next(3) + 1;
                    result.Add(new Vector3Int(i, j));
                    for (var k = 0; k < result.Count; k++)
                    {
                        var x = result[k].x;
                        var y = result[k].y;
                        var op = 1;
                        if (y % 2 == 0) op = -1;

                        var dx = op;
                        var dy = 1;
                        if (CheckBound(x + dx, y + dy) && sourceMap[x + dx, y + dy] == 3 &&
                            treeColor[x + dx, y + dy] == 0)
                        {
                            treeColor[x + dx, y + dy] = treeColor[x, y];
                            result.Add(new Vector3Int(x + dx, y + dy));
                        }

                        dx = 1;
                        dy = 0;
                        if (CheckBound(x + dx, y + dy) && sourceMap[x + dx, y + dy] == 3 &&
                            treeColor[x + dx, y + dy] == 0)
                        {
                            treeColor[x + dx, y + dy] = treeColor[x, y];
                            result.Add(new Vector3Int(x + dx, y + dy));
                        }

                        dx = op;
                        dy = -1;
                        if (CheckBound(x + dx, y + dy) && sourceMap[x + dx, y + dy] == 3 &&
                            treeColor[x + dx, y + dy] == 0)
                        {
                            treeColor[x + dx, y + dy] = treeColor[x, y];
                            result.Add(new Vector3Int(x + dx, y + dy));
                        }


                        dx = 0;
                        dy = 1;
                        if (CheckBound(x + dx, y + dy) && sourceMap[x + dx, y + dy] == 3 &&
                            treeColor[x + dx, y + dy] == 0)
                        {
                            treeColor[x + dx, y + dy] = treeColor[x, y];
                            result.Add(new Vector3Int(x + dx, y + dy));
                        }

                        dx = 0;
                        dy = -1;
                        if (CheckBound(x + dx, y + dy) && sourceMap[x + dx, y + dy] == 3 &&
                            treeColor[x + dx, y + dy] == 0)
                        {
                            treeColor[x + dx, y + dy] = treeColor[x, y];
                            result.Add(new Vector3Int(x + dx, y + dy));
                        }

                        dx = -1;
                        dy = 0;
                        if (CheckBound(x + dx, y + dy) && sourceMap[x + dx, y + dy] == 3 &&
                            treeColor[x + dx, y + dy] == 0)
                        {
                            treeColor[x + dx, y + dy] = treeColor[x, y];
                            result.Add(new Vector3Int(x + dx, y + dy));
                        }
                    }
                }
            }
        }

        for (int i = 0; i < MapSize; i++)
        {
            for (int j = 0; j < MapSize; j++)
            {
                treeColor[i, j] = math.max(treeColor[i, j] - 1, 0);
            }
        }

        return treeColor;
    }

    private int[,] ColorLand(int[,] sourceMap)
    {
        var random = new Random();
        var landColor = new int[MapSize, MapSize];
        for (int t = 0; t < MapSize * MapSize / 5; t++)
        {
            var i = random.Next(MapSize);
            var j = random.Next(MapSize);
            if (CheckBound(i, j))
            {
                if (sourceMap[i, j] != 2 && landColor[i, j] == 0)
                {
                    var result = new List<Vector3Int>();
                    landColor[i, j] = random.Next(3) + 1;
                    result.Add(new Vector3Int(i, j));
                    var lim = random.Next(MapSize - 6) + 2;
                    for (var k = 0; k < result.Count && result.Count < lim; k++)
                    {
                        var x = result[k].x;
                        var y = result[k].y;
                        var op = 1;
                        if (y % 2 == 0) op = -1;

                        var dx = op;
                        var dy = 1;
                        if (CheckBound(x + dx, y + dy) && sourceMap[x + dx, y + dy] != 2 &&
                            landColor[x + dx, y + dy] == 0)
                        {
                            landColor[x + dx, y + dy] = landColor[x, y];
                            result.Add(new Vector3Int(x + dx, y + dy));
                        }

                        dx = 1;
                        dy = 0;
                        if (CheckBound(x + dx, y + dy) && sourceMap[x + dx, y + dy] != 2 &&
                            landColor[x + dx, y + dy] == 0)
                        {
                            landColor[x + dx, y + dy] = landColor[x, y];
                            result.Add(new Vector3Int(x + dx, y + dy));
                        }

                        dx = op;
                        dy = -1;
                        if (CheckBound(x + dx, y + dy) && sourceMap[x + dx, y + dy] != 2 &&
                            landColor[x + dx, y + dy] == 0)
                        {
                            landColor[x + dx, y + dy] = landColor[x, y];
                            result.Add(new Vector3Int(x + dx, y + dy));
                        }

                        dx = 0;
                        dy = 1;
                        if (CheckBound(x + dx, y + dy) && sourceMap[x + dx, y + dy] != 2 &&
                            landColor[x + dx, y + dy] == 0)
                        {
                            landColor[x + dx, y + dy] = landColor[x, y];
                            result.Add(new Vector3Int(x + dx, y + dy));
                        }

                        dx = 0;
                        dy = -1;
                        if (CheckBound(x + dx, y + dy) && sourceMap[x + dx, y + dy] != 2 &&
                            landColor[x + dx, y + dy] == 0)
                        {
                            landColor[x + dx, y + dy] = landColor[x, y];
                            result.Add(new Vector3Int(x + dx, y + dy));
                        }

                        dx = -1;
                        dy = 0;
                        if (CheckBound(x + dx, y + dy) && sourceMap[x + dx, y + dy] != 2 &&
                            landColor[x + dx, y + dy] == 0)
                        {
                            landColor[x + dx, y + dy] = landColor[x, y];
                            result.Add(new Vector3Int(x + dx, y + dy));
                        }
                    }
                }
            }
        }

        return landColor;
    }

    private void SetMap(int[,] sourceMap)
    {
        var treeColor = ColorTree(sourceMap);
        var landColor = ColorLand(sourceMap);
        for (int i = 0; i < sourceMap.GetLength(0); i++)
        {
            for (int j = 0; j < sourceMap.GetLength(1); j++)
            {
                GridController.Instance.SetMap(new Vector3Int(j, i), sourceMap[i, j], landColor[i, j], treeColor[i, j]);
                _map[i * MapSize + j] = sourceMap[i, j] == 3 ? 0 : sourceMap[i, j];
                _realMap[i * MapSize + j] = sourceMap[i, j];
            }
        }
    }
}


[Serializable]
public class Player
{
    public int id;
    public long stars;
    public int prosperityDegree;
    public int peaceDegree;
    public Equipment[] equipments;
    public Mount[] mounts;
    public Model.Item[] items;
    public Shop shop;
    public int[] tech;
}

[Serializable]
public class Character
{
    public int id;
    public Player player;
    public string name;
    public CharacterClass characterClass;
    public int actionState;
    public int actionRange;
    public int attack;
    public int defense;
    public int hp;
    public int level;
    public Equipment equipment;
    public Mount mount;
}

[Serializable]
public class Structure
{
    public int id;
    public Player player;
    public StructureClass structureClass;
    public int level;
    public int hp;
    public int remainingRound;
    public int value;
    public Character[] characters;
    public int startIndex;
}