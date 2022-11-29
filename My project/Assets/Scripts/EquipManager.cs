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

        for (int i = 0; i < equips.Length; i++)
        {
            equips[i].id = i;
            equips[i].ty = 0;
        }

        for (int i = 0; i < mounts.Length; i++)
        {
            mounts[i].id = i;
            mounts[i].ty = 1;
        }

        for (int i = 1; i < items.Length; i++)
        {
            items[i].id = i;
            items[i].ty = 2;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
