using System.Collections;
using System.Collections.Generic;
using SaveLoad;
using UnityEngine;

namespace Options
{
    public class OptionsSaveGameButton : MonoBehaviour
    {
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
            SceneController.LoadSaveLoad(SaveOrLoad.SAVE);
            //GameManager.Instance.SaveArchive();
        }
    }
}