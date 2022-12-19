using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Model;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;
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
            if (DataManager.Instance.GetCharacterByPosition(position).player.id != currentPlayer) return 2; //into enemy
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

        int actionRange = n,currentPlayer=character.player.id;
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
        //greedy, get to the nearest village/enemy in terms of rounds
        GetCharactersPos();
        for (int i = 0; i < characters.Count; i++)
        {
            List<Vector3Int> movableList = new List<Vector3Int>();
            Vector3Int pos = chPositions[i]; 
            movableList = GameUtils.Instance.GetActionRange(pos);
            //int rand = _random.Next(0, movableList.Count);
            Vector3Int destPos = pos;
            
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
            }
            GameManager.Instance.MoveCharacter(pos, destPos);
        }
    }

    private int getEnemyHPByPosition(Vector3Int position)
    {
        Structure objectStructure = DataManager.Instance.GetStructureByPosition(position);
        Character objectCharacter = DataManager.Instance.GetCharacterByPosition(position);
        if (objectStructure == null) return objectCharacter.hp;
        return objectStructure.hp;
    }
    public override async Task AttackCharacters()
    {
        //attack a thing with the min hp
        GetCharactersPos();
        foreach (Vector3Int pos in chPositions)
        {
            List<Vector3Int> attackPositions = GameUtils.Instance.GetActionRange(pos);
            if (attackPositions.Count != 0)
            {
                Vector3Int attackPos = attackPositions[0];
                int minHP = INF;
                for (int i = 0; i < attackPositions.Count; i++)
                {
                    int HPleft = getEnemyHPByPosition(attackPositions[i]);
                    if (HPleft < minHP)
                    {
                        minHP = HPleft;
                        attackPos = attackPositions[i];
                    }
                }
                GameManager.Instance.AttackPosition(pos, attackPos);
            }
        }
    }

    public override void Buy()
    {
        List<Structure> structures = DataManager.Instance.GetStructureByPlayer(player.id);
        List<Vector3Int> structPos = DataManager.Instance.GetStructurePosByPlayer(player.id);
        List<Vector3Int> recruitPos = new List<Vector3Int>();
        if (structures.Count == 0)
        {
            return;
        }
        //upgrade structure
        for (int i = 0; i < structures.Count; i++)
        {
            if (structures[i].structureClass == StructureClass.VILLAGE)
            {
                
                int type = _random.Next(0, 3);
                GameManager.Instance.UpgradeStructure(structPos[i], type);
            }

            int personNum = 3 - (structures[i].characters).Length;
            personNum = Math.Min(personNum, structures[i].remainingRound);
        }

        bool flag = true;
        while (flag)
        {
            int action = _random.Next(0, 5);
            switch (action)
            {
                case 0:
                    flag = Recruit();
                    break;
                case 1:
                    flag = BuyEquipment();
                    break;
                case 2:
                    flag = BuyMount();
                    break;
                case 3:
                    flag = UpgradeTechTree();
                    break;
                case 4:
                    flag = UpgradeStructure();
                    break;
            }
        }
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

    private bool UpgradeStructure()
    {
        return true;
    }

    private bool UpgradeTechTree()
    {
        return true;
    }

    private bool BuyEquipment()
    {
        if (player.stars < 7)
        {
            return false;
        }

        return true;
    }
    

    private bool BuyMount()
    {
        if (player.stars < 7)
        {
            return false;
        }

        return true;
    }

}