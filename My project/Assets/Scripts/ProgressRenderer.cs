using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProgressRenderer : MonoBehaviour
{
    public static ProgressRenderer Instance;
    [SerializeField] private ProgressbarSlider progressbarSlider;

    [SerializeField] private Slider slider;

    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    // Start is called before the first frame update

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        textMeshProUGUI.text = "Welcome home " + PlayerPrefs.GetString("username") + "!";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UnLoad()
    {
        SceneManager.UnloadSceneAsync("Loading");
    }

    public void SetSliderValue(int value)
    {
        slider.value = value;
    }

    public void TestButtonOnClick()
    {
        slider.value += 10;
    }
}
