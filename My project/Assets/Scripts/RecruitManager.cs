using System.Collections;
using System.Collections.Generic;
using Model;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RecruitManager : MonoBehaviour
{
    public Button next;
    public GameObject confirm;
    public GameObject choose;
    public Button recruitBtn1;
    public Button recruitBtn2;
    public Button recruitBtn3;
    public Button next1;
    public Button next2;
    public Member role;
    private void OnClickNext()
    {
        
        confirm.SetActive(false);
        choose.SetActive(true);
    }

    private void OnClickRecruit()
    {
        choose.SetActive(false);
        SceneManager.LoadScene("Game");
        
    }
    // Start is called before the first frame update
    void Start()
    {
        confirm.SetActive(true);
        next.onClick.AddListener(OnClickNext);
        next1.onClick.AddListener(OnClickNext);
        next2.onClick.AddListener(OnClickNext);
        recruitBtn1.onClick.AddListener(OnClickRecruit);
        recruitBtn2.onClick.AddListener(OnClickRecruit);
        recruitBtn3.onClick.AddListener(OnClickRecruit);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
