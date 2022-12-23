using Unity.VisualScripting;
using UnityEngine;

namespace Camp
{
    public class CampCharacterHolder : MonoBehaviour
    {
        public GameObject character, empty;
        private Vector3Int _position;
        [SerializeField] private Character characterInfo;
        [SerializeField] public Structure structureInfo;
        [SerializeField] private CampCharacterImage characterImage;
      

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void UpdateInfo(Vector3Int position)
        {
            characterInfo = DataManager.Instance.GetCharacterByPosition(position);
            if (!(characterInfo is null))
            {
                structureInfo = DataManager.Instance.GetStructureByPosition(position);
                character.GetComponent<CampCharacter>().Inform(characterInfo, structureInfo);
                characterImage.Inform(structureInfo);
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
