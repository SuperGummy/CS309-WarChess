using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Model;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;
using Vector3 = UnityEngine.Vector3;

public class AIJunior: AI
{
    public override async Task MoveCharacters()
    {
        GetCharactersPos();
        for (int i = 0; i < chPositions.Count; i++)
        {
            List<Vector3Int> movableList = new List<Vector3Int>();
            Vector3Int pos = chPositions[i]; 
            // Debug.Log("character pos " + pos);
            movableList = GameUtils.Instance.GetActionRange(pos);
            if (movableList != null && movableList.Count > 0)
            {
                int rand = new Random().Next(0, movableList.Count);
                Vector3Int destPos = movableList[rand];
                Debug.Log("dest position " + destPos);
                await GameManager.Instance.MoveCharacter(pos, destPos);
            }
        }
    }

    public override async Task AttackCharacters()
    {
        GetCharactersPos();
        foreach (Vector3Int pos in chPositions)
        {
            // Debug.Log("current position" + pos);
            List<Vector3Int> attackPositions = GameUtils.Instance.GetAttackRange(pos);
            if (attackPositions != null && attackPositions.Count != 0)
            {
                int num = new Random().Next(0, attackPositions.Count);
                Vector3Int attackPos = attackPositions[num];
                Debug.Log("attack character pos " + pos);
                await GameManager.Instance.AttackPosition(pos, attackPos);
            }
        }
    }

    public override Task Buy()
    {
        List<Structure> structures = DataManager.Instance.GetStructureByPlayer(player.id);
        List<Vector3Int> structPos = DataManager.Instance.GetStructurePosByPlayer(player.id);
        List<Vector3Int> recruitPos = new List<Vector3Int>();
        if (structures.Count == 0)
        {
            return default;
        }
        for (int i = 0; i < structures.Count; i++)
        {
            if (structures[i].structureClass == StructureClass.VILLAGE)
            {
                int type = new Random().Next(0, 3);
                GameManager.Instance.UpgradeStructure(structPos[i], type);
            }

        }

        bool flag = true;
        while (flag && player.stars > 3)
        {
            int action = new Random().Next(0, 9);
            switch (action)
            {
                case 0:
                    Recruit(structPos);
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

        return default;
    }
    



}