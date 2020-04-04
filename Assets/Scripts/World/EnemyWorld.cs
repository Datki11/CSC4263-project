using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class EnemyWorld : MonoBehaviour
{
    public List<GameObject> encounters;
    public float initialZ;
    public Vector2 startPos;
    // Start is called before the first frame update
    private GameObject playerWorld;
    private GameObject world;
    private SpriteRenderer render;

    public virtual void Awake() {
        initialZ = transform.position.z;
        startPos = new Vector2(transform.position.x, transform.position.y);
        render = GetComponent<SpriteRenderer>();
    }
    private bool hasCollided = false;
    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "Player World" && !hasCollided) {
            hasCollided = true;
            
            this.gameObject.tag = "dead";
            playerWorld = GameObject.FindGameObjectWithTag("Player World");
            world = GameObject.FindGameObjectWithTag("World");

            DontDestroyOnLoad(playerWorld);
            DontDestroyOnLoad(world);
            Instantiate(playerWorld.GetComponent<PlayerWorldController>().worldData);
            SceneManager.sceneLoaded += SetupBattle;
            SceneManager.LoadScene("Battle");
            
        }
    }
    
    public virtual void SetupBattle(Scene scene, LoadSceneMode mode) {
        //Randomly selects an encounter from the encounters list
        
        BattleManager.Instance.world = world;
        GameObject.FindGameObjectWithTag("PlayerUnit").GetComponent<Player>().TransferValues(playerWorld.GetComponent<Player>());
        world.SetActive(false);
        int index = Mathf.RoundToInt(Random.Range(0, encounters.Count));
        Instantiate(encounters[index]);
        Destroy(gameObject);
        RemoveLoadingEvent();
    }
    void RemoveLoadingEvent() {
        SceneManager.sceneLoaded -= SetupBattle;
    }

    public void Update() {
        transform.position = new Vector3(transform.position.x, transform.position.y, initialZ + (render.bounds.size.y / 2 + transform.position.y) / 1000 );
    }
}
