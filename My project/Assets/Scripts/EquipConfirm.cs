using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Bson;
using UnityEngine;
using static EquipManager;
public class EquipConfirm : MonoBehaviour
{
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
