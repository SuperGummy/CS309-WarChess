using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaveLoad
{
    public class SaveLoadCloseParentPanelButton : MonoBehaviour
    {
        public GameObject saveLoadManager;

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
            saveLoadManager.GetComponent<SaveLoadManager>().EnableBackGround();
        }
    }
}