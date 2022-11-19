using UnityEngine;

namespace Camp
{
    public class CampCharacterHolder : MonoBehaviour
    {
        private int currentx, currenty;
        private Model.Character _character;
        public bool hasCharacter;
        public GameObject character, empty;
        public bool isTraining;
        public int endTurns;
        public int updateType, updateValue;

        // Start is called before the first frame update
        void Start()
        {
        
        }
        public void Inform(int x, int y)
        {
            currentx = x;
            currenty = y;
            UpdateInfo();
        }

        void GetInfo()
        {
            // TODO: get character based on x & y;
            // get end turn, get update type and update value
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnEnable()
        {
            UpdateInfo();
        }

        public void UpdateInfo()
        {
            // TODO: Check whether there is a character
            GetInfo();
            if (hasCharacter)
            {
                //character.GetComponent<CampCharacter>().Inform(_character.characterClass, _character.name, isTraining, endTurns);
                character.GetComponent<CampCharacter>().Inform(Model.CharacterClass.SCHOLAR, "Ruthor",
                    isTraining, endTurns, updateType, updateValue);
                character.SetActive(true);
                empty.SetActive(false);
            }
            else
            {
                character.SetActive(false);
                empty.SetActive(true);
            }
        }
    }
}
