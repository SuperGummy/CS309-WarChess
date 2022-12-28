
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Model;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public abstract class AI
{
    public static Player player;
    protected List<Character> characters = new List<Character>();
    protected List<Vector3Int> chPositions,strPositions;
    protected int round,star,shopCnt;
    public static bool AIType;

    public static AI GetAI(bool type)
    {
        AIType = type;
        if (type)
            return new AISenior();
        return new AIJunior();
    }

    public int getCharacterCnt(CharacterClass c)
    {
        int ret = 0;
        GetCharacters();
        for(int i=0;i<characters.Count;i++)
            if (characters[i].characterClass == c)
                ret++;
        return ret;
    }

    public void clearShop()
    {
        shopCnt = 0;
    }

    public int getStructureCnt(StructureClass c)
    {
        int ret = 0;
        getStructurePos();
        for (int i = 0; i < strPositions.Count; i++)
        {
            Structure structure = DataManager.Instance.GetStructureByPosition(strPositions[i]);
            if (structure.structureClass == c) ret++;
        }

        return ret;
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
        if (DataManager.Instance.currentPlayer.stars < 7) return; 
        for (int i = 0; i < DataManager.Instance.currentPlayer.shop.equipments.Length; i++)
        {
            Equipment equipment = DataManager.Instance.currentPlayer.shop.equipments[i];
            if(equipment==null) continue;
            GameManager.Instance.BuyEquipment(equipment.id);
            return;
        }
    }


    protected void BuyMount()
    {
        if (DataManager.Instance.currentPlayer.stars < 7) return; 
        for (int i = 0; i < DataManager.Instance.currentPlayer.shop.mounts.Length; i++)
        {
            Mount mount = DataManager.Instance.currentPlayer.shop.mounts[i];
            if(mount==null) continue;
            GameManager.Instance.BuyMount(mount.id);
            return;
        }
    }

    protected void BuyItems()
    {
        if (DataManager.Instance.currentPlayer.stars < 7) return; 
        for (int i = 0; i < DataManager.Instance.currentPlayer.shop.items.Length; i++)
        {
            Item item = DataManager.Instance.currentPlayer.shop.items[i];
            if(item==null) continue;
            GameManager.Instance.BuyItem(item.id);
            return;
        }
    }

    public abstract Task MoveCharacters();

    public abstract Task AttackCharacters();

    public Task NextRound()
    {
        return GameManager.Instance.NextRound();
    }


    public abstract Task Buy();

}