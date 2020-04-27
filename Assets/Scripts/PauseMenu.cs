using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject menuText;
    public GameObject menuTextIndicator;
    public GameObject itemMenuIndicator;
    public GameObject defaultMenu;
    public GameObject itemMenu;
    public GameObject statusMenu;
    public GameObject itemTexts;
    public GameObject itemTextPrefab;
    public Text itemDescriptionText;
    public Text healthText;
    public Text levelText;
    public Text expText;
    public Text attackText;
    public Text defenseText;
    public Text speedText;
    private Player player;
    private int menuTextPos = 0;
    private int itemMenuTextPos = 0;
    private GameObject world;
    private static PauseMenu _instance;
    public static PauseMenu Instance { get { return _instance; } }
    void Awake() {
        //Singleton
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow)) {

            if (defaultMenu.activeSelf) {
                menuTextPos++;
                if (menuTextPos >= menuText.transform.childCount)
                    menuTextPos = 0;
                UpdateMenuIndicator();
            }

            else if (itemMenu.activeSelf) {
                itemMenuTextPos++;
                if (itemMenuTextPos >= itemTexts.transform.childCount)
                    itemMenuTextPos = 0;
                UpdateItemMenuSelection();
            }

        }
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            
            if (defaultMenu.activeSelf) {
                menuTextPos--;
                if (menuTextPos < 0)
                    menuTextPos = menuText.transform.childCount - 1;
                UpdateMenuIndicator();
            }

            else if (itemMenu.activeSelf) {
                itemMenuTextPos--;
                if (itemMenuTextPos < 0)
                    itemMenuTextPos = itemTexts.transform.childCount - 1;
                UpdateItemMenuSelection();
            }

        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Z)) {
            if (defaultMenu.activeSelf) {
                //Opens items menu
                if (menuTextPos == 0) {
                    defaultMenu.SetActive(false);
                    itemMenu.SetActive(true);
                    PopulateItemMenu();
                }
                else if (menuTextPos == 1) {
                    defaultMenu.SetActive(false);
                    statusMenu.SetActive(true);
                    SetStatusText();
                }
                else if (menuTextPos == 2) {
                    ReloadLevel();
                }
                else if (menuTextPos == 3) {
                    Application.Quit();
                }
            }
            else if (itemMenu.activeSelf) {
                List<Item> keys = new List<Item>(player.Items.Keys); 
                Item item = keys[itemMenuTextPos];
                if (!item.OnlyInBattle) {
                    item.UseThroughPauseMenu(player);
                    player.Items[item] -= 1;
                    if(player.Items[item] <= 0) {
                        player.Items.Remove(item);
                    }
                    PopulateItemMenu();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.X)) {
            if (itemMenu.activeSelf) {
                itemMenu.SetActive(false);
                defaultMenu.SetActive(true);
            }
            else if (statusMenu.activeSelf) {
                statusMenu.SetActive(false);
                defaultMenu.SetActive(true);
            }
            else if (defaultMenu.activeSelf) {
                ReloadLevel();
            }
        }
    }

    private void UpdateMenuIndicator() {
        RectTransform rect = menuTextIndicator.GetComponent<RectTransform>();
        RectTransform textRect = menuText.transform.GetChild(menuTextPos).GetComponent<RectTransform>();
        rect.localPosition = new Vector3(rect.localPosition.x, textRect.localPosition.y, rect.localPosition.z );
    }

    private void ReloadLevel() {
        GameObject playerObj = PauseMenu.Instance.gameObject;
        DontDestroyOnLoad(playerObj);
        SceneManager.sceneLoaded += SetupWorld;
        SceneManager.LoadScene("Empty Scene");
    }
    void SetupWorld(Scene scene, LoadSceneMode mode) {

        //Transfer player stats to the world scene
        world.SetActive(true);
        GameObject playerUnit = GameObject.FindGameObjectWithTag("Player");
        GameObject.FindGameObjectWithTag("Player World").GetComponent<Player>().TransferValues(playerUnit.GetComponent<Player>());
        GameObject.FindGameObjectWithTag("Player World").GetComponent<Player>().CurrentHealth = playerUnit.GetComponent<Player>().CurrentHealth;
        Destroy(playerUnit);
        Destroy(PauseMenu.Instance.gameObject);
        SceneManager.sceneLoaded -= SetupWorld;
        

    }
    public void SetWorldObject(GameObject obj) {
        world = obj;
    }
    private void PopulateItemMenu() {

        //Destroy all item texts that already exist
        for(int i = 0; i < itemTexts.transform.childCount; i++) {
            Destroy(itemTexts.transform.GetChild(i).gameObject);
        }
        int num = 0;
        if (player.Items.Count > 0) {
            foreach (KeyValuePair<Item, int> item in player.Items) {
                GameObject textObj = Instantiate(itemTextPrefab, Vector3.zero, Quaternion.identity, itemTexts.transform);
                textObj.GetComponent<RectTransform>().localPosition = Vector3.zero;
                Transform itemNameTextObj = textObj.transform.GetChild(0);
                itemNameTextObj.GetComponent<RectTransform>().localPosition = new Vector3(-60f, 120f - num * 30f, 0f);
                itemNameTextObj.GetComponent<Text>().text = item.Key.Name;
                Transform itemQuantityTextObj = textObj.transform.GetChild(1);
                itemQuantityTextObj.GetComponent<RectTransform>().localPosition = new Vector3(40f, 120f - num * 30f, 0f);
                itemQuantityTextObj.GetComponent<Text>().text = item.Value.ToString();
                num++;


            }
            UpdateItemMenuSelection();
        }

    }

    private void UpdateItemMenuSelection() {
        RectTransform rect = itemMenuIndicator.GetComponent<RectTransform>();
        rect.localPosition = new Vector3(rect.localPosition.x, 120f - itemMenuTextPos * 30, rect.localPosition.z);
        List<Item> keys = new List<Item>(player.Items.Keys);
        itemDescriptionText.text = keys[itemMenuTextPos].Description;
    }

    private void SetStatusText() {
        healthText.text = player.CurrentHealth.ToString() + " / " + player.MaxHealth.ToString();
        levelText.text = player.Level.ToString();
        expText.text = player.CurrentExp.ToString() + " / " + player.ExpToLevelUp.ToString();
        attackText.text = player.Attack.ToString();
        defenseText.text = player.Defense.ToString();
        speedText.text = player.Speed.ToString();

    }
}
