using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;

public class EquipManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Character ch;
    public Equipment equipment;
    public Mount mount;
    public int curTy, curId;
    public equipScript[] equips, mounts, items;
    public bool confirmed;
    public static EquipManager equipManager;
    
    void Start()
    {
        loadCharacter();
    }

    public void loadCharacter()
    {
        //load character;
        //equipment = ch.equipment;
        //mount = ch.mount;
        equipment = new Equipment();
        mount = new Mount();
        //TODO: load data 
        for (int i = 0; i < equips.Length; i++)
        {
            equips[i].number = 0;
            for (int j = 0; j < DataManager.Instance.currentPlayer.equipments.Length; j++)
            {
                if (DataManager.Instance.currentPlayer.equipments[j].name == equips[i].name) equips[i].number++;
            }
        }
        for (int i = 0; i < mounts.Length; i++)
        {
            mounts[i].number = 0;
            for (int j = 0; j < DataManager.Instance.currentPlayer.mounts.Length; j++)
            {
                if (DataManager.Instance.currentPlayer.mounts[j].name == mounts[i].name) mounts[i].number++;
            }
        }
        for (int i = 0; i < items.Length; i++)
        {
            items[i].number = 0;
            for (int j = 0; j < DataManager.Instance.currentPlayer.items.Length; j++)
            {
                if (DataManager.Instance.currentPlayer.items[j].name == items[i].name) items[i].number++;
            }
        }
    }

    public void confirmedEquip()
    {
        if(curTy==0) equips[curId].confirmedEquip();
        if(curTy==1) mounts[curId].confirmedEquip();
        if(curTy==2) items[curId].confirmedEquip();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
