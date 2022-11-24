using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestButton : MonoBehaviour
{
    [SerializeField] private GameObject _characterObject;
    private CharacterObject characterObject;

    [SerializeField] private float speed = 0.4f;

    [SerializeField] private List<Vector3Int> destinations;
    // Start is called before the first frame update
    void Start()
    {
        characterObject = _characterObject.GetComponent<CharacterObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        Debug.Log("My position: " + _characterObject.transform.position);
        characterObject.SetDestinations(destinations);
        characterObject.SetSpeed(speed);
    }
}
