using System;
using System.Collections;
using System.Collections.Generic;
using Model;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager shopManager;
    public int star;
    public TMP_Text startext;
    public ShopTemplate1[] ShopTemplate0,ShopTemplate1,ShopTemplate2;
    private void Awake() => shopManager = this;
    public Button closeBtn;
    
    void Start()
    {
        star = getStar();
        //tot = getTot();
        /*for (int i = 0; i < tot; i++)
        {
            shopItemList.Add(new ShopItem());
            shopItemList[i].id = i;
        }*/
//        closeBtn.onClick.AddListener(CloseShop);
        loadPanels();
        updateUI();
    }

    void Update()
    {
        star = getStar();
    }
    public void CloseShop()
    {
        GameManager.Instance.CloseShop();
    }
    public int getTot()
    {
        return 5;
    }

    public int gettotal(int x)
    {
        if (x == 0) return DataManager.Instance.currentPlayer.shop.equipments.Length;
        if (x == 1) return DataManager.Instance.currentPlayer.shop.mounts.Length;
        if (x == 2) return DataManager.Instance.currentPlayer.shop.items.Length;
        Debug.Log("error");
         return 0;
    }
    public int getStar()
    {
        return (int)DataManager.Instance.currentPlayer.stars;
        return 100;
    }
    public void updateUI()
    {
        star = getStar();
        startext.text = $"{star}";
        foreach (var shopItem in ShopTemplate0) shopItem.updateUI();
        foreach (var shopItem in ShopTemplate1) shopItem.updateUI();
        foreach (var shopItem in ShopTemplate2) shopItem.updateUI();
    }

    public void loadPanels()
    {
        int t = Math.Max(gettotal(0), Math.Max(gettotal(1), gettotal(2)));
        Debug.Log("shopManager.loadpanel: total0="+gettotal(0));
        for (int j = 0; j < gettotal(0); j++)
        {
            Equipment a = DataManager.Instance.currentPlayer.shop.equipments[j];
            if (a == null)
            {
                ShopTemplate0[j].addChain();
                continue;
            }
            Debug.Log("----------shop in equipment-----------"+a.name);
            ShopTemplate0[j].titleText.text = a.name;
            ShopTemplate0[j].costText.text = "7";
            ShopTemplate0[j].id = DataManager.Instance.currentPlayer.shop.index[0,j];
            ShopTemplate0[j].name = a.name;
            ShopTemplate0[j].ec = a.equipmentClass;
            Debug.Log("equipmentclass: "+a.equipmentClass);
            ShopTemplate0[j].ty = 0;
        }
        for (int j = 0; j <gettotal(1); j++)
        {
            Mount b = DataManager.Instance.currentPlayer.shop.mounts[j];
            if (b == null)
            {
                ShopTemplate1[j].addChain();
                continue;
            }
            ShopTemplate1[j].titleText.text = b.name;
            ShopTemplate1[j].costText.text = "7";
            ShopTemplate1[j].id = DataManager.Instance.currentPlayer.shop.index[2,j];
            ShopTemplate1[j].name = b.name;
            ShopTemplate1[j].mc = b.mountClass;
            ShopTemplate1[j].ty = 1;
        }
        for (int j = 0; j < gettotal(2); j++)
        {
            var c = DataManager.Instance.currentPlayer.shop.items[j];
            if (c == null)
            {
                ShopTemplate2[j].addChain();
                continue;
            }
            ShopTemplate2[j].titleText.text = c.name;
            ShopTemplate2[j].costText.text = "7";
            ShopTemplate2[j].id = DataManager.Instance.currentPlayer.shop.index[1,j];
            ShopTemplate2[j].name = c.name;
            ShopTemplate2[j].ic = c.itemClass;
            ShopTemplate2[j].ty = 2;
        }

        for (int j = gettotal(0); j < ShopTemplate0.Length; j++)
        {
            ShopTemplate0[j].addChain();
        }
        for (int j = gettotal(1); j < ShopTemplate1.Length; j++)
        {
            ShopTemplate1[j].addChain();
        }
        for (int j = gettotal(2); j < ShopTemplate2.Length; j++)
        {
            ShopTemplate2[j].addChain();
        }
    }
}
