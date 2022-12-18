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
        }

        bool flag = true;
        while (flag && player.stars > 3)
        {
            int action = _random.Next(0, 9);
            switch (action)
            {
                case 0:
                    GetCharactersPos();
                    Recruit(chPositions);
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
                case 5:
                    BuyItems();
                    break;
                case 6:
                case 7:
                case 8:
                    flag = false;
                    break;
            }
        }
    }
    



}