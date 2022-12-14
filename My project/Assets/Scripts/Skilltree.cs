using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Skilltree : MonoBehaviour
{
    public static Skilltree skilltree;
    public static int TOTSKILLS = 11;
    public static Vector3Int position;
    private void Awake() => skilltree = this;
    public Structure structure;
    public Character character;
    public int[] SkillLevels;
    public int[] SkillCaps;
    public int[] SkillRounds;
    public int[] SkillStars;
    public string[] SkillNames;
    public string[] SkillDescriptions;
    public int[] techStatus;
    public TMP_Text starText;
    
    public List<Skill> skillList;
    public GameObject skillHolder;

    public List<GameObject> ConnectorList;
    public GameObject ConnectorHolder;

    public int stars;

    public void CloseTechTree()
    {
        GameManager.Instance.CloseTechnologies();
    }

    private int getStars()
    {
        return (int)DataManager.Instance.currentPlayer.stars;
    }
    public void Update()
    {
        stars = getStars();
    }

    public void Start()
    {
        stars = getStars();
        structure = DataManager.Instance.GetStructureByPosition(position);
        character = DataManager.Instance.GetCharacterByPosition(position);
        techStatus = DataManager.Instance.currentPlayer.tech;
        //stars = (int)DataManager.Instance.currentPlayer.stars;
        //techStatus = new[] {2, 2, 2, 2,1,1,1,1,0,0,0};
        Debug.Log("start skilltree:"+structure.id+" value:"+structure.value+" remain:"+structure.remainingRound);
        SkillLevels = new int[11];
        SkillCaps = new[] {1, 1, 1, 1, 1, 1,1,1,1,1,1};
        SkillNames = new[] {"Life", "Horse", "Fish", "Sword", "Elephant", "Fox","Beer","Potion","Arrow","Shield","Cannon"};
        SkillRounds = new[] {0, 2, 2, 2, 3, 3,3,3,3,3,5};
        SkillStars = new[] {0, 4, 4, 4, 10, 10,10,10,10,10,20};
        
        SkillDescriptions = new[]
        {
            "",
            "Riding a Horse",
            "Getting a sword",
            "Getting Fish",
            "Riding an Elephant",
            "Riding a Fox",
            "Getting a Chili",
            "Getting a Pill",
            "Getting an Arrow",
            "Getting a Cannon"
        };
        foreach (var skill in skillHolder.GetComponentsInChildren<Skill>()) skillList.Add(skill);
        foreach (var connector in ConnectorHolder.GetComponentsInChildren<RectTransform>())
            ConnectorList.Add(connector.gameObject);
        skillList[0].ConnectedSkills = new[] {1, 2, 3};
        skillList[1].ConnectedSkills = new[] {4, 5};
        skillList[2].ConnectedSkills = new[] {6, 7};
        skillList[3].ConnectedSkills = new[] {8, 9};
        skillList[8].ConnectedSkills = new[] {10};
        
        for (var i = 0; i < skillList.Count; i++) skillList[i].id = i; 
        UpdateSkillUI();
    }

    public void UpdateSkillUI()
    {
        
        starText.text = stars.ToString();
        techStatus = DataManager.Instance.currentPlayer.tech;
        Debug.Log("------------techstatus-------------"+techStatus);
        for(int i=0;i<techStatus.Length;i++) Debug.Log("i="+i+" number = "+techStatus[i]);
        foreach (var skill in skillList) skill.updateUI();
    }
}
