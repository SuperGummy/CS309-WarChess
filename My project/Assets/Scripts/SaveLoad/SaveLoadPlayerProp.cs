using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SaveLoad
{
    public class SaveLoadPlayerProp : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI characterInfoText,
            moneyInfoText,
            peaceInfoText,
            prosperityInfoText,
            occupationInfoText;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void UpdateInfo(Player player, int characterNum, int structureNum)
        {
            characterInfoText.text = characterNum.ToString();
            moneyInfoText.text = player.stars.ToString();
            peaceInfoText.text = player.peaceDegree.ToString();
            prosperityInfoText.text = player.prosperityDegree.ToString();
            occupationInfoText.text = structureNum.ToString();
        }
    }
}
