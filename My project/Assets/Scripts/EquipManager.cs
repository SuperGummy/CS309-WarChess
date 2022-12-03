using System.Collections;
using System.Collections.Generic;
using Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Character ch;
    public Image characterImage;
    public TMP_Text chname;
    public Equipment equipment;
    public Mount mount;
    public int curTy, curId;
    public equipScript[] equips, mounts, items;
    public bool confirmed;
    public string[] equipnames,mountnames,itemnames;
    public static EquipManager equipManager;
    public static Vector3Int position;
    void Start()
    {
        equipManager = this;
        ch = DataManager.Instance.GetCharacterByPosition(position);
        equipnames=new[] {"lolipop","sword","arrow","shield","cannon"};
        mountnames = new[] {"bunny", "horse", "elephant", "fox"};
        itemnames = new[] {"apple", "a fish", "beer", "potion"};
        loadCharacter();
    }


    public void closeEquip()
    {
        GameManager.Instance.CloseEquip();
    }
    public void loadCharacter()
    {
        //load character;
        characterImage.sprite = RenderManager.Instance.GetCharacterImage(ch.characterClass,DataManager.Instance.currentPlayer.ToString());
        chname.text = ch.name;
        equipment = ch.equipment;
        mount = ch.mount;
        Debug.Log(ch.name);
        //equipment = new Equipment();
        //mount = new Mount();
        //TODO: load data 
        for (int i = 0; i < equipnames.Length; i++)
        {
            equips[i].ty = 0;
            equips[i].number = 0;
            equips[i].id = i;
            equips[i].name = equipnames[i];
            for (int j = 0; j < DataManager.Instance.currentPlayer.equipments.Length; j++)
            {
                if (DataManager.Instance.currentPlayer.equipments[j].name == equipnames[i])
                {
                    equips[i].number++;
                    equips[i].id = DataManager.Instance.currentPlayer.equipments[j].id;
                }
            }
        }
        for (int i = 0; i < mountnames.Length; i++)
        {
            mounts[i].ty = 1;
            mounts[i].number = 0;
            mounts[i].id = i;
            mounts[i].name = mountnames[i];
            for (int j = 0; j < DataManager.Instance.currentPlayer.mounts.Length; j++)
            {
                if (DataManager.Instance.currentPlayer.mounts[j].name == mountnames[i])
                {
                    mounts[i].number++;
                    mounts[i].id = DataManager.Instance.currentPlayer.mounts[j].id;
                }
            }
        }
        for (int i = 0; i < itemnames.Length; i++)
        {
            items[i].id = 2;
            items[i].number = 0;
            items[i].id = i;
            items[i].name = itemnames[i];
            for (int j = 0; j < DataManager.Instance.currentPlayer.items.Length; j++)
            {
                if (DataManager.Instance.currentPlayer.items[j].name == itemnames[i])
                {
                    items[i].number++;
                    items[i].id = DataManager.Instance.currentPlayer.items[i].id;
                }
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
