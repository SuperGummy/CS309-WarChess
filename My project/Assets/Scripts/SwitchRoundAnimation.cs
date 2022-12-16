using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SwitchRoundAnimation : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    [SerializeField] private float lifeTime;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeOut());
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetText(string side)
    {
        text.text = side + "\nteam!"; 
        if (side.Equals("red"))
        {
            text.color = new Color(1, 0.43f, 0.43f, text.color.a);
        }
        else
        {
            text.color = new Color(0.43f, 0.84f, 0.95f, text.color.a);
        }
    }
    
    public IEnumerator FadeOut()
    {
        float rate = 1.0f / lifeTime;
        float progress = 0.0f;
        while (progress < 1.0f)
        {
            progress += rate * Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
