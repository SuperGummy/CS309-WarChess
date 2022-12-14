using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Options
{
    public class OptionsManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OpenPreferenceShop()
        {
            SkinShopManager.calledFromGame = true;
            SceneManager.LoadSceneAsync("Preference Shop", LoadSceneMode.Additive);
        }
    }
}
