using UnityEngine;
using UnityEngine.EventSystems;

namespace Camp
{
    public class CampCharacterImage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public GameObject characterPanel;
        public bool active;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (active)
            {
                // TODO: check if there is character
                //characterPanel.GetComponent<CampCharacterPanel>().SetInfo(_character.characterClass, _character.name, isTraining, endTurns);
                characterPanel.SetActive(true);
            }
        }
        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            characterPanel.SetActive(false);
        }
    }
}
