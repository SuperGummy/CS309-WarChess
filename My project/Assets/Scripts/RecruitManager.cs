using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RecruitManager : MonoBehaviour
{
    public Button next;
    public GameObject confirm;
    public GameObject choose;
    public Button recruitBtn1;
    public Button recruitBtn2;
    public Button recruitBtn3;

    private void OnClickNext()
    {
        confirm.SetActive(false);
        choose.SetActive(true);
    }

    private void OnClickRecruit()
    {
        choose.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        next.onClick.AddListener(OnClickNext);
        recruitBtn1.onClick.AddListener(OnClickRecruit);
        recruitBtn2.onClick.AddListener(OnClickRecruit);
        recruitBtn3.onClick.AddListener(OnClickRecruit);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
