using System.Collections;
using System.Collections.Generic;
using Camp;
using Model;
using UnityEngine;

public class CampStartTrainingButton : MonoBehaviour
{
    public int updateType;

    [SerializeField] private GameObject closeButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        GameManager.Instance.UpdateCharacterAtCamp(updateType, closeButton);
    }
}
