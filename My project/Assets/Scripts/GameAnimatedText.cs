using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;

public class GameAnimatedText : MonoBehaviour
{
    [SerializeField] private float speed = 0;

    [SerializeField] private TextMeshPro text;

    [SerializeField] private float lifeTime;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeOut());
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed);
    }
    
    public IEnumerator FadeOut()
    {
        float startAlpha = text.color.a;
        float rate = 1.0f / lifeTime;
        float progress = 0.0f;
        while (progress < 1.0f)
        {
            Color tmp = text.color;
            tmp.a = Mathf.Lerp(startAlpha, 0, progress);
            text.color = tmp;
            progress += rate * Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
