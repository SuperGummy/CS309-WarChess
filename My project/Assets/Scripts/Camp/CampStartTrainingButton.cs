using System.Collections;
using System.Collections.Generic;
using Camp;
using Model;
using UnityEngine;

public class CampStartTrainingButton : MonoBehaviour
{
    public GameObject CampCharacterHolder;

    public int updateType;
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
        // TODO: start training according to update type;
        CampCharacterHolder.GetComponent<CampCharacterHolder>().UpdateInfo();
    }
}
