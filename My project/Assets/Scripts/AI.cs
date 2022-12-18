
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
    protected List<Vector3Int> chPositions;
    protected int round;

    public static AI GetAI(bool type)
    {
        if (type)
            return new AISenior();
        return new AIJunior();
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

    public async void Run()
    {
        await this.MoveCharacters();
        await this.AttackCharacters();
        // this.Buy();
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
                GameManager.Instance.BuyCharacter(pos[i]);
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

    public abstract void Buy();

}