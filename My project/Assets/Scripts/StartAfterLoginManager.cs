using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;
using UnityEngine.UI;

public class StartAfterLoginManager : MonoBehaviour
{
    [SerializeField] private bool isOn;

    [SerializeField] private GameObject levelPanel;

    [SerializeField] private Toggle singlePlayerButton;

    private static string _username;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSinglePlayerSelectChange()
    {
        isOn = singlePlayerButton.isOn;
        if(isOn) levelPanel.SetActive(true);
        else levelPanel.SetActive(false);
    }
}
