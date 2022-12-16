using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EarnedStarAnimation : MonoBehaviour
{
    [SerializeField] private float speed = 0;

    [SerializeField] private GameObject icon;
    [SerializeField] private GameObject plus;

    [SerializeField] private float lifeTime;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeOut(icon));
        StartCoroutine(FadeOut(plus));
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
    
    public IEnumerator FadeOut(GameObject sprite)
    {
        float startAlpha = sprite.GetComponent<SpriteRenderer>().color.a;
        float rate = 1.0f / lifeTime;
        float progress = 0.0f;
        while (progress < 1.0f)
        {
            Color tmp = sprite.GetComponent<SpriteRenderer>().color;
            tmp.a = Mathf.Lerp(startAlpha, 0, progress);
            sprite.GetComponent<SpriteRenderer>().color = tmp;
            progress += rate * Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
