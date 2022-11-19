using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampLevelUpButton : MonoBehaviour
{
    public GameObject CampManager;
    public GameObject levelUpPanel;
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
        levelUpPanel.SetActive(true);
        CampManager.GetComponent<CampManager>().disableBackGround();
    }
}
