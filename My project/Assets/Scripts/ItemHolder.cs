using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    public int hold;
    public GameObject item;
    public int itemId;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RemoveItem(int num)
    {
        hold -= num;
        if(hold <= 0)
        {
            item = null;
        }
    }
}
