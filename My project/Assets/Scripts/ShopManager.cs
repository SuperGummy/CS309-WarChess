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
    public int star,tot;
    public TMP_Text startext;
    public List<ShopItem> shopItemList;
    public ShopTemplate1[] ShopTemplate0,ShopTemplate1,ShopTemplate2;
    private void Awake() => shopManager = this;
    public Button closeBtn;
    
    void Start()
    {
        star = getStar();
        tot = getTot();
        for (int i = 0; i < tot; i++)
        {
            shopItemList.Add(new ShopItem());
            shopItemList[i].id = i;
        }
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
         return 0;
    }
    public int getStar()
    {
        return (int)DataManager.Instance.currentPlayer.stars;
        return 100;
    }
    public void updateUI()
    {
        startext.text = $"{star}";
        foreach (var shopItem in shopItemList) shopItem.update();
    }

    public void loadPanels()
    {
        int t = Math.Max(gettotal(0), Math.Max(gettotal(1), gettotal(2)));
        Debug.Log(t);
        for (int j = 0; j < Math.Max(gettotal(0), Math.Max(gettotal(1), gettotal(2))); j++)
        {
            {
                Equipment a = DataManager.Instance.currentPlayer.equipments[j];
                ShopTemplate0[j].titleText.text = a.name;
                ShopTemplate0[j].costText.text = "7";
                ShopTemplate0[j].id = a.id;
                ShopTemplate0[j].ty = 0;
                

                {
                    Mount b = DataManager.Instance.currentPlayer.mounts[j];
                    ShopTemplate1[j].titleText.text = b.name;
                    ShopTemplate1[j].costText.text = "7";
                    ShopTemplate1[j].id = b.id;
                    ShopTemplate1[j].ty = 1;
                }

                {
                    var c = DataManager.Instance.currentPlayer.items[j];
                    ShopTemplate2[j].titleText.text = c.name;
                    ShopTemplate2[j].costText.text = "7";
                    ShopTemplate2[j].id = c.id;
                    ShopTemplate2[j].ty = 2;
                }
            }
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
