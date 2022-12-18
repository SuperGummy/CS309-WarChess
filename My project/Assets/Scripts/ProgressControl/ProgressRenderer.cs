using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class ProgressRenderer : MonoBehaviour
{
    public static ProgressRenderer Instance;
    [SerializeField] private ProgressbarSlider progressbarSlider;

    [SerializeField] private Slider slider;

    [SerializeField] private TextMeshProUGUI textMeshProUGUI;

    private int _progressValue;

    private float _currentVelocity = 0;
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
        float currentScore = Mathf.SmoothDamp(slider.value, _progressValue, ref _currentVelocity, 50 * Time.deltaTime);
        if (currentScore - slider.value < 1 && slider.value + 3 <= _progressValue)
        {
            currentScore = slider.value + 4;
        }
        slider.value = (int) currentScore;
    }

    public void UnLoad()
    {
        SceneManager.UnloadSceneAsync("Loading");
    }

    public void SetSliderValue(int value)
    {
        Debug.Log(value);
        _progressValue = value;
    }

    public void TestButtonOnClick()
    {
        slider.value += 10;
    }
}
