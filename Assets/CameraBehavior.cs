using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{

    private bool active = false;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private int row = 0;
    private int column = 0;

    // Update is called once per frame
    void LateUpdate()
    {
        if (active) {
            Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.y);
            if (playerPos.x > 0) {
                column = (int) (player.transform.position.x + 28) / 42;
            }
            else
                column = 0;

            row = (int) (player.transform.position.y - 9) / 24;
            transform.position = new Vector3(-7 + column * 42, -3 + row * 24, -20);
        }
        
    }

    public void SetActive() {
        active = true;
        player = GameObject.FindGameObjectWithTag("Player World");
    }
}
