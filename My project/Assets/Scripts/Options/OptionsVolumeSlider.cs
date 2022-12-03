using Audio;
using UnityEngine;

namespace Options
{
    public class OptionsVolumeSlider : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetFloat(float volume)
        {
            volume = (75 + volume) / 80;
            Debug.Log("Current volume: " + volume);
            AudioManager.Instance.ChangeVolume(volume);
        }
    }
}