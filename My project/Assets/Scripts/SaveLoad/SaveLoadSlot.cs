using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
            if (isFull)
            {
                string fileName = dataPath.Substring(SaveLoadManager.Instance.pathPrefix.Length + 1, 
                    dataPath.Length - SaveLoadManager.Instance.pathPrefix.Length - 1);
                Debug.Log(fileName.Split('=')[0] + " --- " + fileName.Split('=')[1]);
                string dateTimeParse = fileName.Split('=')[1];
                dateTimeParse = dateTimeParse.Replace('_', '/');
                dateTimeParse = dateTimeParse.Replace('^', ' ');
                dateTimeParse = dateTimeParse.Replace('-', ':');
                dateTime = DateTime.Parse(dateTimeParse);
                StreamReader sr = new StreamReader(dataPath);
                string jsonString = await sr.ReadToEndAsync();
                sr.Close();
                Sl archive = JsonUtility.FromJson<Sl>(jsonString);
                int characterNumBlue = 0;
                int structureNumBlue = 0;
                int characterNumRed = 0;
                int structureNumRed = 0;
                playerBlue = archive.player1;
                playerRed = archive.player2;
                Debug.Log("ids: " + playerBlue.id + " " + playerRed.id);
                foreach (var t in archive.characterPlayer) {
                    if(t == playerBlue.id) characterNumBlue += 1;
                    else if(t == playerRed.id) characterNumRed += 1;
                    Debug.Log("t = " + t);
                }
                foreach (var t in archive.structurePlayer)
                    if(t == playerBlue.id) structureNumBlue += 1;
                    else if(t == playerRed.id) structureNumRed += 1;
                progressRecordTitle.text = "Game progress saved at: " + dateTime;
                playerProp[0].GetComponent<SaveLoadPlayerProp>().UpdateInfo(playerBlue, characterNumBlue, structureNumBlue);
                playerProp[1].GetComponent<SaveLoadPlayerProp>().UpdateInfo(playerRed, characterNumRed, structureNumRed);
                emptyPanel.SetActive(false);
                propPanel.SetActive(true);
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
                    SaveLoadSaveConfirmButton.path = dataPath;
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
