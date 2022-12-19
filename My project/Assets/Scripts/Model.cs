using System;
using System.Collections.Generic;

namespace Model
{
    [Serializable]
    public class Model<T>
    {
        public int code;
        public string msg;
        public T data;
    }

    [Serializable]
    public class Game
    {
        public int id;
        public int round;
        public bool currentPlayer;
        public Player player1;
        public Player player2;
        public Shop shop;
        public Structure[] structures;
        public int[,] map;
    }

    [Serializable]
    public class Account
    {
        public int id;
        public string username;
        public Archive[] archives;
    }

    [Serializable]
    public class Archive
    {
        public int id;
        public int gameId;
        public Player player;
    }

    [Serializable]
    public class Player
    {
        public int id;
        public long stars;
        public int prosperityDegree;
        public int peaceDegree;
        public int[,] technologyTree;
        public Character[] characters;
        public Equipment[] equipments;
        public Mount[] mounts;
        public Item[] items;
        public Structure[] structures;
    }

    [Serializable]
    public class Character
    {
        public int id;
        public string name;
        public CharacterClass characterClass;
        public int actionState;
        public int actionRange;
        public int attack;
        public int defense;
        public int hp;
        public int level;
        public int x;
        public int y;
        public Equipment equipment;
        public Mount mount;
    }

    [Serializable]
    public enum CharacterClass
    {
        WARRIOR,
        EXPLORER,
        SCHOLAR,
    }

    [Serializable]
    public class Item
    {
        public int id;
        public string name;
        public ItemClass itemClass;
        public int attack;
        public int defense;
        public int hp;
        public string description;
    }

    public enum ItemClass
    {
        BASIC,
        BEER,
        POTION,
        FISH,
    }

    [Serializable]
    public class Mount
    {
        public int id;
        public string name;
        public MountClass mountClass;
        public int attack;
        public int defense;
        public int actionRange;
        public string description;
    }

    public enum MountClass
    {
        BASIC,
        FOX,
        HORSE,
        ELEPHANT,
    }

    [Serializable]
    public class Equipment
    {
        public int id;
        public string name;
        public EquipmentClass equipmentClass;
        public int attack;
        public int defense;
        public int attackRange;
        public string description;
    }

    [Serializable]
    public enum EquipmentClass
    {
        BASIC,
        SWORD,
        ARROW,
        CANNON,
        SHIELD,
    }

    [Serializable]
    public class Structure
    {
        public int id;
        public StructureClass structureClass;
        public int level;
        public int hp;
        public int remainingRound;
        public int value;
        public int x;
        public int y;
        public Character[] characters;
    }

    [Serializable]
    public enum StructureClass
    {
        VILLAGE = 1,
        CAMP,
        MARKET,
        INSTITUTE,
        RELIC,
    }

    [Serializable]
    public class Shop
    {
        public Equipment[] equipments;
        public Item[] items;
        public Mount[] mounts;
        public int[,] index;
    }
}