using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager shopManager;
    public int star,tot;
    public TMP_Text startext;
    public List<ShopItem> shopItemList;
    public ShopItem[] ShopItemSos;
    public ShopTemplate1[] ShopTemplate1s;
    private void Awake() => shopManager = this;

    public void Start()
    {
        star = getStar();
        tot = getTot();
        for (int i = 0; i < tot; i++)
        {
            shopItemList.Add(new ShopItem());
            shopItemList[i].id = i;
        }
        loadPanels();
        updateUI();
    }

    public int getTot()
    {
        return 5;
    }

    public int getStar()
    {
        return 100;
    }
    public void updateUI()
    {
        startext.text = $"{star}";
        foreach (var shopItem in shopItemList) shopItem.update();
    }

    public void loadPanels()
    {
        for (int i = 0; i < ShopItemSos.Length; i++)
        {
            ShopTemplate1s[i].titleText.text = ShopItemSos[i].title;
            ShopTemplate1s[i].costText.text = ShopItemSos[i].cost.ToString();
            ShopTemplate1s[i].id = ShopItemSos[i].id;
            ShopTemplate1s[i].cost = ShopItemSos[i].cost;
        }
    }
}
