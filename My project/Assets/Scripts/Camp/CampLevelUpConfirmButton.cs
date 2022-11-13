using System.Collections;
using System.Collections.Generic;
using Camp;
using UnityEngine;

public class CampLevelUpConfirmButton : MonoBehaviour
{
    public GameObject CampProp;

    public GameObject CloseButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick() 
    {
        //TODO: spend money to level up camp
        CampProp.GetComponent<CampProp>().UpdateInfo();
        CloseButton.GetComponent<CampCloseParentPanelButton>().OnClick();
    }
}
