using System;
using System.Collections;
using System.Collections.Generic;
using Camp;
using Model;
using UnityEngine;
using UnityEngine.UI;

public class CampManager : MonoBehaviour
{
    public Button[] buttonList;
    public GameObject campCharacterImage;
    public static Vector3Int position;
    public Button closeButton;
    [SerializeField] private GameObject campPropObject;
    [SerializeField] private GameObject campCharacterHolderObject;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        UpdateInfo();
        closeButton.onClick.AddListener(GameManager.Instance.CloseCamp);
    }

    public void UpdateInfo()
    {
        Debug.Log("Position? " + position);
        campPropObject.GetComponent<CampProp>().UpdateInfo(position);
        campCharacterHolderObject.GetComponent<CampCharacterHolder>().UpdateInfo(position);
    }
    
    public void UpdateInfo(Vector3Int _position)
    {
        position = _position;
        campPropObject.GetComponent<CampProp>().UpdateInfo(position);
        campCharacterHolderObject.GetComponent<CampCharacterHolder>().UpdateInfo(position);
    }

    public void EnableBackGround()
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].interactable = true;
        }

        campCharacterImage.GetComponent<CampCharacterImage>().active = true;
    }

    public void DisableBackGround()
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].interactable = false;
        }
        campCharacterImage.GetComponent<CampCharacterImage>().active = false;
    }
}
