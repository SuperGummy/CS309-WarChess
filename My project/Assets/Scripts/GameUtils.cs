using System;
using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUtils : MonoBehaviour
{
    public static GameUtils Instance;

    public void Awake()
    {
        Instance = this;
    }

    public bool JudgeEnd()
    {
        var scene = SceneManager.GetActiveScene();
        if (scene.name != "Game") return false;
        var player = DataManager.Instance.currentPlayer;
        if (player.prosperityDegree >= 50 && player.peaceDegree >= 30)
        {
            WinManager.side = player.id == DataManager.Instance.player1.id ? -1 : 1;
            if (GameManager.pvp)
                SceneController.Instance.LoadWin();
            else
            {
                if (!GameManager.Instance.aiTurn)
                    SceneController.Instance.LoadWin();
                else
                    SceneController.Instance.LoadLose();
            }

            return true;
        }

        var structure = DataManager.Instance.GetStructureByPosition(new Vector3Int(0, DataManager.MapSize - 1));
        if (structure == null) return false;
        if (structure.player.id == player.id)
        {
            var structureP = DataManager.Instance.GetStructureByPosition(new Vector3Int(DataManager.MapSize - 1, 0));
            if (structureP.hp == 0)
            {
                WinManager.side = player.id == DataManager.Instance.player1.id ? -1 : 1;
                if (GameManager.pvp)
                    SceneController.Instance.LoadWin();
                else
                {
                    if (!GameManager.Instance.aiTurn)
                        SceneController.Instance.LoadWin();
                    else
                        SceneController.Instance.LoadLose();
                }

                return true;
            }
        }
        else
        {
            if (structure.hp == 0)
            {
                WinManager.side = player.id == DataManager.Instance.player1.id ? -1 : 1;
                if (GameManager.pvp)
                    SceneController.Instance.LoadWin();
                else
                {
                    if (!GameManager.Instance.aiTurn)
                        SceneController.Instance.LoadWin();
                    else
                        SceneController.Instance.LoadLose();
                }

                return true;
            }
        }

        return false;
    }

    public bool CheckAccessible(Vector3Int position, bool type)
    {
        var x = position.x;
        var y = position.y;

        if (x < 0 || x >= DataManager.MapSize || y < 0 || y >= DataManager.MapSize)
        {
            return false;
        }

        if (y % 2 == 1 && x == DataManager.MapSize - 1)
        {
            return false;
        }

        var pos = new Vector3Int(position.y, position.x);
        if (DataManager.Instance.GetMapByPosition(pos) == 1 ||
            (DataManager.Instance.GetMapByPosition(pos) == 2 && (!type)))
        {
            return false;
        }

        if (DataManager.Instance.GetCharacterByPosition(position) != null)
        {
            return false;
        }

        var structure = DataManager.Instance.GetStructureByPosition(position);
        if (structure != null)
        {
            if (structure.player == null) return false;
            if (structure.player.id != DataManager.Instance.currentPlayer.id) return false;
        }

        return true;
    }

    public bool CheckBound(Vector3Int position)
    {
        var x = position.x;
        var y = position.y;
        return !(x < 0 || x >= DataManager.MapSize || y < 0 || y >= DataManager.MapSize);
    }

    public bool CheckAttack(Vector3Int position)
    {
        if (DataManager.Instance.GetCharacterByPosition(position) != null)
        {
            if (DataManager.Instance.GetCharacterByPosition(position).player.id !=
                DataManager.Instance.currentPlayer.id)
            {
                return true;
            }
        }

        var structure = DataManager.Instance.GetStructureByPosition(position);
        if (structure != null)
        {
            if (structure.player?.id != DataManager.Instance.currentPlayer.id) return true;
        }

        return false;
    }

    public List<Vector3Int> GetActionPath(Vector3Int position, Vector3Int target)
    {
        var character = DataManager.Instance.GetCharacterByPosition(position);
        bool type = character.characterClass == CharacterClass.EXPLORER;
        if (!CheckAccessible(target, type))
        {
            return default;
        }

        var n = DataManager.MapSize;
        var actionRange = character.actionRange + 1;
        var result = new List<Vector3Int>();
        var dis = new int[n, n];
        for (var i = 0; i < n; i++)
        {
            for (var j = 0; j < n; j++)
            {
                dis[i, j] = 0;
            }
        }

        dis[position.x, position.y] = 1;
        for (var t = 1; t < actionRange; t++)
        for (var i = 0; i < n; i++)
        for (var j = 0; j < n; j++)
            if (dis[i, j] == t)
            {
                var op = 1;
                if (j % 2 == 0) op = -1;
                var temp = new Vector3Int(i + op, j + 1);
                if (CheckAccessible(temp, type))
                {
                    if (dis[i + op, j + 1] == 0)
                    {
                        dis[i + op, j + 1] = t + 1;
                    }
                }

                temp = new Vector3Int(i + 1, j);
                if (CheckAccessible(temp, type))
                {
                    if (dis[i + 1, j] == 0)
                    {
                        dis[i + 1, j] = t + 1;
                    }
                }

                temp = new Vector3Int(i + op, j - 1);
                if (CheckAccessible(temp, type))
                {
                    if (dis[i + op, j - 1] == 0)
                    {
                        dis[i + op, j - 1] = t + 1;
                    }
                }

                temp = new Vector3Int(i, j + 1);
                if (CheckAccessible(temp, type))
                {
                    if (dis[i, j + 1] == 0)
                    {
                        dis[i, j + 1] = t + 1;
                    }
                }

                temp = new Vector3Int(i, j - 1);
                if (CheckAccessible(temp, type))
                {
                    if (dis[i, j - 1] == 0)
                    {
                        dis[i, j - 1] = t + 1;
                    }
                }

                temp = new Vector3Int(i - 1, j);
                if (CheckAccessible(temp, type))
                {
                    if (dis[i - 1, j] == 0)
                    {
                        dis[i - 1, j] = t + 1;
                    }
                }
            }

        if (dis[target.x, target.y] == 0)
        {
            //can't
            return default;
        }

        var x = target.x;
        var y = target.y;
        result.Add(target);
        for (var t = dis[x, y] - 1; t >= 2; t--)
        {
            var op = 1;
            if (y % 2 == 0) op = -1;
            var temp = new Vector3Int(x + op, y + 1);
            if (CheckAccessible(temp, type))
            {
                if (dis[x + op, y + 1] == t)
                {
                    result.Add(temp);
                    x += op;
                    y += 1;
                    continue;
                }
            }

            temp = new Vector3Int(x + 1, y);
            if (CheckAccessible(temp, type))
            {
                if (dis[x + 1, y] == t)
                {
                    result.Add(temp);
                    x += 1;
                    continue;
                }
            }

            temp = new Vector3Int(x + op, y - 1);
            if (CheckAccessible(temp, type))
            {
                if (dis[x + op, y - 1] == t)
                {
                    result.Add(temp);
                    x += op;
                    y -= 1;
                    continue;
                }
            }

            temp = new Vector3Int(x, y + 1);
            if (CheckAccessible(temp, type))
            {
                if (dis[x, y + 1] == t)
                {
                    result.Add(temp);
                    y += 1;
                    continue;
                }
            }

            temp = new Vector3Int(x, y - 1);
            if (CheckAccessible(temp, type))
            {
                if (dis[x, y - 1] == t)
                {
                    result.Add(temp);
                    y -= 1;
                    continue;
                }
            }

            temp = new Vector3Int(x - 1, y);
            if (CheckAccessible(temp, type))
            {
                if (dis[x - 1, y] == t)
                {
                    result.Add(temp);
                    x -= 1;
                }
            }
        }

        result.Add(position);
        result.Reverse();
        return result;
    }

    internal List<Vector3Int> GetActionRange(Vector3Int position)
    {
        var character = DataManager.Instance.GetCharacterByPosition(position);
        if (character == null) return default;

        var type = character.characterClass == CharacterClass.EXPLORER;
        if (character.player.id != DataManager.Instance.currentPlayer.id || character.hp <= 0)
        {
            return default;
        }

        if (character.actionState >= 1) return default;

        var n = DataManager.MapSize;
        var actionRange = character.actionRange + 1;
        var result = new List<Vector3Int>();
        var dis = new int[n, n];
        for (var i = 0; i < n; i++)
        {
            for (var j = 0; j < n; j++)
            {
                dis[i, j] = 0;
            }
        }

        dis[position.x, position.y] = 1;
        for (var t = 1; t < actionRange; t++)
        for (var i = 0; i < n; i++)
        for (var j = 0; j < n; j++)
            if (dis[i, j] == t)
            {
                var op = 1;
                if (j % 2 == 0) op = -1;
                var temp = new Vector3Int(i + op, j + 1);
                if (CheckAccessible(temp, type))
                {
                    if (dis[i + op, j + 1] == 0)
                    {
                        dis[i + op, j + 1] = t + 1;
                    }
                }

                temp = new Vector3Int(i + 1, j);
                if (CheckAccessible(temp, type))
                {
                    if (dis[i + 1, j] == 0)
                    {
                        dis[i + 1, j] = t + 1;
                    }
                }

                temp = new Vector3Int(i + op, j - 1);
                if (CheckAccessible(temp, type))
                {
                    if (dis[i + op, j - 1] == 0)
                    {
                        dis[i + op, j - 1] = t + 1;
                    }
                }

                temp = new Vector3Int(i, j + 1);
                if (CheckAccessible(temp, type))
                {
                    if (dis[i, j + 1] == 0)
                    {
                        dis[i, j + 1] = t + 1;
                    }
                }

                temp = new Vector3Int(i, j - 1);
                if (CheckAccessible(temp, type))
                {
                    if (dis[i, j - 1] == 0)
                    {
                        dis[i, j - 1] = t + 1;
                    }
                }

                temp = new Vector3Int(i - 1, j);
                if (CheckAccessible(temp, type))
                {
                    if (dis[i - 1, j] == 0)
                    {
                        dis[i - 1, j] = t + 1;
                    }
                }
            }

        for (var i = 0; i < n; i++)
        {
            for (var j = 0; j < n; j++)
            {
                if (dis[i, j] > 1)
                {
                    var temp = new Vector3Int(i, j);
                    result.Add(temp);
                }
            }
        }

        return result;
    }

    public List<Vector3Int> GetEmptyPosition(Vector3Int position)
    {
        var structure = DataManager.Instance.GetStructureByPosition(position);
        if (structure?.player == null || structure.player.id != DataManager.Instance.currentPlayer.id)
            return default;
        var type = false;

        var res = new List<Vector3Int>();
        var i = position.x;
        var j = position.y;
        var op = 1;
        if (j % 2 == 0) op = -1;
        var temp = new Vector3Int(i, j);
        if (CheckAccessible(temp, type))
        {
            res.Add(temp);
        }

        temp = new Vector3Int(i + op, j + 1);
        if (CheckAccessible(temp, type))
        {
            res.Add(temp);
        }

        temp = new Vector3Int(i + 1, j);
        if (CheckAccessible(temp, type))
        {
            res.Add(temp);
        }

        temp = new Vector3Int(i + op, j - 1);

        if (CheckAccessible(temp, type))
        {
            res.Add(temp);
        }

        temp = new Vector3Int(i, j + 1);

        if (CheckAccessible(temp, type))
        {
            res.Add(temp);
        }

        temp = new Vector3Int(i, j - 1);

        if (CheckAccessible(temp, type))
        {
            res.Add(temp);
        }

        temp = new Vector3Int(i - 1, j);

        if (CheckAccessible(temp, type))
        {
            res.Add(temp);
        }

        return res;
    }


    public List<Vector3Int> GetAttackRange(Vector3Int position)
    {
        var character = DataManager.Instance.GetCharacterByPosition(position);
        if (character == null)
        {
            return default;
        }

        if (character.player.id != DataManager.Instance.currentPlayer.id || character.hp <= 0)
        {
            return default;
        }

        if (character.actionState >= 2)
        {
            return default;
        }

        var n = DataManager.MapSize;
        var attackRange = character.actionRange;
        if (character.equipment == null)
            attackRange += 1;
        else
            attackRange += character.equipment.attackRange + 1;
        var result = new List<Vector3Int>();
        var dis = new int[n, n];
        for (var i = 0; i < n; i++)
        {
            for (var j = 0; j < n; j++)
            {
                dis[i, j] = 0;
            }
        }

        dis[position.x, position.y] = 1;
        for (var t = 1; t < attackRange; t++)
        for (var i = 0; i < n; i++)
        for (var j = 0; j < n; j++)
            if (dis[i, j] == t)
            {
                var op = 1;
                if (j % 2 == 0) op = -1;
                var temp = new Vector3Int(i + op, j + 1);
                if (CheckBound(temp))
                {
                    if (dis[i + op, j + 1] == 0)
                    {
                        dis[i + op, j + 1] = t + 1;
                    }
                }

                temp = new Vector3Int(i + 1, j);
                if (CheckBound(temp))
                {
                    if (dis[i + 1, j] == 0)
                    {
                        dis[i + 1, j] = t + 1;
                    }
                }

                temp = new Vector3Int(i + op, j - 1);
                if (CheckBound(temp))
                {
                    if (dis[i + op, j - 1] == 0)
                    {
                        dis[i + op, j - 1] = t + 1;
                    }
                }

                temp = new Vector3Int(i, j + 1);
                if (CheckBound(temp))
                {
                    if (dis[i, j + 1] == 0)
                    {
                        dis[i, j + 1] = t + 1;
                    }
                }

                temp = new Vector3Int(i, j - 1);
                if (CheckBound(temp))
                {
                    if (dis[i, j - 1] == 0)
                    {
                        dis[i, j - 1] = t + 1;
                    }
                }

                temp = new Vector3Int(i - 1, j);
                if (CheckBound(temp))
                {
                    if (dis[i - 1, j] == 0)
                    {
                        dis[i - 1, j] = t + 1;
                    }
                }
            }

        for (var i = 0; i < n; i++)
        {
            for (var j = 0; j < n; j++)
            {
                var temp = new Vector3Int(i, j);
                if (dis[i, j] != 0 && CheckAttack(temp))
                {
                    result.Add(temp);
                }
            }
        }

        return result;
    }
}