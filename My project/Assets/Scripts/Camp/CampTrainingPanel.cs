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

        public void SetInfo(int endTurns, int updateType, int updateValue)
        {
            this._endTurns = endTurns;
            this._updateType = updateType;
            this._updateValue = updateValue;
        }

        public void OnEnable()
        {
            endTurnText.text = "Rounds to go: " + _endTurns;
            updateText.text = "+" + _updateValue;
            // TODO: update icon according to update type;
        }
    }
}
