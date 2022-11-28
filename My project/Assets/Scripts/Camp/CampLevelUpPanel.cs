using TMPro;
using UnityEngine;

namespace Camp
{
    public class CampLevelUpPanel : MonoBehaviour
    {
        public TextMeshProUGUI healthIncreaseCurrentLevelText, strengthIncreaseCurrentLevelText;
        public TextMeshProUGUI healthIncreaseNextLevelText, strengthIncreaseNextLevelText;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void SetInfo(int healthIncrease, int strengthIncrease)
        {
            healthIncreaseCurrentLevelText.text = "+" + healthIncrease;
            strengthIncreaseCurrentLevelText.text = "+" + strengthIncrease;
            healthIncreaseNextLevelText.text = "+" + (healthIncrease + 2);
            strengthIncreaseNextLevelText.text = "+" + (strengthIncrease + 2);
        }
    }
}
