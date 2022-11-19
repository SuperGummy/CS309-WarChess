using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSet : MonoBehaviour
{
    public ItemHolder[] itemHolder;
    public GameObject itemHolderFrame;

    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < itemHolder.Length; i++)
        {
            itemHolder[i] = itemHolderFrame.transform.GetChild(i).gameObject.GetComponent<ItemHolder>();
            itemHolder[i].hold = 0;
            itemHolder[i].itemId = i;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void AddItem(GameObject item, int cnt)
    {
        Debug.Log(itemHolder.Length);
        for (int i = 0; i < itemHolder.Length; i++)
        {
            if (itemHolder[i].hold != 0 && itemHolder[i].item.name == item.name)
            {
                itemHolder[i].hold++;
                return;
            }
        }
        for (int i = 0; i < itemHolder.Length; i ++)
        {
            if (itemHolder[i].hold == 0)
            {
                itemHolder[i].hold = 1;
                itemHolder[i].item = item;
                return;
            }
        }
    }
}
