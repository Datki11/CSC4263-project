using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class EnemyWorld : MonoBehaviour
{
    public List<GameObject> encounters;
    // Start is called before the first frame update
    private GameObject playerWorld;
    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "Player World") {
            playerWorld = GameObject.FindGameObjectWithTag("Player World");
            DontDestroyOnLoad(playerWorld);
            SceneManager.sceneLoaded += SetupBattle;
            SceneManager.LoadScene("Battle");
        }
    }
    
    void SetupBattle(Scene scene, LoadSceneMode mode) {

        //Randomly selects an encounter from the encounters list
        int index = Mathf.RoundToInt(Random.Range(0, encounters.Count - 1));
        Instantiate(encounters[index]);
        GameObject.FindGameObjectWithTag("PlayerUnit").GetComponent<Player>().TransferValues(playerWorld.GetComponent<Player>());
        Destroy(playerWorld);
        SceneManager.sceneLoaded -= SetupBattle;
    }
}
