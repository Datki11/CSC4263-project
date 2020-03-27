using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerWorldController : MonoBehaviour
{

    private Rigidbody2D rb;
    private float speed = 8f;
    public float initialZ;
    public GameObject worldData;
    private List<GameObject> interactableObjects;
    private SpriteRenderer render;
    // Start is called before the first frame update
    void Awake()
    {
        interactableObjects = new List<GameObject>();
        rb = GetComponent<Rigidbody2D>();
        initialZ = transform.position.z;
        render = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveDir = new Vector2(0,0);
        if (Input.GetKey(KeyCode.LeftArrow))
            moveDir = new Vector2(-1, moveDir.y);
        if (Input.GetKey(KeyCode.RightArrow))
            moveDir = new Vector2(1, moveDir.y);
        if (Input.GetKey(KeyCode.UpArrow))
            moveDir = new Vector2(moveDir.x, 1);
        if (Input.GetKey(KeyCode.DownArrow))
            moveDir = new Vector2(moveDir.x, -1);
        rb.velocity = Vector2.ClampMagnitude(moveDir * speed, speed);

        if (Input.GetKeyDown(KeyCode.Return)) {
            if (interactableObjects.Count > 0) {
                GameObject obj = interactableObjects[0];
                if (obj.tag == "Chest") {
                    interactableObjects.Remove(obj);
                    Item chestItem = obj.GetComponent<Chest>().item;
                    obj.GetComponent<Chest>().Open();
                    
                    GetComponent<Player>().AddItem(chestItem);
                }
            }
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, initialZ + (render.bounds.size.y / 2 + transform.position.y) / 1000 );
    }

    void OnTriggerEnter2D(Collider2D collider) {

        if (collider.gameObject.tag == "Chest") {
            interactableObjects.Add(collider.gameObject);
        }
        
    }

    void OnTriggerExit2D(Collider2D collider) {
        if (interactableObjects.Contains(collider.gameObject)) {
            interactableObjects.Remove(collider.gameObject);
        }
    }
}
