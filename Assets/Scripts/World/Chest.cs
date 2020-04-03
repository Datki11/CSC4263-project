using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Chest : MonoBehaviour
{
    public GameObject openChest;
    public GameObject chestItemText;
    public string itemName;
    public Item item;
    // Start is called before the first frame update
    void Start()
    {
        //Lord forgive me
        if (itemName == "Potion")
            item = new Potion();
        else if (itemName == "Firecracker")
            item = new Firecracker();

        //default item
        else
            item = new Potion();
    }

    public void Open() {
        Instantiate(chestItemText, transform.position, Quaternion.identity).GetComponent<ChestItemText>().SetText(itemName);
        TurnToOpenedChest();
        
    }
    public void TurnToOpenedChest() {
        Instantiate(openChest, new Vector3(transform.position.x, transform.position.y, GetComponent<WorldPerspective>().initialZ), Quaternion.identity);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
