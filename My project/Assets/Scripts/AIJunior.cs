using System;
using System.Collections.Generic;
using System.Numerics;
using Model;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;
using Vector3 = UnityEngine.Vector3;

public class AIJunior: AI
{
    private Random _random = new Random();
    public override void MoveCharacters()
    {
        GetCharactersPos();
        for (int i = 0; i < characters.Count; i++)
        {
            List<Vector3Int> movableList = new List<Vector3Int>();
            Vector3Int pos = chPositions[i]; 
            movableList = GameManager.Instance.GetActionRange(pos);
            int rand = _random.Next(0, movableList.Count);
            Vector3Int destPos = movableList[rand];
            GameManager.Instance.MoveCharacter(pos, destPos);
        }
    }

    public override void AttackCharacters()
    {
        GetCharactersPos();
        foreach (Vector3Int pos in chPositions)
        {
            List<Vector3Int> attackPositions = GameManager.Instance.GetActionRange(pos);
            if (attackPositions.Count != 0)
            {
                int num = _random.Next(0, attackPositions.Count);
                Vector3Int attackPos = attackPositions[num];
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