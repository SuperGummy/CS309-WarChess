using System;
using Model;

[System.Serializable]
public class Sl
{
    public int gameID;
    public int round;
    public Boolean currentPlayer;
    public Player player1;
    public Player player2;

    public int[] map;

    public Model.Character[] characters;
    public Model.Structure[] structures;
    public int[] characterPlayer;
    public int[] structurePlayer;

    public int[] structureCharacterCount;
    public Character[] structureCharacters;

    public int[] startIndex;
    public bool AIType;
}