
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public abstract class AI
{
    protected Player player;
    protected List<Character> characters = new List<Character>();
    protected List<Vector3Int> chPositions;
    protected int round;
    private Random _random = new Random();

    public void GetCharactersPos()
    {
        chPositions = DataManager.Instance.GetCharacterPosByPlayer(player.id);
    }

    public void GetCharacters()
    {
        characters = DataManager.Instance.CharactersOfPlayer(player.id);
    }

    public void getRound()
    {
        round = DataManager.Instance.round;
    }
    
    
    protected bool Recruit(List<Vector3Int> pos)
    {
        if (player.stars < 3)
        {
            return false;
        }
        else
        {
            int i = _random.Next(0, pos.Count);
            GameManager.Instance.BuyCharacter(pos[i]);
        }

        return true;
    }
    protected void UpgradeStructure()
    {
        
    }

    protected void UpgradeTechTree()
    {
        GameManager.Instance.OpenTechnologies();
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

    public abstract void MoveCharacters();

    public abstract void AttackCharacters();

    public abstract void Buy();

}