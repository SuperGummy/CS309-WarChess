using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinManager : MonoBehaviour
{
    public static int side;
    private bool isOn;

    [SerializeField] private TextMeshProUGUI winnerText;

    [SerializeField] private GameObject winPanel;
    [SerializeField] private Toggle animationButton;
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
        if (side == -1)
        {
            winnerText.text = "♦ " + "BLUE TEAM" + " ♦";
            winnerText.color = new Color(0.43f, 0.84f, 0.95f, winnerText.color.a);
        }
        else
        {
            winnerText.text = "♦ " + "RED TEAM" + " ♦";
            winnerText.color = new Color(1, 0.43f, 0.43f, winnerText.color.a);
        }
        
    }

    public void OnExit()
    {
        winPanel.SetActive(false);
        animationButton.isOn = false;
        isOn = false;
    }

    public void OnReturnMenu()
    {
        SceneManager.LoadSceneAsync("Start After Login");
    }

    public void OnAnimationButtonSelectChange()
    {
        isOn = animationButton.isOn;
        if(isOn) winPanel.SetActive(true);
        else winPanel.SetActive(false);
    }
}
