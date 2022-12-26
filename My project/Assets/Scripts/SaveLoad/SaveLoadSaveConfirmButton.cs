using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaveLoad
{
    public class SaveLoadSaveConfirmButton : MonoBehaviour
    {
        public static int slotId;

        public static string path;
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
            DataManager.Instance.RemoveArchive(path);
            SaveLoadManager.Instance.SaveSlot(slotId);
        }
    }
}
