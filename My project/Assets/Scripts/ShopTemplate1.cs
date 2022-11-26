using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ShopManager;

public class ShopTemplate1 : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text costText;
    public Image image;
    public int id;
    public int cost;

    public void buy()
    {
        if (shopManager.star < cost)
        {
            NotEnoughStar();
            return;
        }
        shopManager.star -= cost;
        //Debug.Log("ababa");
        addChain();
        shopManager.updateUI();
    }

    public void addChain()
    {
        //Debug.Log("addChain");
        Transform[] father = GetComponentsInChildren<Transform>(true);

        foreach (var child in father)
        {
            if(child.name=="Button") child.gameObject.SetActive(false);
            if(child.name=="chain1"||child.name=="chain2") child.gameObject.SetActive(true);
            //Debug.Log(child.name);
        }
    }
    public void NotEnoughStar()
    {
        
    }
}
