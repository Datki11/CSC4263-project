using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalControls : MonoBehaviour
{

    private GameObject playerWorld;
    private GameObject world;
     private static GlobalControls _instance;
    public static GlobalControls Instance { get { return _instance; } }
    // Start is called before the first frame update
    void Awake()
    {
        //Singleton
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (GameObject.FindGameObjectWithTag("Player World") != null) {

            playerWorld = GameObject.FindGameObjectWithTag("Player World");
            world = GameObject.FindGameObjectWithTag("World");

            DontDestroyOnLoad(world);
            Instantiate(playerWorld.GetComponent<PlayerWorldController>().worldData);
            SceneManager.sceneLoaded += SetupMenu;
            SceneManager.LoadScene("Pause Menu");
            }
        }
    }

    public void OpenShopMenu() {
         playerWorld = GameObject.FindGameObjectWithTag("Player World");
            world = GameObject.FindGameObjectWithTag("World");

            DontDestroyOnLoad(world);
            Instantiate(playerWorld.GetComponent<PlayerWorldController>().worldData);
            SceneManager.sceneLoaded += SetupShopMenu;
            SceneManager.LoadScene("Shop");
    }

    public virtual void SetupMenu(Scene scene, LoadSceneMode mode) {
        //Randomly selects an encounter from the encounters list
        PauseMenu.Instance.SetWorldObject(world);
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().TransferValues(playerWorld.GetComponent<Player>());
        world.SetActive(false);
        SceneManager.sceneLoaded -= SetupMenu;
    }
    public virtual void SetupShopMenu(Scene scene, LoadSceneMode mode) {
        //Randomly selects an encounter from the encounters list
        ShopMenu.Instance.SetWorldObject(world);
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().TransferValues(playerWorld.GetComponent<Player>());
        world.SetActive(false);
        SceneManager.sceneLoaded -= SetupShopMenu;
    }
}
