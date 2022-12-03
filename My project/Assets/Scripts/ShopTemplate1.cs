using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Model;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;
using static ShopManager;

public class ShopTemplate1 : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text costText;
    public Image image;
    public string name;
    public int id;
    public int ty;
    public int cost;
    public EquipmentClass ec;
    public MountClass mc;
    public ItemClass ic;
    public void updateUI()
    {
        titleText.text = name;
        if (ty == 0) image.sprite = RenderManager.Instance.GetEquipmentImage(ec);
        if (ty == 1) image.sprite = RenderManager.Instance.GetMountImage(mc);
        if (ty == 2) image.sprite = RenderManager.Instance.GetItemImage(ic);

    }
    public void buy()
    {
        if (shopManager.star < cost)
        {
            NotEnoughStar();
            return;
        }
        if(ty==0) GameManager.Instance.BuyEquipment(id);
        if(ty==1) GameManager.Instance.BuyMount(id);
        if(ty==2) GameManager.Instance.BuyItem(id);
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
