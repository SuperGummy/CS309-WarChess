using UnityEngine;

namespace Camp
{
    public class CampCharacterPanel : MonoBehaviour
    {
        public GameObject trainingPanel;
        public GameObject nonTrainingPanel;
    
        private bool _isTraining;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void SetInfo(bool isTraining)
        {
            this._isTraining = isTraining;
        }

        void OnEnable()
        {
            // TODO: Find Icon according to type & current player & set Image
            if (_isTraining)
            {
                trainingPanel.SetActive(true);
                nonTrainingPanel.SetActive(false);
            }
            else
            {
                trainingPanel.SetActive(false);
                nonTrainingPanel.SetActive(true);
            }
        }
    }
}
