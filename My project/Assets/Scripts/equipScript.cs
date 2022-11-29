using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using Model;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static  EquipManager;
public class equipScript : MonoBehaviour
{
    public GameObject conf;
    public int id;
    public int ty;
    public TMP_Text name;
    public Image image;
    void Start()
    {
        updateUI();
    }

    public void updateUI()
    {
        if (ty == 0)
        {
            name.text = DataManager.Instance.currentPlayer.equipments[id].name;
            //image
        }
        if (ty == 1)
        {
            name.text = DataManager.Instance.currentPlayer.mounts[id].name;
            //image
        }

        if (ty == 2)
        {
            name.text = DataManager.Instance.currentPlayer.items[id].name;
        }
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
    public void equip()
    {
        equipManager.curTy = ty;
        equipManager.curId = id;
        confirmationPop();
        // if (equipManager.equipment.name =="")
        // {
        //     Debug.Log("null");
        //     confirmationPop();
        // }
        // else
        // {
        //     Debug.Log("not null");
        //     confirmationPop();
        // }
    }

    public void confirmationPop()
    {
        //Debug.Log(conf);
        conf.SetActive(true);
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
