using System;
using TMPro;
using UnityEngine;

namespace Camp
{
    public class CampCharacter : MonoBehaviour
    {
        public GameObject trainingPanel, characterPanel;
        public TextMeshProUGUI characterName;
        public GameObject characterImage, characterOpButton;
    
        public bool isTraining, currentPlayer;
        public int endTurns, updateType, updateValue;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        public void Inform(Model.CharacterClass type, String characterName, bool isTraining, 
            int endTurns, int updateType, int updateValue)
        {
            // TODO: set according to type & player characterImage.GetComponent<Image>().sprite();
            this.characterName.text = characterName;
            this.isTraining = isTraining;
            this.endTurns = endTurns;
            this.updateValue = updateValue;
            this.updateType = updateType;
            characterPanel.GetComponent<CampCharacterPanel>().SetInfo(this.isTraining);
            trainingPanel.GetComponent<CampTrainingPanel>().SetInfo(this.endTurns, this.updateType, this.updateValue);
            characterOpButton.GetComponent<CampCharacterOpButton>().Inform(this.isTraining);
        }

        void OnEnable()
        {
            // TODO: Find Icon according to type & current player & set Image
        }
    }
}