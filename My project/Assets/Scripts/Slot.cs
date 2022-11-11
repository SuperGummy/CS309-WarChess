using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ItemHolder itemHolder;
    public TextMeshProUGUI description;

    public Image slotIconGO;
    //public 
    public int slotId;
    public TextMeshProUGUI count;

    // Start is called before the first frame update
    void Start()
    {
        slotIconGO = transform.GetChild(0).gameObject.GetComponent<Image>();
        if (itemHolder == null || itemHolder.hold == 0)
        {
            slotIconGO.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateSlot()
    {
        if(itemHolder != null && itemHolder.hold > 0)
        {
            Debug.Log("Updated!");
            slotIconGO.sprite = itemHolder.item.GetComponent<ItemDisplay>().icon.sprite;
            description.text = itemHolder.item.GetComponent<ItemDisplay>().description.text;
            count.text = itemHolder.hold.ToString();
            Debug.Log("Active!");
            slotIconGO.gameObject.SetActive(true);
            count.gameObject.SetActive(true);
        }
        else
        {
            transform.Find("DescriptionPanel").gameObject.SetActive(false);
            count.gameObject.SetActive(false);
            slotIconGO.gameObject.SetActive(false);
        }
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (itemHolder != null && itemHolder.hold > 0)
        {
            transform.Find("DescriptionPanel").gameObject.SetActive(true);
        }
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        transform.Find("DescriptionPanel").gameObject.SetActive(false);
    }
}
