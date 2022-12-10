
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public abstract class AI
{
    protected Player player;
    protected List<Character> characters = new List<Character>();
    protected List<Vector3Int> chPositions;

    public void GetCharactersPos()
    {
        chPositions = DataManager.Instance.GetCharacterPosByPlayer(player.id);
    }

    public void GetCharacters()
    {
        characters = DataManager.Instance.CharactersOfPlayer(player.id);
    }

    public abstract void MoveCharacters();

    public abstract void AttackCharacters();

    public abstract void Buy();

}