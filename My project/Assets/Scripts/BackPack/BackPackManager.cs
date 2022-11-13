using UnityEngine;

namespace BackPack
{
    public class BackPackManager : MonoBehaviour
    {
        public BackPackSlot[] slots;
        public GameObject backPackFrame;
        public GameObject slotHolder;
        public GameObject itemSet;
        public GameManager gameManager;


        void Awake()
        {
            for(int i = 0; i < slots.Length; i ++)
            {
                slots[i] = slotHolder.transform.GetChild(i).gameObject.GetComponent<BackPackSlot>();
                Debug.Log("added listener");
                slots[i].slotId = i;
                slots[i].UpdateSlot();
            }
        }

        // void CallSellObjectFrame()
        // {
        //     GameObject currentButton = EventSystem.current.currentSelectedGameObject;
        //     Slot parentSlot = currentButton.gameObject.GetComponent<Slot>();
        //     if(parentSlot.itemHolder == null || parentSlot.itemHolder.hold == 0)
        //     {
        //         return;
        //     }
        //     sellInfoBox.GetComponent<SellInfoBox>().slotId = parentSlot.slotId;
        //     sellInfoBox.GetComponent<SellInfoBox>().value = parentSlot.itemHolder.item.GetComponent<ItemDisplay>().item.value;
        //     sellInfoBox.SetActive(true);
        // }

        // Update is called once per frame
        void Update()
        {
        
        }

        void Start()
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            itemSet = GameObject.Find("ItemSet");
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
}
