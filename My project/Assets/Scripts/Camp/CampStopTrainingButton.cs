using System.Collections;
using System.Collections.Generic;
using Camp;
using UnityEngine;

public class CampStopTrainingButton : MonoBehaviour
{
    public GameObject CampCharacterHolder;
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
        // TODO: stop training
        CampCharacterHolder.GetComponent<CampCharacterHolder>().UpdateInfo();
    }
}
