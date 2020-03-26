using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWorldController : MonoBehaviour
{

    private Rigidbody2D rb;
    private float speed = 8f;
    public float initialZ;
    public GameObject worldData;
    private SpriteRenderer render;
    // Start is called before the first frame update
    void Awake()
    {
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

        transform.position = new Vector3(transform.position.x, transform.position.y, initialZ + (render.bounds.size.y / 2 + transform.position.y) / 1000 );
    }
}
