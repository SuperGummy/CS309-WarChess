using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfoFrame : MonoBehaviour
{
    private int x, y;

    public void Inform(int x, int y)
    {
        this.x = x;
        this.y = y;
        
    }
}