using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInfoBar : MonoBehaviour
{
    public TextMeshProUGUI team;
    public TextMeshProUGUI star;
    public TextMeshProUGUI characters;
    public TextMeshProUGUI structures;
    public TextMeshProUGUI prosperity;
    public TextMeshProUGUI peace;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void RenderData()
    {
        var player = DataManager.Instance.currentPlayer;
        team.text = player.id == DataManager.Instance.player1.id ? "Blue Team" : "Red Team";
        star.text = player.stars.ToString();
        var character = 0;
        var structure = 0;
        for (int i = 0; i < DataManager.MapSize; i++)
        {
            for (int j = 0; j < DataManager.MapSize; j++)
            {
                var pos = new Vector3Int(i, j);
                if (DataManager.Instance.GetCharacterByPosition(pos)?.player.id == player.id)
                {
                    character += 1;
                }

                if (DataManager.Instance.GetStructureByPosition(pos)?.player.id == player.id)
                {
                    structure += 1;
                }
            }
        }

        characters.text = character.ToString();
        structures.text = structure.ToString();
        prosperity.text = player.prosperityDegree.ToString();
        peace.text = player.peaceDegree.ToString();
    }
}