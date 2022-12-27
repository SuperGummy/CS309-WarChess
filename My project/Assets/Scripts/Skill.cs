using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Model;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using static Skilltree;

public class Skill : MonoBehaviour
{
    public int id;
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    public Image skillUI;
    public Image circle;
    public int round;
    public int cost;
    public int[] ConnectedSkills;
    public void updateUI()
    {
        titleText.text = $"{skilltree.SkillNames[id]}";
        /*Color c =  skilltree.SkillLevels[id] >= skilltree.SkillCaps[id] ?(skilltree.rounds>=skilltree.stopRounds[id]?
                new Color(0.5119972f,0.7679467f,0.9622642f): new Color(1.0f,0.0f,0.0f))
            : skilltree.stars > skilltree.SkillStars[id] ? new Color(0.61227f,0.9245283f,0.3698113f) : new Color(0.6674795f,0.7264151f,0.7061736f);*/
        Color c = skilltree.techStatus[id+TOTSKILLS]>0 ? (DataManager.Instance.currentPlayer.stars >= skilltree.SkillStars[id]
                ? new Color(0.61227f, 0.9245283f, 0.3698113f)
                : new Color(0.6674795f, 0.7264151f, 0.7061736f)) :
            skilltree.techStatus[id]==0 ? new Color(1.0f, 0.0f, 0.0f) : new Color(0.5119972f, 0.7679467f, 0.9622642f);
        if (skilltree.structure.remainingRound>0&&skilltree.structure.value == id)
        {
            titleText.color=new Color(1,0.5f,0);
            
            descriptionText.text = $"Lighting..";
        }
        else if (skilltree.techStatus[id+TOTSKILLS]>0&&skilltree.techStatus[id] == 0)
        {
            titleText.color=new Color(1,0,0);
            
            descriptionText.text = $"Other Trees: Lighting..";
        }
        else if (skilltree.techStatus[id+TOTSKILLS]==0)
        {
            titleText.color=Color.blue;
            
            descriptionText.text = $"Done";
        }
        else if(DataManager.Instance.currentPlayer.stars>=skilltree.SkillStars[id])
        {
            titleText.color = Color.green;
            descriptionText.text = $"{skilltree.SkillStars[id]} S / {skilltree.SkillRounds[id]} R";
        }
        else
        {
            titleText.color = Color.gray;
            descriptionText.text = $"{skilltree.SkillStars[id]} S / {skilltree.SkillRounds[id]} R";
        }

        c = titleText.color;
        GetComponent<Image>().color = new Color(1,1,1,0.7f);
        circle.color = c;
        foreach (var connectedSkill in ConnectedSkills)
        {
            skilltree.skillList[connectedSkill].gameObject.SetActive(skilltree.techStatus[id+TOTSKILLS]==0);
            skilltree.ConnectorList[connectedSkill].SetActive(skilltree.techStatus[id+TOTSKILLS]==0);
        }

    }

    public async void Buy()
    {
        if (skilltree.character == null || skilltree.character.characterClass != CharacterClass.SCHOLAR) return;
        if (skilltree.structure.remainingRound > 0)
        {
            Debug.Log("have other tech in process");
            return;
        }

        if (skilltree.stars < skilltree.SkillStars[id] || skilltree.techStatus[id] == 0)
        {
            Debug.Log("Not enough money OR Lighting");
            return;
        }
        //skilltree.stars -= skilltree.SkillStars[id];
        //skilltree.SkillLevels[id]++;
        Debug.Log("buy tech:"+id+" Success!");
        await GameManager.Instance.UpdateTech(id);
        skilltree.UpdateSkillUI();
    }
}
