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
        private string _path;

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

            _path = Application.persistentDataPath +
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
            }
            string[] fileEntries = Directory.GetFiles(_path);
            foreach (string fileName in fileEntries)
            {
                int slotId = Int32.Parse(fileName.Split('|')[0]);
                saveLoadSlots[slotId].isFull = true;
                saveLoadSlots[slotId].dataPath = fileName;
                saveLoadSlots[slotId].dateTime = DateTime.Parse(fileName.Split('|')[1]);
                saveLoadSlots[slotId].UpdateInfo();
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
            string filename = _path + "/" + slotId + "-" + DateTime.Now;
            GameManager.Instance.SaveArchive(filename);
        }

        public void LoadSlot(int slotId)
        {
            GameManager.Instance.LoadArchive(saveLoadSlots[slotId].dataPath);
        }
    }
}