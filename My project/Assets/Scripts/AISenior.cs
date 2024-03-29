﻿using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Model;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;
using Vector2 = System.Numerics.Vector2;
using Vector3 = UnityEngine.Vector3;
public class AISenior: AI
{
    private Random _random = new Random();

    private int ATTACKINGROUND = 10;
    private int INF = 2000;

    private int CheckAccessible(Vector3Int position,bool type,int currentPlayer)
    {
        var x = position.x;
        var y = position.y;

        if (x < 0 || x >= DataManager.MapSize || y < 0 || y >= DataManager.MapSize)
        {
            return 0;
        }

        if (y % 2 == 1 && x == DataManager.MapSize - 1)
        {
            return 0;
        }

        var pos = new Vector3Int(position.y, position.x);
        if (DataManager.Instance.GetMapByPosition(pos) == 1 ||
            (DataManager.Instance.GetMapByPosition(pos) == 2 && (!type)))
        {
            return 0;
        }
        if (DataManager.Instance.GetCharacterByPosition(position) != null)
        {
            if (DataManager.Instance.GetCharacterByPosition(position).player.id != DataManager.Instance.currentPlayer.id) return 2; //into enemy
            else return 0;
        }

        var structure = DataManager.Instance.GetStructureByPosition(position);
        if (structure != null)
        {
            if (structure.player == null) return 1;// into a village
            if (structure.player.id != DataManager.Instance.currentPlayer.id) return 2;//into enemy building
        }

        return -1;//null space
    }

    private int min(int x, int y)
    {
        if (x < y) return x;
        return y;
    }
    private int getClosest(Vector3Int position,Character character,int questType)//1:village 2:enemy
    {
        int ret = INF;
        var n = DataManager.MapSize;
        var dis = new int[n, n];
        bool type = character.characterClass == CharacterClass.EXPLORER;
        for (var i = 0; i < n; i++)
        {
            for (var j = 0; j < n; j++)
            {
                dis[i, j] = 0;
            }
        }

        int actionRange = n+n,currentPlayer=character.player.id;
        dis[position.x, position.y] = 1;
        for (var t = 1; t < actionRange; t++)
        for (var i = 0; i < n; i++)
        for (var j = 0; j < n; j++)
            if (dis[i, j] == t)
            {
                var op = 1;
                if (j % 2 == 0) op = -1;
                var temp = new Vector3Int(i + op, j + 1);
                int c = CheckAccessible(temp, type, currentPlayer);
                if (c==-1)
                {
                    if (dis[i + op, j + 1] == 0)
                    {
                        dis[i + op, j + 1] = t + 1;
                    }

                }
                else if (c == questType) ret = min(ret, t+1);

                temp = new Vector3Int(i + 1, j);
                c=CheckAccessible(temp, type, currentPlayer);
                if (c==-1)
                {
                    if (dis[i + 1, j] == 0)
                    {
                        dis[i + 1, j] = t + 1;
                    }
                }
                else if (c == questType) ret = min(ret, t+1);

                temp = new Vector3Int(i + op, j - 1);
                c=CheckAccessible(temp, type, currentPlayer);
                if (c==-1)
                {
                    if (dis[i + op, j - 1] == 0)
                    {
                        dis[i + op, j - 1] = t + 1;
                    }
                }
                else if (c == questType) ret = min(ret, t+1);

                temp = new Vector3Int(i, j + 1);
                c=CheckAccessible(temp, type, currentPlayer);
                if (c==-1)
                {
                    if (dis[i, j + 1] == 0)
                    {
                        dis[i, j + 1] = t + 1;
                    }
                }
                else if (c == questType) ret = min(ret, t+1);

                temp = new Vector3Int(i, j - 1);
                c=CheckAccessible(temp, type, currentPlayer);
                if (c==-1)
                {
                    if (dis[i, j - 1] == 0)
                    {
                        dis[i, j - 1] = t + 1;
                    }
                    
                }
                else if (c == questType) ret = min(ret, t+1);

                temp = new Vector3Int(i - 1, j);
                c=CheckAccessible(temp, type, currentPlayer);
                if (c==-1)
                {
                    if (dis[i - 1, j] == 0)
                    {
                        dis[i - 1, j] = t + 1;
                    }
                }
                else if (c == questType) ret = min(ret, t+1);
            }

        return ret;
    }
    public override async Task MoveCharacters()
    {
        getRound();
        GetCharacters();
        //greedy, get to the nearest village/enemy in terms of rounds
        GetCharactersPos();
        for (int i = 0; i < characters.Count; i++)
        {
            List<Vector3Int> movableList = new List<Vector3Int>();
            Vector3Int pos = chPositions[i]; 
            movableList = GameUtils.Instance.GetActionRange(pos);
            //int rand = _random.Next(0, movableList.Count);
            Vector3Int destPos = pos;
            if(characters[i].characterClass==CharacterClass.SCHOLAR) continue;
            if (round < ATTACKINGROUND)
            {
                //get to the place which gets closest to an unoccupied village
                int minDist=getClosest(destPos,characters[i],1);
                for (int j = 0; j < movableList.Count; j++)
                {
                    int currentDist = getClosest(movableList[j], characters[i], 1);
                    if (currentDist < minDist)
                    {
                        minDist = currentDist;
                        destPos = movableList[j];
                    }
                }
            }
            else
            {
                //get to the place which gets closest to an enemy
                int minDist=getClosest(destPos,characters[i],2);
                for (int j = 0; j < movableList.Count; j++)
                {
                    int currentDist = getClosest(movableList[j], characters[i], 2);
                    if (currentDist < minDist)
                    {
                        minDist = currentDist;
                        destPos = movableList[j];
                    }
                }
                Debug.Log("closest enemy:"+destPos);
            }
            Debug.Log("Move: "+pos+" "+destPos);
            if(pos.x!=destPos.x||pos.y!=destPos.y) await GameManager.Instance.MoveCharacter(pos, destPos);
        }
    }

    private int getEnemyHPByPosition(Vector3Int position)
    {
        Debug.Log("Attack:"+position);
        Structure objectStructure = DataManager.Instance.GetStructureByPosition(position);
        Character objectCharacter = DataManager.Instance.GetCharacterByPosition(position);
        if (objectStructure == null) return objectCharacter.hp;
        return objectStructure.hp;
    }
    public override async Task AttackCharacters()
    {
        //attack a thing with the min hp
        chPositions = DataManager.Instance.GetCharacterPosByPlayer(player.id);
        foreach (Vector3Int pos in chPositions)
        {
            Debug.Log("charapos:"+pos);
            List<Vector3Int> attackPositions = GameUtils.Instance.GetAttackRange(pos);
            if (attackPositions != null&&attackPositions.Count != 0)
            {
                Vector3Int attackPos = attackPositions[0];
                int minHP = INF;
                for (int i = 0; i < attackPositions.Count; i++)
                {
                    Debug.Log("attackposition:"+attackPositions[i]);
                    int HPleft = getEnemyHPByPosition(attackPositions[i]);
                    if (HPleft < minHP)
                    {
                        minHP = HPleft;
                        attackPos = attackPositions[i];
                    }
                }
                await GameManager.Instance.AttackPosition(pos, attackPos);
            }
        }
    }

    public override async Task Buy()
    {
        List<Structure> structures = DataManager.Instance.GetStructureByPlayer(player.id);
        //List<Vector3Int> structPos = DataManager.Instance.GetStructurePosByPlayer(player.id);
        //List<Vector3Int> recruitPos = new List<Vector3Int>();
        if (structures.Count == 0)
        {
            return;
        }
        //upgrade structure
        getStructurePos();
        getStar();
        for (int i = 0; i < strPositions.Count; i++)
        {
            Vector3Int position = strPositions[i];
            Structure structure = DataManager.Instance.GetStructureByPosition(strPositions[i]);
            Debug.Log("nmd"+structure.structureClass);
            if (structure.structureClass == StructureClass.VILLAGE)
            {
                Debug.Log("给爷升级！");
                int type = 0;
                if (getStructureCnt(StructureClass.INSTITUTE) == 0) type = 2;
                if (getStructureCnt(StructureClass.MARKET) < 3) type = 1;
                
                await GameManager.Instance.UpgradeStructure(position, type);
            }
            await buyPeople(strPositions[i]);
        }

        buyTech();
        clearShop();
        for(int i=0;i<3;i++) useShop();

        /*bool flag = true;
        while (flag)
        {
            int action = _random.Next(0, 5);
            switch (action)
            {
                case 0:
                    flag = Recruit();
                    break;
                case 1:
                    BuyEquipment();
                    break;
                case 2:
                    BuyMount();
                    break;
                case 3:
                    UpgradeTechTree();
                    break;
                case 4:
                    UpgradeStructure();
                    break;
            }
        }*/

        return;
    }

    private async Task useShop()
    {

        if (shopCnt > 2 || star < 7) return;
        int t = _random.Next(3);
        if(t==0) BuyEquipment();
        if(t==1) BuyMount();
        if(t==2) BuyItems();
    }

    private async Task buyTech()
    {
        if (getCharacterCnt(CharacterClass.SCHOLAR) == 0 || getStructureCnt(StructureClass.INSTITUTE) == 0) return;
        getStructurePos();
        for (int i = 0; i < strPositions.Count; i++)
        {
            Structure structure = DataManager.Instance.GetStructureByPosition(strPositions[i]);
            if(structure.structureClass!=StructureClass.INSTITUTE) continue;
            if(structure.remainingRound>0) continue;
            for (int j = 0; j < 11; j++)
            {
                if (DataManager.Instance.currentPlayer.tech[j] == 1)
                { 
                    GameManager.Instance.UpdateTech(j);
                    break;
                }
            }
        }
    }

    private async Task buyPeople(Vector3Int position)
    {
        getStar();
        
        Structure structure = DataManager.Instance.GetStructureByPosition(position);
        if (DataManager.Instance.GetCharacterByPosition(position) != null||star<3) return;
        if(structure.structureClass==StructureClass.BASE||structure.structureClass==StructureClass.RELIC) return;
        Debug.Log("buyPeople:"+position);
        int type = 0;
        if (getCharacterCnt(CharacterClass.SCHOLAR) == 0
            &&DataManager.Instance.GetStructureByPosition(position).structureClass==StructureClass.INSTITUTE) type = 2;
        if (getCharacterCnt(CharacterClass.EXPLORER)<4) type = 1;
        if (structure.characters[0] != null)
        {
            await GameManager.Instance.AIbuyCharacter(position, structure.characters[0].id, position.x, position.y, type);
        }
        else if (structure.characters[1] != null)
            await GameManager.Instance.AIbuyCharacter(position, structure.characters[1].id, position.x, position.y, type);
        else if (structure.characters[2] != null)
            await GameManager.Instance.AIbuyCharacter(position, structure.characters[2].id, position.x, position.y, type);
    }
    private bool Recruit()
    {
        if (player.stars < 3)
        {
            return false;
        }
        else
        {
            // GameManager.Instance.BuyCharacter();
        }

        return true;
    }



}