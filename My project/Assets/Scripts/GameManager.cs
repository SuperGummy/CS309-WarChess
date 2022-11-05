using System.Collections;
using System.Collections.Generic;
using TMPro.SpriteAssetUtilities;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Button recruitBtn;
    public GameObject recruitImg;

    private void RecruitOnClick()
    {
        recruitImg.SetActive(true);
    }
    void Start()
    {
        recruitBtn.onClick.AddListener(RecruitOnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
