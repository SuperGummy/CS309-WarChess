using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfoButton : MonoBehaviour
{
    public int x, y;
    public bool showCharacter;
    public GameObject characterInfoFrame;
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
        // TODO: check whether there is a showable character
        showCharacter = true;
    }

    public void OnClick()
    {
        if (gameManager.gridEnable && showCharacter)
        {
            state ^= 1;
            if (state == 1)
            {
                characterInfoFrame.GetComponent<CharacterInfoFrame>().Inform(x, y);
                characterInfoFrame.SetActive(true);
            }
            else
            {
                characterInfoFrame.SetActive(false);
            }
        }
    }
}
