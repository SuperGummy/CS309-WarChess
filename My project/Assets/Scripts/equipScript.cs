using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using Model;
using TMPro;
using Unity.VisualScripting;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;
using static  EquipManager;
using Color = System.Drawing.Color;

public class equipScript : MonoBehaviour
{
    public GameObject conf;
    public int id;
    public int index;
    public int ty;
    public string name;
    public TMP_Text nameText;
    public Image image;
    public int number;
    public TMP_Text numberText;
    public Button button,imageButton;
    void Start()
    {
        updateUI();
    }

    public void updateUI()
    {
        if (ty == 0)
        {
            //image = RenderManager.Instance.GetEquipmentImage();
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
            imageButton.interactable = true;
        }
        numberText.text = $"{number}";
        nameText.text = $"{name}";
    }

    public void imageClicked()
    {
        Debug.Log("Image clicked! name:"+name+" number: "+number);
        
        if (number > 0)
        {
            button.gameObject.SetActive(true);
            /*Transform[] father = GetComponentsInChildren<Transform>(true);

            foreach (var child in father)
            {
                if(child.name=="EquipButton") child.gameObject.SetActive(true);
            }*/
        }
        equipManager.loadCharacter();
    }
    public void equip()
    {
        equipManager.curTy = ty;
        equipManager.curId = index;
        //confirmationPop();
        if ((ty==0&&equipManager.ch.equipment == null)||(ty==1&&equipManager.ch.mount==null)||ty==2)
        {
            Debug.Log("null");
            //confirmationPop();
            confirmedEquip();
        }
        else
        {
            Debug.Log("not null");
            confirmationPop();
        }
    }

    public Equipment getEquipment()
    {
        for (int i = 0; i < DataManager.Instance.currentPlayer.equipments.Length; i++)
        {
            if(DataManager.Instance.currentPlayer.equipments[i]==null) continue;
            if (DataManager.Instance.currentPlayer.equipments[i].name == name)
                return DataManager.Instance.currentPlayer.equipments[i];
        }

        return new Equipment();
    }
    public Mount getMount()
    {
        for (int i = 0; i < DataManager.Instance.currentPlayer.mounts.Length; i++)
        {
            if(DataManager.Instance.currentPlayer.mounts[i]==null) continue;
            if (DataManager.Instance.currentPlayer.mounts[i].name == name)
                return DataManager.Instance.currentPlayer.mounts[i];
        }

        return new Mount();
    }
    public void confirmedEquip()
    {
        if (ty == 0)
        {
            if(equipManager.ch.equipment!=null) GameManager.Instance.ChangeEquipment(equipManager.ch.equipment.id,true);
            GameManager.Instance.ChangeEquipment(getEquipment().id, false);

            equipManager.ch.equipment = getEquipment();
            number--;
        }

        if (ty == 1)
        {
            if (equipManager.ch.mount != null)
            {
                Debug.Log("off:"+equipManager.ch.mount.name);
                GameManager.Instance.ChangeMount(equipManager.ch.mount.id,true);
            }
            GameManager.Instance.ChangeMount(getMount().id,false);
            equipManager.ch.mount = getMount();
            number--;
        }

        if (ty == 2)
        {
            GameManager.Instance.UseItem(id);
            number--;
            //TODO: consume Item
        }
        equipManager.loadCharacter();
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
