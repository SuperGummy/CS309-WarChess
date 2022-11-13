using UnityEngine;

namespace Camp
{
    public class CampCloseParentPanelButton : MonoBehaviour
    {
        public GameObject CampManager;
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
            this.transform.parent.gameObject.SetActive(false);
            CampManager.GetComponent<CampManager>().enableBackGround();
        }
    }
}