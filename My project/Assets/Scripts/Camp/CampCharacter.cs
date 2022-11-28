using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Camp
{
    public class CampCharacter : MonoBehaviour
    {
        public GameObject trainingPanel, characterPanel;
        public TextMeshProUGUI characterName;
        public GameObject characterImage, characterOpButton;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        public void Inform(Character characterInfo, Structure structureInfo)
        {
            characterName.text = characterInfo.name;
            characterImage.GetComponent<Image>().sprite =
                RenderManager.Instance.GetCharacterImage(
                    characterInfo.characterClass,
                    DataManager.Instance.CheckCharacterSide(characterInfo) == -1 ? "blue" : "red"
                );
            
            bool isTraining = structureInfo.remainingRound > 0;
            characterPanel.GetComponent<CampCharacterPanel>().SetInfo(isTraining);
            trainingPanel.GetComponent<CampTrainingPanel>().SetInfo(structureInfo);
            characterOpButton.GetComponent<CampCharacterOpButton>().Inform(isTraining);
        }
    }
}