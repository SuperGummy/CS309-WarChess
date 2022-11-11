using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SellInfoBox : MonoBehaviour
{
    public ItemSet itemSet;
    public int slotId;
    public int value;
    public TextMeshProUGUI infoText;
    public GameObject confirmButton, cancelButton;
    // Start is called before the first frame update
    void Start()
    {
        confirmButton.GetComponent<Button>().onClick.AddListener(SellObjectReaction);
        cancelButton.GetComponent<Button>().onClick.AddListener(CloseFrame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        infoText.text = "Do you want to sell this object for " + value.ToString() + "$ ?";
    }

    void SellObjectReaction()
    {
        itemSet.itemHolder[slotId].RemoveItem(1);
        transform.parent.gameObject.GetComponent<BackPackManager>().UpdateInfo();
        CloseFrame();
    }

    public void SetItemSet(ItemSet itemSet)
    {
        this.itemSet = itemSet;
    }

    void CloseFrame()
    {
        transform.gameObject.SetActive(false);
    }
}
