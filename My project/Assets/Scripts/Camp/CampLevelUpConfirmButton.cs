using System.Threading.Tasks;
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

        public async void OnClick()
        {
            await GameManager.Instance.UpgradeStructure(0);
            CloseButton.GetComponent<CampCloseParentPanelButton>().OnClick();
            Debug.Log("Go update info after camp level up!");
            campManager.UpdateInfo();
        }
    }
}
