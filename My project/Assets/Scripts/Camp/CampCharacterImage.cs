using UnityEngine;
using UnityEngine.EventSystems;

namespace Camp
{
    public class CampCharacterImage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public GameObject characterPanel;
        public bool active;
        private Structure _structure;
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void Inform(Structure structure)
        {
            _structure = structure;
        }
    

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (active)
            {
                characterPanel.GetComponent<CampCharacterPanel>().SetInfo(
                        _structure.remainingRound > 0);
                characterPanel.SetActive(true);
            }
        }
        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            characterPanel.SetActive(false);
        }
    }
}
