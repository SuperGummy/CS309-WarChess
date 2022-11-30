using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using Model;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static  EquipManager;
using Button = UnityEngine.UIElements.Button;
using Color = System.Drawing.Color;

public class equipScript : MonoBehaviour
{
    public GameObject conf;
    public int id;
    public int ty;
    public string name;
    public TMP_Text nameText;
    public Image image;
    public int number;
    public TMP_Text numberText;
    void Start()
    {
        updateUI();
    }

    public void updateUI()
    {
        if (ty == 0)
        {
            //name.text = DataManager.Instance.currentPlayer.equipments[id].name;
            //image
        }
        if (ty == 1)
        {
            //name.text = DataManager.Instance.currentPlayer.mounts[id].name;
            //image
        }

        if (ty == 2)
        {
            //name.text = DataManager.Instance.currentPlayer.items[id].name;
        }

        if (number == 0)
        {
            nameText.color=new UnityEngine.Color(0, 0, 0);
            image.color = new UnityEngine.Color(0, 0, 0);
        }
        else
        {
            nameText.color=new UnityEngine.Color(1, 1, 1);
            image.color = new UnityEngine.Color(1, 1, 1);
        }

        numberText.text = $"{number}";
        nameText.text = $"{name}";
        equipManager.loadCharacter();
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

    public void imageClicked()
    {
        if (number > 0)
        {
            Transform[] father = GetComponentsInChildren<Transform>(true);

            foreach (var child in father)
            {
                if(child.name=="EquipButton") child.gameObject.SetActive(true);
            }
        }
    }
    public void equip()
    {
//        equipManager.curTy = ty;
//        equipManager.curId = id;
        confirmationPop();
 /*       if (equipManager.equipment.name =="")
        {
            Debug.Log("null");
            //confirmationPop();
            confirmedEquip();
        }
        else
        {
            Debug.Log("not null");
            confirmationPop();
        }*/
    }

    public Equipment getEquipment()
    {
        for (int i = 0; i < DataManager.Instance.currentPlayer.equipments.Length; i++)
        {
            if (DataManager.Instance.currentPlayer.equipments[i].name == name)
                return DataManager.Instance.currentPlayer.equipments[i];
        }

        return new Equipment();
    }
    public Mount getMount()
    {
        for (int i = 0; i < DataManager.Instance.currentPlayer.mounts.Length; i++)
        {
            if (DataManager.Instance.currentPlayer.mounts[i].name == name)
                return DataManager.Instance.currentPlayer.mounts[i];
        }

        return new Mount();
    }
    public void confirmedEquip()
    {
        if (ty == 0)
        {
            equipManager.ch.equipment = getEquipment();
            number--;
        }

        if (ty == 1)
        {
            equipManager.ch.mount = getMount();
            number--;
        }

        if (ty == 2)
        {
            number--;
            //TODO: consume Item
        }
        updateUI();
    }
    public void confirmationPop()
    {
        Debug.Log(conf);
        equipManager.confirmed = false;
        conf.SetActive(true);
        
    }
    // Update is called once per frame
    void Update()
    {
        /*if (equipManager.confirmed == true & equipManager.curTy == ty & equipManager.curId == id)
        {
            equipManager.confirmed = false;
            confirmedEquip();
        }*/
    }
}
