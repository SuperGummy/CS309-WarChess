
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public abstract class AI
{
    public static Player player;
    protected List<Character> characters = new List<Character>();
    protected List<Vector3Int> chPositions,strPositions;
    protected int round,star;
    public static bool AIType;

    public static AI GetAI(bool type)
    {
        AIType = type;
        if (type)
            return new AISenior();
        return new AIJunior();
    }

    public void getStar()
    {
        star = (int)DataManager.Instance.currentPlayer.stars;
    }

    public void getStructurePos()
    {
        strPositions = DataManager.Instance.GetStructurePosByPlayer(player.id);
    }
    public void GetCharactersPos()
    {
        chPositions = DataManager.Instance.GetCharacterPosByPlayer(player.id);
    }

    public void GetCharacters()
    {
        characters = DataManager.Instance.CharactersOfPlayer(player.id);
    }

    // public
    public void getRound()
    {
        round = DataManager.Instance.round;
    }

    public async Task Run()
    {
        await MoveCharacters();
        await AttackCharacters();
        await Buy();
        // this.Buy();
        await NextRound();
    }

    protected bool Recruit(List<Vector3Int> pos)
    {
        if (player.stars < 3)
        {
            return false;
        }
        else
        {
            if (pos.Count > 0)
            {
                int i = new Random().Next(0, pos.Count);
                Structure structure = DataManager.Instance.GetStructureByPosition(pos[i]);
                Character[] characters = structure.characters.Where(x => x != null).ToArray();
                if (characters.Length == 0)
                {
                    return false;
                }

                int type = new Random().Next(0, 3);
                GameManager.Instance.AIbuyCharacter(pos[i], characters[0].id, pos[i].x, pos[i].y, type);
                pos.RemoveAt(i);
            }
        }

        return true;
    }
    protected void UpgradeStructure()
    {
        
    }

    protected void UpgradeTechTree()
    {
    }

    protected void BuyEquipment()
    {
        if (player.stars >= 7)
        {
            GameManager.Instance.BuyEquipment(0);
        }

    }


    protected void BuyMount()
    {
        if (player.stars > 7)
        {
            GameManager.Instance.BuyEquipment(0);
        }

    }

    protected void BuyItems()
    {
        
    }

    public abstract Task MoveCharacters();

    public abstract Task AttackCharacters();

    public Task NextRound()
    {
        return GameManager.Instance.NextRound();
    }


    public abstract Task Buy();

}