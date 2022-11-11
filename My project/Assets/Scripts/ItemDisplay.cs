using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ItemDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI type;
    public TextMeshProUGUI description;

    public TextMeshProUGUI strengthText, healthText, defenseText, travelRangeText, attackRangeText;
    public TextMeshProUGUI valueText;
    public Image icon;
    public Item item;

    // Start is called before the first frame update
    void Start()
    {
        icon.sprite = item.icon;
        description.text = item.description;
        /*
        type.text = item.type;
        description.text = item.description;
        strengthText.text = item.strength.ToString();
        healthText.text = item.health.ToString();
        defenseText.text = item.defense.ToString();
        travelRangeText.text = item.travelRange.ToString();
        attackRangeText.text = item.attackRange.ToString();
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        transform.Find("DescriptionPanel").gameObject.SetActive(true);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        transform.Find("DescriptionPanel").gameObject.SetActive(false);
    }
}
