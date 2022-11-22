using UnityEngine;
using UnityEngine.SceneManagement;

namespace Options
{
    public class OptionsQuitGameButton : MonoBehaviour
    {
        [SerializeField] private GameObject quitGameInfoBox;
        [SerializeField] private GameObject optionsButtonLayout;
        
        public void ShowQuitGameInfoBox()
        {
            quitGameInfoBox.SetActive(true);
            optionsButtonLayout.SetActive(false);
        }
    }
}