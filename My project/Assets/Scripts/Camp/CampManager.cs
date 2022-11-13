using System.Collections;
using System.Collections.Generic;
using Camp;
using Model;
using UnityEngine;
using UnityEngine.UI;

public class CampManager : MonoBehaviour
{
    public Button[] buttonList;

    public GameObject GampCharacterImage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void enableBackGround()
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].interactable = true;
        }

        GampCharacterImage.GetComponent<CampCharacterImage>().active = true;
    }

    public void disableBackGround()
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].interactable = false;
        }
        GampCharacterImage.GetComponent<CampCharacterImage>().active = false;
    }
}
