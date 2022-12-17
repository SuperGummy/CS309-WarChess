using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] private GameObject switchRoundAnimationPrefab;

    [SerializeField] private GameObject canvas;
    [SerializeField] private Vector3Int position;
    
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowRoundChange()
    {
        var animatedObject = Instantiate(switchRoundAnimationPrefab, position, Quaternion.identity);
        animatedObject.transform.SetParent(canvas.transform, false);
        Debug.Log(animatedObject.transform.position);
        string sideString;
        if (DataManager.Instance.currentPlayer.id == DataManager.Instance.player1.id)
            sideString = "blue";
        else
            sideString = "red";
        animatedObject.GetComponent<SwitchRoundAnimation>().SetText(sideString);
    }
}
