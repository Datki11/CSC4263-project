using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ShopMenu : MonoBehaviour
{
     public GameObject menuText;
    public GameObject menuTextIndicator;
    public GameObject itemMenuIndicator;
    public GameObject defaultMenu;
    public GameObject itemMenu;
    public GameObject itemTexts;
    public GameObject itemTextPrefab;
    public Text itemDescriptionText;
    public GameObject alreadyOwnedObject;
    public Text alreadyOwnedTextValue;
    private Player player;
    private int menuTextPos = 0;
    private int itemMenuTextPos = 0;
    private GameObject world;
    private static ShopMenu _instance;
    private Dictionary<Item, int> itemsInStock;
    public static ShopMenu Instance { get { return _instance; } }
    void Awake() {
        //Singleton
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }

        itemsInStock = new Dictionary<Item, int>();
        itemsInStock.Add(new Potion(), 15);
        itemsInStock.Add(new Firecracker(), 20);
        itemsInStock.Add(new Adrenaline(), 25);
        itemsInStock.Add(new BigPotion(), 35);
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
                    ReloadLevel();
                }
            }
            else if (itemMenu.activeSelf) {
                List<Item> keys = new List<Item>(itemsInStock.Keys); 
                Item item = keys[itemMenuTextPos];
                if (player.Gold >= itemsInStock[item]) {
                    player.Gold -= itemsInStock[item];
                    
                    List<Item> playerItems = new List<Item>(player.Items.Keys);
                    Item playerItem = playerItems.Find(x => x.Name == item.Name);
                    

                    if (playerItem == null) {
                        player.Items.Add(item, 1);
                    }
                    else {
                        player.Items[playerItem] += 1;
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
        GameObject playerObj = ShopMenu.Instance.gameObject;
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
        Destroy(ShopMenu.Instance.gameObject);
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
        if (itemsInStock.Count > 0) {
            foreach (KeyValuePair<Item, int> item in itemsInStock) {
                GameObject textObj = Instantiate(itemTextPrefab, Vector3.zero, Quaternion.identity, itemTexts.transform);
                textObj.GetComponent<RectTransform>().localPosition = Vector3.zero;
                Transform itemNameTextObj = textObj.transform.GetChild(0);
                itemNameTextObj.GetComponent<RectTransform>().localPosition = new Vector3(-60f, 120f - num * 30f, 0f);
                itemNameTextObj.GetComponent<Text>().text = item.Key.Name;
                Transform itemCostObj= textObj.transform.GetChild(1);
                itemCostObj.GetComponent<RectTransform>().localPosition = new Vector3(110f, 120f - num * 30f, 0f);
                itemCostObj.GetChild(1).gameObject.GetComponent<Text>().text = item.Value.ToString();
                num++;


            }
            UpdateItemMenuSelection();
        }

    }

    private void UpdateItemMenuSelection() {
        RectTransform rect = itemMenuIndicator.GetComponent<RectTransform>();
        rect.localPosition = new Vector3(rect.localPosition.x, 120f - itemMenuTextPos * 30, rect.localPosition.z);
        List<Item> keys = new List<Item>(itemsInStock.Keys);
        itemDescriptionText.text = keys[itemMenuTextPos].Description;
        //Checks to see how many of the items the player has
        List<Item> playerItems = new List<Item>(player.Items.Keys);
        Item playerItem = playerItems.Find(x => x.Name == keys[itemMenuTextPos].Name);
        if (playerItem != null) {
            alreadyOwnedTextValue.text = player.Items[playerItem].ToString();
        }
        else {
            alreadyOwnedTextValue.text = "0";
        }

    }
}
