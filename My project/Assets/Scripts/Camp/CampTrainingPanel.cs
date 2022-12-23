using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Camp
{
    public class CampTrainingPanel : MonoBehaviour
    {
        public TextMeshProUGUI endTurnText;
        public TextMeshProUGUI updateText;

        public Image characterIcon;
        private int _endTurns, _updateType, _updateValue;
    
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetInfo(Structure structureInfo)
        {
            _endTurns = structureInfo.remainingRound;
            _updateType = structureInfo.value;
            _updateValue = structureInfo.level * 2;
        }

        public void OnEnable()
        {
            endTurnText.text = "Rounds to go: " + _endTurns;
            updateText.text = "+" + _updateValue;
            switch (_updateType)
            {
                case 0:
                    characterIcon.sprite = RenderManager.Instance.GetStatIcon(StatType.HEALTH);
                    break;
                case 1:
                    characterIcon.sprite = RenderManager.Instance.GetStatIcon(StatType.STRENGTH);
                    break;
            }
        }
    }
}
