using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Bson;
using TMPro;
using UnityEngine;
using static EquipManager;
public class EquipConfirm : MonoBehaviour
{
    public TMP_Text replace;

    private void Update()
    {
        if (equipManager.curId == 0)
        {
            replace.text = $"Replace {equipManager.ch.equipment.name}?";
        }
        else
        {
            replace.text = $"Replace {equipManager.ch.mount.name}?";
        }
    }

    public void yesClicked()
    {
        equipManager.confirmed = true;
        equipManager.confirmedEquip();
        //changeEquip();
    }

    public void noClicked()
    {
        equipManager.confirmed = false;
    }
}
