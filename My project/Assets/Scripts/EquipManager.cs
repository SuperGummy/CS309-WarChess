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
    private void Awake() => equipManager = this;
    void Start()
    {
        equipManager = this;
        ch = DataManager.Instance.GetCharacterByPosition(position);
        equipnames=new[] {"basic","sword","arrow","shield","cannon"};
        mountnames = new[] {"basic", "horse", "elephant", "fox"};
        itemnames = new[] {"basic", "a fish", "beer", "potion"};
        string side = DataManager.Instance.CheckCharacterSide(ch)==-1?"blue":"red";
        characterImage.sprite = RenderManager.Instance.GetCharacterImage(ch.characterClass,side);
        chname.text = ch.name;
        loadCharacter();
    }


    public void closeEquip()
    {
        GameManager.Instance.CloseEquip();
    }
    public void loadCharacter()
    {
        //load character;
        
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
            equips[i].index = i;
            equips[i].name = equipnames[i];
            for (int j = 0; j < DataManager.Instance.currentPlayer.equipments.Length; j++)
            {
                if(DataManager.Instance.currentPlayer.equipments[j]==null) continue;
                Debug.Log(DataManager.Instance.currentPlayer.equipments[j].name);
                if (DataManager.Instance.currentPlayer.equipments[j].name == equipnames[i])
                {
                    equips[i].number++;
                    equips[i].id = DataManager.Instance.currentPlayer.equipments[j].id;
                }
            }
            equips[i].updateUI();
        }
        for (int i = 0; i < mountnames.Length; i++)
        {
            mounts[i].ty = 1;
            mounts[i].number = 0;
            mounts[i].index = i;
            mounts[i].name = mountnames[i];
            for (int j = 0; j < DataManager.Instance.currentPlayer.mounts.Length; j++)
            {
                if(DataManager.Instance.currentPlayer.mounts[j]==null) continue;
                Debug.Log(DataManager.Instance.currentPlayer.mounts[j].name);
                if (DataManager.Instance.currentPlayer.mounts[j].name == mountnames[i])
                {
                    mounts[i].number++;
                    mounts[i].id = DataManager.Instance.currentPlayer.mounts[j].id;
                }
            }
            mounts[i].updateUI();
        }
        for (int i = 0; i < itemnames.Length; i++)
        {
            items[i].ty = 2;
            items[i].number = 0;
            items[i].index = i;
            items[i].name = itemnames[i];
            for (int j = 0; j < DataManager.Instance.currentPlayer.items.Length; j++)
            {
                if(DataManager.Instance.currentPlayer.items[j]==null) continue;
                Debug.Log(DataManager.Instance.currentPlayer.items[j].name);
                if (DataManager.Instance.currentPlayer.items[j].name == itemnames[i])
                {
                    items[i].number++;
                    items[i].id = DataManager.Instance.currentPlayer.items[i].id;
                }
            }
            items[i].updateUI();
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
