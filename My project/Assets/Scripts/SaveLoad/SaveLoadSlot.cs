using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SaveLoad
{
    public class SaveLoadSlot : MonoBehaviour
    {
        public bool isFull;
        public bool saveLoadRender;

        private DateTime _dateTime;
        [SerializeField] private Model.Player playerBlue, playerRed;
        [SerializeField] private GameObject[] playerProp;
        [SerializeField] private GameObject propPanel, emptyPanel;
        [SerializeField] private TextMeshProUGUI progressRecordTitle;
        public GameObject coverWarningPanel;

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

        public void UpdateInfo()
        {
            // TODO: GetInfo : get whether it is empty, get players
            _dateTime = DateTime.Now; // TODO: delete this row, only for testing;
            progressRecordTitle.text = "Game progress saved at: " + _dateTime;
            if (isFull)
            {
                emptyPanel.SetActive(false);
                propPanel.SetActive(true);
                playerProp[0].GetComponent<SaveLoadPlayerProp>().UpdateInfo(playerBlue);
                playerProp[1].GetComponent<SaveLoadPlayerProp>().UpdateInfo(playerRed);
            }
            else
            {
                emptyPanel.SetActive(true);
                propPanel.SetActive(false);
            }
        }

        public void OnClick()
        {
            if (saveLoadRender is false)
            {
                if (isFull)
                {
                    // TODO: tell coverWarningPanel which progress is currently been operated on
                    coverWarningPanel.SetActive(true);
                }
                else
                {
                    // TODO: Save on slot;
                }
            }
            else
            {
                if (isFull)
                {
                    // TODO: Load on slot
                }
            }
        }
    }
}
