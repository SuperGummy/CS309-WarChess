using System;
using System.Collections.Generic;
using Model;
using UnityEngine;
using UnityEngine.UI;

namespace BackPack
{
    public class BackPackManager : MonoBehaviour
    {
        public Button closeButton;
        [SerializeField] private BackPackSlot[] slots;
        [SerializeField] private GameObject slotHolder;
        private readonly Dictionary<string, int> _slotDictionary = new ();
        [SerializeField] private Equipment[] equipments;
        [SerializeField] private Mount[] mounts;
        [SerializeField] private Item[] items;


        void Awake()
        {
            closeButton.onClick.AddListener(GameManager.Instance.CloseBackPack);
            for(int i = 0; i < slots.Length; i ++)
            {
                slots[i] = slotHolder.transform.GetChild(i).gameObject.GetComponent<BackPackSlot>();
            }
        }

        void UpdateInfo()
        {
            _slotDictionary.Clear();
            //var equipments = DataManager.Instance.currentPlayer.equipments;
            int maxId = 0;
            for (int i = 0; i < equipments.Length; i++)
            {
                string equipmentName = equipments[i].name;
                if (_slotDictionary.ContainsKey(equipmentName))
                {
                    int slotId = _slotDictionary[equipmentName];
                    slots[slotId].AddItem(1);
                }
                else
                {
                    int slotId = maxId;
                    maxId += 1;
                    slots[slotId].InitializeEquipment(equipments[i]);
                    _slotDictionary.Add(equipmentName, slotId);
                }
            }

            //var mounts = DataManager.Instance.currentPlayer.mounts;
            for (int i = 0; i < mounts.Length; i++)
            {
                string mountName = mounts[i].name;
                if (_slotDictionary.ContainsKey(mountName))
                {
                    int slotId = _slotDictionary[mountName];
                    slots[slotId].AddItem(1);
                }
                else
                {
                    int slotId = maxId;
                    maxId += 1;
                    slots[slotId].InitializeMount(mounts[i]);
                    _slotDictionary.Add(mountName, slotId);
                }
            }
            
            //var items = DataManager.Instance.currentPlayer.items;
            for (int i = 0; i < items.Length; i++)
            {
                string itemName = items[i].name;
                if (_slotDictionary.ContainsKey(itemName))
                {
                    int slotId = _slotDictionary[itemName];
                    slots[slotId].AddItem(1);
                }
                else
                {
                    int slotId = maxId;
                    maxId += 1;
                    slots[slotId].InitializeItem(items[i]);
                    _slotDictionary.Add(itemName, slotId);
                }
            }

            for (; maxId < slots.Length; maxId ++)
            {
                slots[maxId].ClearSlot();
            }
        }

        void OnEnable()
        {
            UpdateInfo();
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

    }
}
