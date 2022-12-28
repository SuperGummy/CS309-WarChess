using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoseManager : MonoBehaviour
{
    private bool isOn;
    
    [SerializeField] private GameObject losePanel;
    [SerializeField] private Toggle animationButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnExit()
    {
        losePanel.SetActive(false);
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
        if(isOn) losePanel.SetActive(true);
        else losePanel.SetActive(false);
    }
}
