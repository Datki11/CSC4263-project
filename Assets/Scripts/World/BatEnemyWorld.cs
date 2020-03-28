using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BatEnemyWorld : EnemyWorld
{
    Rigidbody2D rb;
    
    //How far the enemy is allowed to move
    private float territoryRange = 12;
    private bool chasingPlayer;
    private float chaseSpeed = 7;
    private float idleSpeed = 3;
    private bool isIdlePointSet = false;
    private Vector2 idlePoint;
    private Vector2 direction;
    private GameObject player;
    public override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player World");
        
    }

    public void Start() {
        Invoke("SetNewIdlePoint", Random.Range(0.2f,0.5f));
    }

    // Update is called once per frame
    void Update()
    {
        if (!chasingPlayer) {
            if (isIdlePointSet) {
                Vector2 oldDirection = direction;
                MoveTowardsPoint(idlePoint, idleSpeed);
                if ( oldDirection != direction) {
                    rb.velocity = Vector2.zero;
                    isIdlePointSet = false;
                    Invoke("SetNewIdlePoint",Random.Range(0.5f,2f));
                }
            }

            Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.y);
            if ( (playerPos - new Vector2(transform.position.x, transform.position.y)).magnitude < 8) {
                if (IsPlayerInTerritory()) {
                    chasingPlayer = true;
                    isIdlePointSet = false;
                }
            }
        }
        else {
            Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.y);
            MoveTowardsPoint(playerPos, chaseSpeed);
            if ( (new Vector2(transform.position.x, transform.position.y) - playerPos).magnitude > 12 || !IsPlayerInTerritory()) {
                rb.velocity = Vector2.zero;
                chasingPlayer = false;
                Invoke("SetNewIdlePoint", Random.Range(1f,1.5f));
            }
        }
    }

    private bool IsPlayerInTerritory() {
        Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.y);
        return playerPos.x > startPos.x - territoryRange / 2 && playerPos.x < startPos.x + territoryRange / 2 && playerPos.y < startPos.y + territoryRange / 2 && playerPos.y > startPos.y - territoryRange / 2;
    }

    private void SetNewIdlePoint() {
        isIdlePointSet = true;
        int random = Mathf.RoundToInt(Random.Range(0,2));
        if (random == 0) {
            idlePoint = new Vector2(transform.position.x, Random.Range(startPos.y - territoryRange / 2, startPos.y + territoryRange / 2));
        }
        else {
            idlePoint = new Vector2(Random.Range(startPos.x - territoryRange / 2, startPos.x + territoryRange / 2), transform.position.y);
        }
        direction = (idlePoint - new Vector2(transform.position.x, transform.position.y)).normalized;

        //Makes sure the new idle point isn't too close to the enemy
        if ( (idlePoint - new Vector2(transform.position.x, transform.position.y)).magnitude < 2)
            SetNewIdlePoint();

    }

    private void MoveTowardsPoint(Vector2 point, float speed) {
        direction = (point - new Vector2(transform.position.x, transform.position.y)).normalized;
        rb.velocity = Vector2.ClampMagnitude(direction * speed, speed);
    }

    
}
