using TMPro;
using UnityEngine;

namespace Camp
{
    public class CampCharacterOpButton : MonoBehaviour
    {
        private bool _isTraining;
        public GameObject stopTrainingPanel, startTrainingPanel;
        public TextMeshProUGUI buttonText;

        public GameObject CampManager;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void Inform(bool isTraining)
        {
            _isTraining = isTraining;
            if (_isTraining)
            {
                buttonText.text = "Stop Training";
            }
            else
            {
                buttonText.text = "Start Training";
            }
        }

        public void OnClick()
        {
            if (_isTraining)
            {
                stopTrainingPanel.SetActive(true);
                
            }
            else
            {
                startTrainingPanel.SetActive(true);
            }
            CampManager.GetComponent<CampManager>().disableBackGround();
        }
    }
}
