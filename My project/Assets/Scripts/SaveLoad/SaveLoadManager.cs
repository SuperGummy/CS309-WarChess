using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SaveLoad
{
    public enum SaveOrLoad{
        SAVE, LOAD
    }

    public class SaveLoadManager : MonoBehaviour
    {
        public static SaveLoadManager Instance;
        public static SaveOrLoad saveLoadRender;
        public SaveLoadSlot[] saveLoadSlots;
        public GameObject saveLoadSlotHolder;
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private List<Button> buttonList;
        public GameObject coverWarningPanel;
        public string pathPrefix;

        // Start is called before the first frame update
        void Awake()
        {
            Instance = this; 
            Transform holder = saveLoadSlotHolder.transform;
            for (int i = 0; i < saveLoadSlots.Length; i++)
            {
                GameObject saveLoadSlot = holder.GetChild(i).gameObject;
                saveLoadSlots[i] = saveLoadSlot.GetComponent<SaveLoadSlot>();
                buttonList.Add(saveLoadSlot.GetComponent<Button>());
                saveLoadSlots[i].coverWarningPanel = coverWarningPanel;
            }

            pathPrefix = Application.persistentDataPath +
                         "/" + PlayerPrefs.GetString("username").Replace('\\', '-');
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void OnEnable()
        {
            title.text = saveLoadRender == SaveOrLoad.SAVE ? "SAVE" : "LOAD";
            for (int i = 0; i < saveLoadSlots.Length; i++)
            {
                saveLoadSlots[i].saveLoadRender = saveLoadRender;
                saveLoadSlots[i].slotId = i;
                saveLoadSlots[i].isFull = false;
            }
            bool exists = Directory.Exists(pathPrefix);
            if(!exists)
                Directory.CreateDirectory(pathPrefix);
            string[] fileEntries = Directory.GetFiles(pathPrefix);
            foreach (string fileNameRaw in fileEntries)
            {
                string fileName = fileNameRaw.Substring(pathPrefix.Length + 1, fileNameRaw.Length - pathPrefix.Length - 1);
                Debug.Log(fileName.Split('=')[0] + " --- " + fileName.Split('=')[1]);
                int slotId = Int32.Parse(fileName.Split('=')[0]);
                saveLoadSlots[slotId].isFull = true;
                saveLoadSlots[slotId].dataPath = fileNameRaw;
            }
            for (int i = 0; i < saveLoadSlots.Length; i++)
            {
                saveLoadSlots[i].UpdateInfo();
            }
        }

        public void EnableBackGround()
        {
            for (int i = 0; i < buttonList.Count; i++)
            {
                buttonList[i].interactable = true;
            }
        }

        public void DisableBackGround()
        {
            for (int i = 0; i < buttonList.Count; i++)
            {
                buttonList[i].interactable = false;
            }
        }

        public void SaveSlot(int slotId)
        {
            string dateTime = DateTime.Now.ToString();
            dateTime = dateTime.Replace('/', '_');
            dateTime = dateTime.Replace(' ', '^');
            dateTime = dateTime.Replace(':', '-');
            string filename = pathPrefix + "/" + slotId + "=" + dateTime;
            Debug.Log("saved to : " + filename);
            GameManager.Instance.SaveArchive(filename);
            saveLoadSlots[slotId].isFull = true;
            saveLoadSlots[slotId].dataPath = filename;
            saveLoadSlots[slotId].UpdateInfo();
        }

        public void LoadSlot(int slotId)
        {
            StartAfterLoginManager.Instance.LoadArchive(saveLoadSlots[slotId].dataPath);
        }
    }
}