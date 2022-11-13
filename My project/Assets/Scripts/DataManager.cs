using System;
using System.Collections;
using System.Collections.Generic;
using Model;
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
    public int[,] Map = new int[MapSize, MapSize];
    public Character[,] Characters = new Character[MapSize, MapSize];
    public Structure[,] Structures = new Structure[MapSize, MapSize];

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetData(Game game)
    {
        gameID = game.id;
        UpdateData(game);
        SetMap(game.map);
        SetStructure(game.structures);
        SetCharacter(game.player1.characters[0], game.player2.characters[0]);
    }

    public void UpdateData(Game game)
    {
        round = game.round;
        currentPlayer = game.currentPlayer ? player2 : player1;
        SetPlayer(player1, game.player1);
        SetPlayer(player2, game.player2);
    }

    public void UpdateCharacterAttribute(Model.Character character)
    {
        if (Characters[character.x, character.y] == null)
        {
            Characters[character.x, character.y] = new Character();
        }

        Characters[character.x, character.y].id = character.id;
        Characters[character.x, character.y].name = character.name;
        Characters[character.x, character.y].characterClass = character.characterClass;
        Characters[character.x, character.y].actionRange = character.actionRange;
        Characters[character.x, character.y].attack = character.attack;
        Characters[character.x, character.y].defense = character.defense;
        Characters[character.x, character.y].hp = character.hp;
        Characters[character.x, character.y].level = character.level;
        Characters[character.x, character.y].equipment = character.equipment;
        Characters[character.x, character.y].mount = character.mount;
    }

    public void UpdateCharacterPlayer(Model.Character character)
    {
        if (Characters[character.x, character.y] == null)
        {
            return;
        }

        Characters[character.x, character.y].player = currentPlayer;
    }

    public void UpdateCharacterState(Vector2Int vector2Int, int state)
    {
        if (Characters[vector2Int.x, vector2Int.y] == null)
        {
            return;
        }

        Characters[vector2Int.x, vector2Int.y].actionState = state;
    }

    private void SetCharacter(Model.Character character1, Model.Character character2)
    {
        UpdateCharacterAttribute(character1);
        Characters[character1.x, character1.y].player = player1;
        UpdateCharacterAttribute(character2);
        Characters[character2.x, character2.y].player = player2;
    }

    public void UpdateStructureAttribute(Model.Structure structure)
    {
        if (Structures[structure.x, structure.y] == null)
        {
            Structures[structure.x, structure.y] = new Structure();
        }

        Structures[structure.x, structure.y].id = structure.id;
        Structures[structure.x, structure.y].structureClass = structure.structureClass;
        Structures[structure.x, structure.y].level = structure.level;
        Structures[structure.x, structure.y].hp = structure.hp;
        Structures[structure.x, structure.y].remainingRound = structure.remainingRound;
        Structures[structure.x, structure.y].value = structure.value;
        Structures[structure.x, structure.y].characters = new Character[structure.characters.Length];
        for (var i = 0; i < structure.characters.Length; i++)
        {
            Structures[structure.x, structure.y].characters[i].id = structure.characters[i].id;
            Structures[structure.x, structure.y].characters[i].name = structure.characters[i].name;
            Structures[structure.x, structure.y].characters[i].actionRange = structure.characters[i].actionRange;
            Structures[structure.x, structure.y].characters[i].attack = structure.characters[i].attack;
            Structures[structure.x, structure.y].characters[i].defense = structure.characters[i].defense;
            Structures[structure.x, structure.y].characters[i].hp = structure.characters[i].hp;
            Structures[structure.x, structure.y].characters[i].level = structure.characters[i].level;
        }
    }

    public void UpdateStructurePlayer(Model.Structure structure)
    {
        if (Structures[structure.x, structure.y] == null)
        {
            return;
        }

        Structures[structure.x, structure.y].player = currentPlayer;
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

    public void SetPlayer(Player destinationPlayer, Model.Player sourcePlayer)
    {
        destinationPlayer.id = sourcePlayer.id;
        destinationPlayer.stars = sourcePlayer.stars;
        destinationPlayer.prosperityDegree = sourcePlayer.prosperityDegree;
        destinationPlayer.peaceDegree = sourcePlayer.peaceDegree;
        destinationPlayer.equipments = sourcePlayer.equipments;
        destinationPlayer.mounts = sourcePlayer.mounts;
        destinationPlayer.items = sourcePlayer.items;
    }

    public void SetPlayerShop(Shop shop)
    {
    }

    private void SetMap(int[,] sourceMap)
    {
        for (int i = 0; i < sourceMap.GetLength(0); i++)
        {
            for (int j = 0; j < sourceMap.GetLength(1); j++)
            {
                int target = (sourceMap[i, j] >> 2) & 3;
                Map[i, j] = target == 3 ? 0 : target;
            }
        }
    }

    public int GetMapByPosition(Vector3Int vector3Int)
    {
        return Map[vector3Int.x, vector3Int.y];
    }

    public Character GetCharacterByPosition(Vector3Int vector3Int)
    {
        return Characters[vector3Int.x, vector3Int.y];
    }

    public Structure GetStructureByPosition(Vector3Int vector3Int)
    {
        return Structures[vector3Int.x, vector3Int.y];
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