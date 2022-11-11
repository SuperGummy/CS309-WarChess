using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSetManager : MonoBehaviour
{
    public GameObject[] itemSets;

    public GameObject ItemDatabase;

    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        AddItem("Bow", 0);
        AddItem("Bow", 0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddItem(string name, int index, int cnt)
    {
        GameObject selectedItem = null;
        foreach (Transform item in ItemDatabase.transform)
        {
            if (item.gameObject.name == name)
            {
                selectedItem = item.gameObject;
                break;
            }
        }
        if (selectedItem is null)
            return;
        itemSets[index].GetComponent<ItemSet>().AddItem(selectedItem, cnt);
    }

    public void AddItem(string name, int index)
    {
        AddItem(name, index, 1);
    }
}
