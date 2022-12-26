using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

namespace SaveLoad
{
    public class SaveLoadSlot : MonoBehaviour
    {
        public bool isFull;

        public int slotId;
        public SaveOrLoad saveLoadRender;

        public DateTime dateTime;
        [SerializeField] private Player playerBlue, playerRed;
        [SerializeField] private GameObject[] playerProp;
        [SerializeField] private GameObject propPanel, emptyPanel;
        [SerializeField] private TextMeshProUGUI progressRecordTitle;
        public GameObject coverWarningPanel;
        public string dataPath;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void GetInfo()
        {

        }

        public async void UpdateInfo()
        {
            StreamReader sr = new StreamReader(dataPath);
            string jsonString = await sr.ReadToEndAsync();
            sr.Close();
            Sl archive = JsonUtility.FromJson<Sl>(jsonString);
            playerBlue = archive.player1;
            int characterNumBlue = 0;
            int structureNumBlue = 0;
            int characterNumRed = 0;
            int structureNumRed = 0;
            playerRed = archive.player2;
            foreach (var t in archive.characterPlayer)
                switch (t)
                {
                    case -1:
                        characterNumBlue += 1;
                        break;
                    case 1:
                        characterNumRed += 1;
                        break;
                }

            foreach (var t in archive.structurePlayer)
                switch (t)
                {
                    case -1:
                        structureNumBlue += 1;
                        break;
                    case 1:
                        structureNumRed += 1;
                        break;
                }

            progressRecordTitle.text = "Game progress saved at: " + dateTime;
            if (isFull)
            {
                emptyPanel.SetActive(false);
                propPanel.SetActive(true);
                playerProp[0].GetComponent<SaveLoadPlayerProp>().UpdateInfo(playerBlue, characterNumBlue, structureNumBlue);
                playerProp[1].GetComponent<SaveLoadPlayerProp>().UpdateInfo(playerRed, characterNumRed, structureNumRed);
            }
            else
            {
                emptyPanel.SetActive(true);
                propPanel.SetActive(false);
            }
        }

        public void OnClick()
        {
            if (saveLoadRender == SaveOrLoad.SAVE)
            {
                if (isFull)
                {
                    // TODO: tell coverWarningPanel which progress is currently been operated on
                    SaveLoadSaveConfirmButton.slotId = slotId;
                    coverWarningPanel.SetActive(true);
                }
                else
                {
                    SaveLoadManager.Instance.SaveSlot(slotId);
                }
            }
            else
            {
                if (isFull)
                {
                    SaveLoadManager.Instance.LoadSlot(slotId);
                }
            }
        }
    }
}
