using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;
using UnityEngine.UI;

public class TestButton : MonoBehaviour
{
    [SerializeField] private GameObject _characterObject;
    private CharacterObject characterObject;

    [SerializeField] private float speed = 0.4f;

    [SerializeField] private List<Vector3Int> destinations;
    [SerializeField] private GridController gridController;
    [SerializeField] private Vector3Int position;
    [SerializeField] private Image image;
    [SerializeField] public RuntimeAnimatorController controller;

    public int forTest;
    // Start is called before the first frame update
    void Start()
    {
        characterObject = _characterObject.GetComponent<CharacterObject>();
        RuntimeAnimatorController controller2 = RenderManager.Instance.GetCharacterController(CharacterClass.SCHOLAR, "blue");
        //controller = controller2;
        Debug.Log("Where? " + controller);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        if (forTest == 0)
        {
            gridController.SetAttackableHighlight(destinations, true);
        }
        else if (forTest == 1)
        {
            gridController.SetAttackableHighlight(destinations, false);
        }
        else if (forTest == 2)
        {
            gridController.SetMovableHighlight(destinations, true);
        }
        else if (forTest == 3)
        {
            gridController.SetMovableHighlight(destinations, false);
        }
    }
}
