using System;
using System.Collections.Generic;
using System.Linq;
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
    public int gameID;
    public int round;
    public Player currentPlayer;
    public Player player1;
    public Player player2;
    private int[,] _map = new int[MapSize, MapSize];
    private Character[,] _characters = new Character[MapSize, MapSize];
    private Structure[,] _structures = new Structure[MapSize, MapSize];

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

    public int GetMapByPosition(Vector3Int vector3Int)
    {
        return _map[vector3Int.x, vector3Int.y];
    }

    public Character GetCharacterByPosition(Vector3Int vector3Int)
    {
        return _characters[vector3Int.x, vector3Int.y];
    }

    public Structure GetStructureByPosition(Vector3Int vector3Int)
    {
        return _structures[vector3Int.x, vector3Int.y];
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
        foreach (var structure in game.structures)
        {
            UpdateStructureAttribute(structure);
        }
    }
    /* Player Controller */
    public async Task GetAll(int playerId)
    {
        var res = await api.GET(
            url: api.Player + "/" + currentPlayer.id,
            param: new Dictionary<string, string>
            {  
            });
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
    
    public async Task UpdateItem(int characterId, int itemId)
    {
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
        UpdateCharacterAttribute(character);
        for (int i = 0; i < currentPlayer.items.Length; i++)
        {
            if (currentPlayer.items[i].id == itemId)
            {
                currentPlayer.items[i] = null;
                break;
            }
        }
    }

    public async Task UpdateEquipment(int characterId, int equipmentId,bool off)
    {
        var res = await api.PUT(
            url:api.Player+"/"+currentPlayer.id+"/equip",
            param:new Dictionary<string, string>
            {
                {"characterid",characterId.ToString()},
                {"equipmentid",equipmentId.ToString()},
                {"off",off.ToString()}
            }
        );
        var character = GetModel<Model.Character>(res);
        if (character == null)
        {
            return;
        }

        UpdateCharacterAttribute(character);
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

    public async Task UpdateMount(int characterId, int mountId,bool off)
    {
        var res = await api.PUT(
            url:api.Player+"/"+currentPlayer.id+"/mount",
            param:new Dictionary<string, string>
            {
                {"characterid",characterId.ToString()},
                {"mountid",mountId.ToString()},
                {"off",off.ToString()}
            }
        );
        var character = GetModel<Model.Character>(res);
        if (character == null)
        {
            return;
        }

        UpdateCharacterAttribute(character);
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
            url: api.Player + "/" + currentPlayer.id+"/tech",
            param: new Dictionary<string, string>
            {  
            });
        // TODO: Model.Tech? int[][]?
        //var tech = GetModel<int[,]>(res);

    }

    public async Task GetEquipments()
    {
        var res = await api.GET(
            url:api.Player+"/"+currentPlayer.id+"/equip",
            param: new Dictionary<string, string>
            {
                
            });
        var equipments = GetModel<Equipment[]>(res);
        if (equipments == null) return;
        currentPlayer.equipments = equipments;
    }

    public async Task GetMount()
    {
        var res = await api.GET(
            url:api.Player+"/"+currentPlayer.id+"/mount",
            param: new Dictionary<string, string>
            {
            }
            );
        var mounts = GetModel<Mount[]>(res);
        if (mounts == null) return;
        currentPlayer.mounts = mounts;
    }

    public async Task GetItem()
    {
        var res = await api.GET(
            url:api.Player+"/"+currentPlayer.id+"/item",
            param:new Dictionary<string, string>
            {
                
            }
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
        currentPlayer.equipments.AddRange(new Equipment[] {equipment});
        if (player1.id == currentPlayer.id)
        {
            player1.equipments.AddRange(new Equipment[]{equipment});
        }
        else
        {
            player2.equipments.AddRange(new Equipment[]{equipment});
        }
        // TODO:update star?
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
        currentPlayer.mounts.AddRange(new Mount[] {mount});
        if (player1.id == currentPlayer.id)
        {
            player1.mounts.AddRange(new Mount[]{mount});
        }
        else
        {
            player2.mounts.AddRange(new []{mount});
        }
        // TODO:update star?
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
        currentPlayer.items.AddRange(new Model.Item[] {item});
        if (player1.id == currentPlayer.id)
        {
            player1.items.AddRange(new Model.Item[]{item});
        }
        else
        {
            player2.items.AddRange(new Model.Item[]{item});
        }
        // TODO:update star?
    }
    
    /* Character Controller*/

    
    public async Task GetCharacter(int id)
    {
        var res = await api.GET(
            url:api.Character+"/"+id,
            param: new Dictionary<string, string>()
        );
        var character = GetModel<Model.Character>(res);
        if (character == null) return;
        UpdateCharacterAttribute(character);
    }

    public async Task GetCharacter(Character characterOld)
    {
        var res = await api.GET(
            url:api.Character+"/"+characterOld.id,
            param: new Dictionary<string, string>()
        );
        var character = GetModel<Model.Character>(res);
        if (character == null) return;
        UpdateCharacterAttribute(character);
    }

    public async Task DismissCharacter(int characterOld)
    {
        var res = await api.PUT(
            url:api.Character+"/"+characterOld+"/dismiss",
            param: new Dictionary<string, string>()
        );
        var character = GetModel<Model.Character>(res);
        if (character == null) return;
        //TODO: delete _character[characterOld.x,characterOld.y]
    }

    public async Task MoveCharacter(Character characterOld,int x,int y)
    {
        var res = await api.PUT(
            url:api.Character+"/"+characterOld.id+"/move",
            param: new Dictionary<string, string>
            {
                {"x",x.ToString()},
                {"y",y.ToString()}
            }
        );
        var character = GetModel<Model.Character>(res);
        if (character == null) return;
        //TODO: delete _character[characterOld.x,characterOld.y]
        UpdateCharacterAttribute(character);
    }

    public async Task AttackCharacter(Character characterAttack,Character characterAttacked)
    {
        var res = await api.PUT(
            url:api.Character+"/"+characterAttack.id+"/char",
            param: new Dictionary<string, string>
            {
                {"attackid",characterAttacked.id.ToString()}
            }
        );
        var character = GetModel<Model.Character>(res);
        if (character == null) return;
        UpdateCharacterAttribute(character);
        //TODO: check  update other character
        await GetCharacter(characterAttacked);
        //TODO: check value of z
        await GetStructure(GetStructureByPosition(new Vector3Int(character.x,character.y,0)));
    }
    
    public async Task AttackStructure(Character characterAttack,Structure structure)
    {
        var res = await api.PUT(
            url:api.Character+"/"+characterAttack.id+"/structure",
            param: new Dictionary<string, string>
            {
                {"attackid",structure.id.ToString()}
            }
        );
        var character = GetModel<Model.Character>(res);
        if (character == null) return;
        UpdateCharacterAttribute(character);
        //TODO: check  update other character
        await GetStructure(structure);
    }

    /*structure controller*/
    public async Task GetStructure(Structure structureOld)
    {
        var res = await api.GET(
            url:api.Structure+"/"+structureOld.id,
            param:new Dictionary<string, string>()
        );
        var structure = GetModel<Model.Structure>(res);
        if (structure == null) return;
        UpdateStructureAttribute(structure);
    }

    public async Task GetCharacters(Structure structureOld)
    {
        var res = await api.GET(
            url:api.Structure+"/"+structureOld.id+"/char",
            param:new Dictionary<string, string>()
        );
        var structure = GetModel<Model.Structure>(res);
        if (structure == null) return;
        UpdateStructureAttribute(structure);
    }

    public async Task BuyCharacters(Structure structureOld,int id,int x,int y,int type)
    {
        var res = await api.POST(
            url:api.Structure+"/"+structureOld.id+"/char",
            param:new Dictionary<string, string>
            {
                {"playerid",currentPlayer.id.ToString()},
                {"id",id.ToString()},
                {"x",x.ToString()},
                {"y",y.ToString()},
                {"type",type.ToString()}
            }
        );
        var structure = GetModel<Model.Structure>(res);
        if (structure == null) return;
        UpdateStructureAttribute(structure);
        //TODO:update player's character and update player's star
        await GetCharacter(id);
        currentPlayer.stars -= 3;
        if (player1.id == currentPlayer.id)
        {
            player1.stars -= 3;
        }
        else
        {
            player2.stars -= 3;
        }
    }
    
    public async Task UpdateCharacter(Structure structureOld,Character character,int option)
    {
        var res = await api.PUT(
            url:api.Structure+"/"+structureOld.id+"/camp",
            param:new Dictionary<string, string>
            {
                {"characterid",character.id.ToString()},
                {"option",option.ToString()}
            }
        );
        var structure = GetModel<Model.Structure>(res);
        if (structure == null) return;
        UpdateStructureAttribute(structure);
        await GetCharacter(character);
    }
    
    public async Task EarnStars(Structure structureOld,Character character)
    {
        var res = await api.PUT(
            url:api.Structure+"/"+structureOld.id+"/market",
            param:new Dictionary<string, string>
            {
                {"characterid",character.id.ToString()},
            }
        );
        var structure = GetModel<Model.Structure>(res);
        if (structure == null) return;
        UpdateStructureAttribute(structure);
        await GetCharacter(character);
    }

    public async Task UpdateTechnologies(Structure structureOld,Character character,int option)
    {
        var res = await api.PUT(
            url:api.Structure+"/"+structureOld.id+"/insititude",
            param:new Dictionary<string, string>
            {
                {"characterid",character.id.ToString()},
                {"option",option.ToString()}
            }
        );
        var structure = GetModel<Model.Structure>(res);
        if (structure == null) return;
        UpdateStructureAttribute(structure);
        await GetCharacter(character);
    }

    public async Task UpdateStructure(Structure structureOld,int option)
    {
        var res = await api.PUT(
            url:api.Structure+"/"+structureOld.id+"/upd",
            param:new Dictionary<string, string>
            {
                {"option",option.ToString()}
            }
        );
        var structure = GetModel<Model.Structure>(res);
        if (structure == null) return;
        UpdateStructureAttribute(structure);
        currentPlayer.stars -= structureOld.level * 10;
        if (currentPlayer.id == player1.id)
        {
            player1.stars -= structureOld.level * 10;
        }
        else
        {
            player2.stars -= structureOld.level * 10;
        }
    }
    
    private T GetModel<T>(HttpResponseMessage res)
    {
        if (res == null)
        {
            EditorUtility.DisplayDialog("Unknown errors", "Check your network", "ok");
            return default;
        }

        if (res.StatusCode != HttpStatusCode.OK)
        {
            EditorUtility.DisplayDialog("Errors", res.ReasonPhrase, "ok");
            return default;
        }

        var content = res.Content.ReadAsStringAsync().Result;
        var model = JsonConvert.DeserializeObject<Model<T>>(content);

        if (model == null)
        {
            return default;
        }

        if (model.code != 200)
        {
            EditorUtility.DisplayDialog("Bad Request", model.msg, "ok");
            return default;
        }

        return model.data;
    }

    private void SetData(Game game)
    {
        gameID = game.id;
        UpdateData(game);
        SetMap(game.map);
        SetStructure(game.structures);
        SetCharacter(game.player1.characters[0], game.player2.characters[0]);
    }

    public void UpdateData(Game game)
    {
        round = game.round / 2;
        SetPlayer(player1, game.player1);
        SetPlayer(player2, game.player2);
        currentPlayer = game.currentPlayer ? player2 : player1;
        SetPlayerShop(game.currentPlayer ? player1 : player2, game.shop);
    }

    private void UpdateCharacterAttribute(Model.Character character)
    {
        if (_characters[character.x, character.y] == null)
        {
            _characters[character.x, character.y] = new Character();
        }

        _characters[character.x, character.y].id = character.id;
        _characters[character.x, character.y].name = character.name;
        _characters[character.x, character.y].characterClass = character.characterClass;
        _characters[character.x, character.y].actionRange = character.actionRange;
        _characters[character.x, character.y].attack = character.attack;
        _characters[character.x, character.y].defense = character.defense;
        _characters[character.x, character.y].hp = character.hp;
        _characters[character.x, character.y].level = character.level;
        _characters[character.x, character.y].equipment = character.equipment;
        _characters[character.x, character.y].mount = character.mount;
    }

    private void UpdateCharacterPlayer(Model.Character character)
    {
        if (_characters[character.x, character.y] == null)
        {
            return;
        }

        _characters[character.x, character.y].player = currentPlayer;
    }

    private void UpdateCharacterState(Vector2Int vector2Int, int state)
    {
        if (_characters[vector2Int.x, vector2Int.y] == null)
        {
            return;
        }

        _characters[vector2Int.x, vector2Int.y].actionState = state;
    }

    private void SetCharacter(Model.Character character1, Model.Character character2)
    {
        UpdateCharacterAttribute(character1);
        _characters[character1.x, character1.y].player = player1;
        UpdateCharacterAttribute(character2);
        _characters[character2.x, character2.y].player = player2;
    }

    private void UpdateStructureAttribute(Model.Structure structure)
    {
        if (_structures[structure.x, structure.y] == null)
        {
            _structures[structure.x, structure.y] = new Structure();
        }

        _structures[structure.x, structure.y].id = structure.id;
        _structures[structure.x, structure.y].structureClass = structure.structureClass;
        _structures[structure.x, structure.y].level = structure.level;
        _structures[structure.x, structure.y].hp = structure.hp;
        _structures[structure.x, structure.y].remainingRound = structure.remainingRound;
        _structures[structure.x, structure.y].value = structure.value;
        _structures[structure.x, structure.y].characters = new Character[structure.characters.Length];
        for (var i = 0; i < structure.characters.Length; i++)
        {
            _structures[structure.x, structure.y].characters[i].id = structure.characters[i].id;
            _structures[structure.x, structure.y].characters[i].name = structure.characters[i].name;
            _structures[structure.x, structure.y].characters[i].actionRange = structure.characters[i].actionRange;
            _structures[structure.x, structure.y].characters[i].attack = structure.characters[i].attack;
            _structures[structure.x, structure.y].characters[i].defense = structure.characters[i].defense;
            _structures[structure.x, structure.y].characters[i].hp = structure.characters[i].hp;
            _structures[structure.x, structure.y].characters[i].level = structure.characters[i].level;
        }
    }

    private void UpdateStructurePlayer(Model.Structure structure)
    {
        if (_structures[structure.x, structure.y] == null)
        {
            return;
        }

        _structures[structure.x, structure.y].player = currentPlayer;
    }

    private void SetStructure(Model.Structure[] structure)
    {
        foreach (var s in structure)
        {
            UpdateStructureAttribute(s);
            GridController.Instance.AddStructure(new Vector3Int(s.x - 8, s.y - 8, 0),
                s.structureClass == StructureClass.VILLAGE ? 0 : 1);
        }
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
        if (shop == null)
        {
            return;
        }

        player.shop = shop;
    }

    private void SetMap(int[,] sourceMap)
    {
        for (int i = 0; i < sourceMap.GetLength(0); i++)
        {
            for (int j = 0; j < sourceMap.GetLength(1); j++)
            {
                int target = (sourceMap[i, j] >> 2) & 3;
                _map[i, j] = target == 3 ? 0 : target;
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