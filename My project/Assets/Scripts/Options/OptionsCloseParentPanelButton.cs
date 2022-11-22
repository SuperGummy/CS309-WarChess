using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsCloseParentPanelButton : MonoBehaviour
{
    [SerializeField] private GameObject optionButtonsLayout;
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
        optionButtonsLayout.SetActive(true);
        this.transform.parent.gameObject.SetActive(false);
    }
}
