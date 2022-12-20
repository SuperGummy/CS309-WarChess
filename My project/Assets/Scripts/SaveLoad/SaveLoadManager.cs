using System;
using System.Collections;
using System.Collections.Generic;
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
        public static SaveOrLoad saveLoadRender;
        public SaveLoadSlot[] saveLoadSlots;
        public GameObject saveLoadSlotHolder;
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private List<Button> buttonList;
        public GameObject coverWarningPanel;

        // Start is called before the first frame update
        void Awake()
        {
            Transform holder = saveLoadSlotHolder.transform;
            for (int i = 0; i < saveLoadSlots.Length; i++)
            {
                GameObject saveLoadSlot = holder.GetChild(i).gameObject;
                saveLoadSlots[i] = saveLoadSlot.GetComponent<SaveLoadSlot>();
                buttonList.Add(saveLoadSlot.GetComponent<Button>());
                saveLoadSlots[i].coverWarningPanel = coverWarningPanel;
            }
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
    }
}