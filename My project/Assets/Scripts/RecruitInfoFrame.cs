using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecruitInfoFrame: MonoBehaviour
{
    public TextMeshProUGUI naming;
    public TextMeshProUGUI life;
    public TextMeshProUGUI strength;
    public TextMeshProUGUI defend;

    public void RenderText(String n, String l, String s, String d)
    {
        naming.text = n;
        life.text = l;
        strength.text = s;
        defend.text = d;
    }
}