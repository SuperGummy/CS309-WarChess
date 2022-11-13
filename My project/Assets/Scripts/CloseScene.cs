using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CloseScene : MonoBehaviour
{
    public Button closeButton;
    public GameManager gameManager;

    public String SceneName;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        //UnloadSceneAsync
        gameManager.gridEnable = true;
		SceneManager.UnloadSceneAsync(SceneName);
    }
}
