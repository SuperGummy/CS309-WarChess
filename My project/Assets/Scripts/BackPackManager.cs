using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BackPackManager : MonoBehaviour
{
    public Slot[] slots;
    public GameObject backPackFrame;
    public GameObject slotHolder;
    public GameObject itemSet;
    public GameManager gameManager;
    public GameObject sellInfoBox;


    void Awake()
    {
        for(int i = 0; i < slots.Length; i ++)
        {
            slots[i] = slotHolder.transform.GetChild(i).gameObject.GetComponent<Slot>();
            Debug.Log("added listener");
            slots[i].gameObject.GetComponent<Button>().onClick.AddListener(CallSellObjectFrame);
            slots[i].slotId = i;
            slots[i].UpdateSlot();
        }
    }

    void CallSellObjectFrame()
    {
        GameObject currentButton = EventSystem.current.currentSelectedGameObject;
        Slot parentSlot = currentButton.gameObject.GetComponent<Slot>();
        if(parentSlot.itemHolder == null || parentSlot.itemHolder.hold == 0)
        {
            return;
        }
        sellInfoBox.GetComponent<SellInfoBox>().slotId = parentSlot.slotId;
        sellInfoBox.GetComponent<SellInfoBox>().value = parentSlot.itemHolder.item.GetComponent<ItemDisplay>().item.value;
        sellInfoBox.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Start()
    {
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		itemSet = GameObject.Find("ItemSet");
		sellInfoBox.GetComponent<SellInfoBox>().SetItemSet(itemSet.GetComponent<ItemSet>());
        UpdateInfo();
    }

    public void UpdateInfo()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].itemHolder = itemSet.GetComponent<ItemSet>().itemHolder[i].GetComponent<ItemHolder>();
            slots[i].UpdateSlot();
        }
    }

}
