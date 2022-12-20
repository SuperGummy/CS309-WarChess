using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaveLoad
{
    public class SaveLoadBackButton : MonoBehaviour
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
            // TODO: Call gameManager to close scene
            SceneController.UnLoadSaveLoad();
        }
    }
}
