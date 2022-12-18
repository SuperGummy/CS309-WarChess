using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProgressbarSlider : MonoBehaviour
{
    private int _progress;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private ProgressRenderer progressRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SetFloat(float progress)
    {
        _progress = (int)progress;
        if (_progress >= 99) _progress = 100;
        progressText.text = ((int) _progress >= 100 ? 100 : _progress) + "/100";
        if (_progress >= 100)
        {
            progressRenderer.UnLoad();
        }
    }
}
