using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Item")]
public class Item : ScriptableObject
{
    public string type;
    public string itemName;

    [TextArea(4, 4)]
    public string description;
    
    public int strength, health, defense, travelRange, attackRange;
    public int value;
    public Sprite icon;
}
