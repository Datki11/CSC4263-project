using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Chest : MonoBehaviour
{
    public GameObject openChest;
    public GameObject chestItemText;
    public Item item;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Open() {
        Instantiate(chestItemText, transform.position, Quaternion.identity).GetComponent<ChestItemText>().SetText(item.Name);
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
