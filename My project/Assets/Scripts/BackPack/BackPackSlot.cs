using Model;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BackPack
{
    public class BackPackSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public TextMeshProUGUI description;
        public Image slotIcon;
        public TextMeshProUGUI count;
        [SerializeField] private int countNumber;
        [SerializeField] private GameObject descriptionPanel;
        public TextMeshProUGUI emptyText;

        // Update is called once per frame
        void Update()
        {
        
        }

        public void AddItem(int x)
        {
            countNumber += x;
            UpdateCount();
        }

        public void UpdateCount()
        {
            count.text = countNumber.ToString();
            if (countNumber > 0) {
                count.gameObject.SetActive(true);
                emptyText.gameObject.SetActive(false);
            }
            else
            {
                ClearSlot();
            }
        }

        public void UpdateCount(int x)
        {
            countNumber = x;
            UpdateCount();
        }

        public void InitializeEquipment(Equipment equipment)
        {
            slotIcon.sprite = RenderManager.Instance.GetEquipmentImage(equipment.equipmentClass);
            description.text = equipment.description;
            UpdateCount(1);
            slotIcon.gameObject.SetActive(true);
        }
        
        public void InitializeMount(Mount mount)
        {
            slotIcon.sprite = RenderManager.Instance.GetMountImage(mount.mountClass);
            description.text = mount.description;
            UpdateCount(1);
            slotIcon.gameObject.SetActive(true);
        }
        
        public void InitializeItem(Item item)
        {
            slotIcon.sprite = RenderManager.Instance.GetItemImage(item.itemClass);
            description.text = item.description;
            UpdateCount(1);
            slotIcon.gameObject.SetActive(true);
        }

        public void ClearSlot()
        {
            slotIcon.gameObject.SetActive(false);
            descriptionPanel.SetActive(false);
            countNumber = 0;
            count.gameObject.SetActive(false);
            emptyText.gameObject.SetActive(true);
            Debug.Log("Should clear slot!");
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (countNumber > 0)
            {
                descriptionPanel.SetActive(true);
            }
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            descriptionPanel.SetActive(false);
        }

        void OnClick()
        {
            
        }
    }
}
