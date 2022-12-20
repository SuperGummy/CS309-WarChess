using System.Collections;
using System.Collections.Generic;
using Model;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Linq;
using TMPro;


public class RecruitManager : MonoBehaviour
{
    public GameObject confirm;
    public GameObject choose;
    public GameObject noPlace;
    public Button[] nextBtn;
    private int _id;
    public static Vector3Int Position;
    private Character[] _characters;
    public GameObject[] members;
    public Button[] recruitBtn;
    public Button[] closeBtn;
    public static bool Place;
    private Button _chosenBtn;


    private void OnClickNext(Button btn)
    {
        string btnName = btn.transform.GetComponentInChildren<TextMeshProUGUI>().text;
        string characterName = btnName.Split(" ")[1];
        foreach (Character character in _characters)
        {
            if ((character.name).Equals(characterName))
            {
                _id = character.id;
                break;
            }
        }
        
        // btn.enabled = false;
        
        confirm.SetActive(false);
        choose.SetActive(true);
    }
    
    
    public class NameNotFound: ApplicationException
    {
        public NameNotFound(string message): base(message)
        {
        }
    }

    private void OnClickRecruit(Button btn)
    {
        int type = -1;
        switch (btn.name)
        {
            case "RecruitMemberButton0":
                type = 2;
                break;
            case "RecruitMemberButton1":
                type = 0;
                break;
            case "RecruitMemberButton2":
                type = 1;
                break;
            default:
                throw new NameNotFound("Cannot find " + btn.name);
        }
        
        GameManager.Instance.ChooseNewCharacterPosition(_id, type);
        
        choose.SetActive(false);
        
    }

    void RenderInitial(Vector3Int pos)
    {
        Structure structure = DataManager.Instance.GetStructureByPosition(pos);
        _characters = structure.characters;
        // _characters = new Character[2];
        // _characters[0] = new Character();
        // _characters[1] = new Character();
        // _characters[0].attack = 10;
        // _characters[0].hp = 20;
        // _characters[0].defense = 15;
        // _characters[0].name = "flower";
        // _characters[1].attack = 20;
        // _characters[1].hp = 10;
        // _characters[1].defense = 25;
        // _characters[1].name = "sea";
        //
        // _characters[2].attack = 40;
        // _characters[2].hp = 50;
        // _characters[2].defense = 65;
        // _characters[2].name = "aaaa";

        for (int i = 0; i < 3; i++)
        {
            // Debug.Log("character " + i + ": " + _characters[i]);
            if (_characters[i] == null)
            {
                Button btn = nextBtn[i];
                btn.enabled = false;
                btn.GetComponentInChildren<TextMeshProUGUI>().text = "sold out!";
                btn.GetComponentInChildren<Image>().color = Color.gray;
                continue;
            }
            RecruitInfoFrame member = members[i].GetComponent<RecruitInfoFrame>();
            (member.defend).text = "defend: " + (_characters[i].defense);
            (member.life).text = "life: " + (_characters[i].hp);
            (member.naming).text = "name: " + (_characters[i].name);
            (member.strength).text = "attack: " + (_characters[i].attack);
            nextBtn[i].transform.GetComponentInChildren<TextMeshProUGUI>().text = "Get " + _characters[i].name;
        } 
    }

    private void OnEnable()
    {
        // _pos = new Vector3Int(1, 1, 0);
        // nextBtn[2].enabled = false;
        
        RenderInitial(Position);
        // _place = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        _chosenBtn = null;
        foreach (Button btn in closeBtn)
        {
            btn.onClick.AddListener(OnClickClose);
        }
        if (Place)
        {
            confirm.SetActive(true);
            choose.SetActive(false);
            for(int i = 0; i < 3; i++)
            {
                Button btn = nextBtn[i];
                btn.onClick.AddListener(delegate { OnClickNext(btn); });
            }

            foreach (Button btn in recruitBtn)
            {
                btn.onClick.AddListener(delegate { OnClickRecruit(btn); });
            }
        }
        else
        {
           confirm.SetActive(false);
           choose.SetActive(false);
           noPlace.SetActive(true); 
        }

    }

    public void OnClickClose()
    {
        if (null != _chosenBtn)
        {
            _chosenBtn.enabled = true;
        }
        confirm.SetActive(false);
        choose.SetActive(false);
        noPlace.SetActive(false); 
        
        GameManager.Instance.CloseRecruit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
