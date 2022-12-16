using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField] private GameObject switchRoundAnimationPrefab;
    [SerializeField] private string sideString;
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject test;
    
    public int forTest;
    // Start is called before the first frame update
    void Start()
    {
        // characterObject = _characterObject.GetComponent<CharacterObject>();
        // RuntimeAnimatorController controller2 = RenderManager.Instance.GetCharacterController(CharacterClass.SCHOLAR, "blue");
        // //controller = controller2;
        // Debug.Log("Where? " + controller);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        UIManager.Instance.ShowRoundChange(-1);
        //gridController.CreateMoneyAnimation(position);
        // gridController.CreateCharacter(position);
        // switch (forTest)
        // {
        //     case 0:
        //         gridController.ShowCharacterDamageText(position, 3);
        //         break;
        //     case 1:
        //         gridController.ShowCharacterAddHealthText(position, 6);
        //         break;
        //     case 2:
        //         gridController.ShowCharacterAddStrengthText(position, 7);
        //         break;
        //     case 3:
        //         gridController.ShowConquerText(position);
        //         break;
        // }
        // CampManager.position = new Vector3Int(0, 0, 0);
        // SceneManager.LoadSceneAsync("Camp", LoadSceneMode.Additive);
    }
}
