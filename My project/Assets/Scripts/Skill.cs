using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
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
        Color c = skilltree.SkillLevels[id] >= skilltree.SkillCaps[id] ? new Color(0.5119972f,0.7679467f,0.9622642f)
            : skilltree.stars > skilltree.SkillStars[id] ? new Color(0.61227f,0.9245283f,0.3698113f) : new Color(0.6674795f,0.7264151f,0.7061736f);
        if (skilltree.SkillLevels[id]>=skilltree.SkillCaps[id])
        {
            titleText.color=Color.blue;
            
            descriptionText.text = $"";
        }
        else
        {
            titleText.color = Color.green;
            descriptionText.text = $"{skilltree.SkillStars[id]} S / {skilltree.SkillRounds[id]} R";
        }

        GetComponent<Image>().color = new Color(1,1,1,0.7f);
        circle.color = c;
        foreach (var connectedSkill in ConnectedSkills)
        {
            skilltree.skillList[connectedSkill].gameObject.SetActive(skilltree.SkillLevels[id]>0);
            skilltree.ConnectorList[connectedSkill].SetActive(skilltree.SkillLevels[id] > 0);
        }

    }

    public void Buy()
    {
        if (skilltree.stars < skilltree.SkillStars[id] || skilltree.SkillLevels[id] >= skilltree.SkillCaps[id]) return;
        skilltree.stars -= skilltree.SkillStars[id];
        skilltree.SkillLevels[id]++;
        skilltree.UpdateSkillUI();
    }
}
