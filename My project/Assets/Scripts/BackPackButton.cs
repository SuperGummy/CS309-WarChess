using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackPackButton : MonoBehaviour
{
    public GameObject backPackFrame;
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickBackPack()
    {
        // backPackFrame.SetActive(true);
        if (gameManager.gridEnable)
        {
            gameManager.gridEnable = false;
            SceneManager.LoadScene("Back Pack", LoadSceneMode.Additive);
        }
    }
}
