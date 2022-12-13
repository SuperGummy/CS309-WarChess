using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using api = API.Service;
using Model;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

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
    public int[] _tech = new int [2 * TechSize];
    private int[] _map = new int[MapSize * MapSize];
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

    public List<Character> CharactersOfPlayer(int id)
    {
        List<Character> ch = new List<Character>();
        for (int i = 0; i < MapSize * MapSize; i++)
        {
            Character character = characters[i];
            if (character.player != null && id == character.player.id)
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
            if (characters[i].player != null && characters[i].player.id == id)
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
            if (structures[i].player != null && structures[i].player.id == id)
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
            if (id == structure.player.id)
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

    public async Task Register(string username, string password)
    {
        var res = await api.POST(
            url: api.Register,
            param: new Dictionary<string, string>
            {
                { "username", username },
                { "password", password }
            });

        var account = GetModel<Account>(res);
        if (account == null)
        {
            EditorUtility.DisplayDialog("Errors", "Register failed", "ok");
        }
        else
        {
            EditorUtility.DisplayDialog("Congratulations", "Register successfully", "ok");
        }
    }

    public async Task Login(string username, string password)
    {
        var res = await api.POST(
            url: api.Login,
            param: new Dictionary<string, string>
            {
                { "username", username },
                { "password", password }
            });

        if (res == null)
        {
            EditorUtility.DisplayDialog("Unknown errors", "Check your network", "ok");
            return;
        }

        if (res.StatusCode != HttpStatusCode.OK)
        {
            EditorUtility.DisplayDialog("Errors", res.ReasonPhrase, "ok");
            return;
        }

        var token = res.Content.ReadAsStringAsync().Result;

        if (token == null)
        {
            EditorUtility.DisplayDialog("Errors", "Login failed", "ok");
            return;
        }

        EditorUtility.DisplayDialog("Congratulations", "Login successfully", "ok");
        PlayerPrefs.SetString("username", username);
        PlayerPrefs.SetString("token", token);
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

    public async Task Play(string username1, string username2)
    {
        var res = await api.POST(
            url: api.Play,
            param: new Dictionary<string, string>
            {
                { "username1", username1 },
                { "username2", username2 }
            });

        var game = GetModel<Game>(res);
        if (game == null)
        {
            return;
        }

        SetData(game);
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
            UpdateStructureAttribute(structure);
        foreach (var structure in game.player1.structures)
        {
            UpdateStructureAttribute(structure);
            structures[structure.x * MapSize + structure.y].player = player1;
        }

        foreach (var structure in game.player2.structures)
        {
            UpdateStructureAttribute(structure);
            structures[structure.x * MapSize + structure.y].player = player2;
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
                currentPlayer.items[i] = null;
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
                    currentPlayer.equipments[i] = null;
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
                    currentPlayer.mounts[i] = null;
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
        var tech = GetModel<int[]>(res);
        if (tech == null) return;
        _tech = tech;
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
        currentPlayer.equipments.AddRange(new Equipment[] { equipment });
        for (int i = 0; i < currentPlayer.shop.equipments.Length; i++)
        {
            if (currentPlayer.shop.index[0, i] == shopid)
            {
                currentPlayer.equipments[i] = null;
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
        currentPlayer.mounts.AddRange(new Mount[] { mount });
        for (int i = 0; i < currentPlayer.shop.equipments.Length; i++)
        {
            if (currentPlayer.shop.index[2, i] == shopid)
            {
                currentPlayer.equipments[i] = null;
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
        currentPlayer.items.AddRange(new Model.Item[] { item });
        currentPlayer.stars -= 7;
        for (int i = 0; i < currentPlayer.shop.equipments.Length; i++)
        {
            if (currentPlayer.shop.index[1, i] == shopid)
            {
                currentPlayer.equipments[i] = null;
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

        UpdateStructureAttribute(structure);
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
        UpdateStructureAttribute(structure);
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
        UpdateStructureAttribute(structure);
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
        UpdateStructureAttribute(structure);
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
        UpdateStructureAttribute(structure);
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

        UpdateStructureAttribute(structure);
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
        UpdateStructureAttribute(structure);
        currentPlayer.stars -= structureOld.level * 10;
    }

    private T GetModel<T>(HttpResponseMessage res)
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

    private void SetData(Game game)
    {
        gameID = game.id;
        InitiateData(game);
        SetMap(game.map);
        SetStructure(game.structures, game.player1.structures[0], game.player2.structures[0]);
        SetCharacter(game.player1.characters[0], game.player2.characters[0]);
    }

    private void InitiateData(Game game)
    {
        round = game.round / 2;
        SetPlayer(player1, game.player1);
        SetPlayer(player2, game.player2);
        currentPlayer = game.currentPlayer ? player2 : player1;
        SetPlayerShop(game.currentPlayer ? player1 : player2, game.shop);
    }

    private void UpdateData(Game game)
    {
        InitiateData(game);
        foreach (var structure in game.structures)
            UpdateStructureAttribute(structure);
        if (game.currentPlayer)
        {
            foreach (var character in game.player1.characters)
                UpdateCharacterAttribute(character, false);
            foreach (var structure in game.player1.structures)
                UpdateStructureAttribute(structure);
        }
        else
        {
            foreach (var character in game.player2.characters)
                UpdateCharacterAttribute(character, false);
            foreach (var structure in game.player2.structures)
                UpdateStructureAttribute(structure);
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
        characters[character.x * MapSize + character.y].equipment = character.equipment;
        characters[character.x * MapSize + character.y].mount = character.mount;
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

    private void UpdateStructureAttribute(Model.Structure structure)
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

        structures[structure.x * MapSize + structure.y].characters = new Character[structure.characters.Length];
        for (var i = 0; i < structure.characters.Length; i++)
        {
            structures[structure.x * MapSize + structure.y].characters[i] = new Character();
            structures[structure.x * MapSize + structure.y].characters[i].id = structure.characters[i].id;
            structures[structure.x * MapSize + structure.y].characters[i].name = structure.characters[i].name;
            structures[structure.x * MapSize + structure.y].characters[i].actionRange =
                structure.characters[i].actionRange;
            structures[structure.x * MapSize + structure.y].characters[i].attack = structure.characters[i].attack;
            structures[structure.x * MapSize + structure.y].characters[i].defense = structure.characters[i].defense;
            structures[structure.x * MapSize + structure.y].characters[i].hp = structure.characters[i].hp;
            structures[structure.x * MapSize + structure.y].characters[i].level = structure.characters[i].level;
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
            UpdateStructureAttribute(s);
            GridController.Instance.AddVillageAndRelic(new Vector3Int(s.x - 8, s.y - 8, 0),
                s.structureClass == StructureClass.VILLAGE ? 0 : 1);
        }

        UpdateStructureAttribute(structure1);
        UpdateStructureAttribute(structure2);
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
    }

    private void SetPlayerShop(Player player, Shop shop)
    {
        if (shop == null) return;
        player.shop = shop;
    }

    private void SetMap(int[,] sourceMap)
    {
        for (int i = 0; i < sourceMap.GetLength(0); i++)
        {
            for (int j = 0; j < sourceMap.GetLength(1); j++)
            {
                _map[i * MapSize + j] = sourceMap[i, j] == 3 ? 0 : sourceMap[i, j];
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
}