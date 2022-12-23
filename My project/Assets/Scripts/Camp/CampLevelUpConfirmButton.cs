using UnityEngine;

namespace Camp
{
    public class CampLevelUpConfirmButton : MonoBehaviour
    {
        public GameObject CloseButton;
        [SerializeField] private CampManager campManager;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void OnClick() 
        {
            GameManager.Instance.UpgradeStructure(0);
            CloseButton.GetComponent<CampCloseParentPanelButton>().OnClick();
            campManager.UpdateInfo();
        }
    }
}
