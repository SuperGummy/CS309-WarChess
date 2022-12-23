using System.Collections;
using System.Collections.Generic;
using Camp;
using UnityEngine;

public class CampLevelUpButton : MonoBehaviour
{
    public GameObject CampManager;
    public GameObject levelUpPanel;
    public GameObject showCantUpdateInfoPanel;

    [SerializeField] private CampCharacterHolder campCharacterHolder;
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
        if (campCharacterHolder.structureInfo.remainingRound > 0)
        {
            showCantUpdateInfoPanel.SetActive(true);
        }
        else levelUpPanel.SetActive(true);
        CampManager.GetComponent<CampManager>().DisableBackGround();
    }
}
