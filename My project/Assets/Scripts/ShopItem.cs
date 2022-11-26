using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ShopManager;
[CreateAssetMenu(fileName = "shopMenu",menuName = "Scriptable Objects/New Shop Item",order = 1)]
public class ShopItem : ScriptableObject
{
    public string title;
    public int id;
    public int cost;

    public void update()
    {
        //title=DataManager.Instance.currentPlayer.shop
    }
}
