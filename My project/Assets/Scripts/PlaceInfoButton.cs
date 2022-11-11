using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceInfoButton : MonoBehaviour
{
    public int x, y;
    public bool showPlace;
    public GameObject placeInfoFrame;
    public GameManager gameManager;

    public int state;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        state = 0;
        // TODO: check whether there is a showable place
        showPlace = true;
    }

    public void OnClick()
    {
        if (gameManager.gridEnable && showPlace)
        {
            state ^= 1;
            if (state == 1)
            {
                placeInfoFrame.GetComponent<PlaceInfoFrame>().Inform(x, y);
                placeInfoFrame.SetActive(true);
            }
            else
            {
                placeInfoFrame.SetActive(false);
            }   
        }
    }
}
