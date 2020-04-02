using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public GameObject playerMenu;
    public GameObject playerMenuSelection;
    public GameObject playerAbilityMenu;
    public GameObject playerItemMenu;
    public GameObject abilityText;
    public GameObject abilityMenuSelection;
    public GameObject itemMenuSelection;
    public GameObject unitSelection;
    public GameObject unitSelectionFriendly;
    public GameObject damageText;
    public Text abilityDescriptionText;
    public Text itemDescriptionText;
    public GameObject gameOverScreen;
    private int playerMenuPos = 0;
    private int unitSelectionPos = 0;
    private int unitSelectionFriendlyPos;
    private List<GameObject> characters;
    private List<GameObject> enemies;
    private List<Ability> _abilities;
    private Dictionary<Item, int> _items;
    private int abilityMenuPos = 0;
    private int itemMenuPos = 0;
    private bool usingOffensiveItem = false;
    private List<GameObject> abilityTexts;
    private List<GameObject> itemTexts;

    private static BattleManager _instance;

    public static BattleManager Instance { get { return _instance; } }
    
    private enum Turn {
        Player,
        Enemy
    }
    private Turn turn = Turn.Player;
    private int turnPos = 0;
    void Awake()
    {

        //Singleton
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    void Start() {
        characters = new List<GameObject>(GameObject.FindGameObjectsWithTag("PlayerUnit"));
        enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("EnemyUnit"));
    }

    // Update is called once per frame
    void Update()
    {
        //This code is for the menu navigation

        #region Menu navigation

        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            if (playerMenu.activeSelf) {
                playerMenuPos--;
                if (playerMenuPos < 0)
                    playerMenuPos = 3;
                UpdateMenuSelection();
            }
            else if (unitSelection.activeSelf) {
                UnitSelectionAscend();
            }
        }


        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            if (playerMenu.activeSelf) {
                playerMenuPos++;
                if (playerMenuPos > 3)
                    playerMenuPos = 0;
                UpdateMenuSelection();
            }
            else if (unitSelection.activeSelf) {
                UnitSelectionDescend();
            }   
        }


        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            if (playerAbilityMenu.activeSelf) {
                abilityMenuPos++;
                if (abilityMenuPos > abilityTexts.Count - 1)
                    abilityMenuPos = 0;
                UpdateAbilityMenuSelection();
            }
            else if (unitSelection.activeSelf) {
                UnitSelectionDescend();
            }
            else if (playerItemMenu.activeSelf) {
                itemMenuPos++;
                if (itemMenuPos > itemTexts.Count - 1)
                    itemMenuPos = 0;
                UpdateItemMenuSelection();
            }      
        }


        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            if (playerAbilityMenu.activeSelf) {
                abilityMenuPos--;
                if (abilityMenuPos < 0)
                    abilityMenuPos = abilityTexts.Count - 1;
                UpdateAbilityMenuSelection();
            }
            else if (unitSelection.activeSelf) {
                UnitSelectionAscend();
            }
            else if (playerItemMenu.activeSelf) {
                itemMenuPos--;
                if (itemMenuPos < 0)
                    itemMenuPos = itemTexts.Count - 1;
                UpdateItemMenuSelection();
            }  
        }


        if (Input.GetKeyDown(KeyCode.Return)) {
            if (playerMenu.activeSelf) {
                if (playerMenuPos == 0) {
                    playerMenu.SetActive(false);
                    playerAbilityMenu.SetActive(true);
                    PopulateAbilityMenu(characters[0].GetComponent<Character>().Class.Abilities);
                }
                else if (playerMenuPos == 1) {
                    playerMenu.SetActive(false);
                    playerItemMenu.SetActive(true);
                    PopulateItemMenu(characters[0].GetComponent<Player>().Items);
                }
            }
            else if (playerAbilityMenu.activeSelf) {
                playerAbilityMenu.SetActive(false);
                unitSelection.SetActive(true);
                UpdateUnitSelection();
            }
            else if (unitSelection.activeSelf) {

                //Using an offensive item on an enemy
                if (usingOffensiveItem) {
                    List<Item> keys = new List<Item>(_items.Keys);
                    keys[itemMenuPos].Use(enemies[unitSelectionPos].GetComponent<Unit>());
                    _items[keys[itemMenuPos]] -= 1;
                    if (_items[keys[itemMenuPos]] <= 0) {
                        _items.Remove(keys[itemMenuPos]);
                    }
                    usingOffensiveItem = false;
                    unitSelection.SetActive(false);
                }

                //Using an offensive ability on an enemy
                else {
                    var ability = _abilities[abilityMenuPos];
                    if (ability.Type == TargetType.Single)
                        ability.Action(enemies[unitSelectionPos].GetComponent<Unit>());
                    else if (ability.Type == TargetType.AllEnemy)
                        foreach (GameObject enemy in enemies.ToArray())
                            ability.Action(enemy.GetComponent<Unit>());
                    unitSelection.SetActive(false);
                }

            }
            else if (unitSelectionFriendly.activeSelf) {
                List<Item> keys = new List<Item>(_items.Keys);
                keys[itemMenuPos].Use(characters[0].GetComponent<Unit>());
                _items[keys[itemMenuPos]] -= 1;
                if (_items[keys[itemMenuPos]] <= 0) {
                    _items.Remove(keys[itemMenuPos]);
                }
                unitSelectionFriendly.SetActive(false);
            }
            else if (playerItemMenu.activeSelf) {
                playerItemMenu.SetActive(false);
                List<Item> keys = new List<Item>(_items.Keys);
                if (keys[itemMenuPos].IsFriendly) {
                    unitSelectionFriendly.SetActive(true);
                    UpdateUnitSelectionFriendly();
                }
                else {
                    usingOffensiveItem = true;
                    unitSelection.SetActive(true);
                    UpdateUnitSelection();
                }
            }
        }


        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (playerAbilityMenu.activeSelf) {
                playerAbilityMenu.SetActive(false);
                playerMenu.SetActive(true);
            }
            else if (unitSelection.activeSelf) {
                unitSelection.SetActive(false);
                if (usingOffensiveItem) {
                    usingOffensiveItem = false;
                    playerItemMenu.SetActive(true);
                }
                else {
                    playerAbilityMenu.SetActive(true);
                }
            }
            else if (playerItemMenu.activeSelf) {
                playerItemMenu.SetActive(false);
                playerMenu.SetActive(true);
            }
            else if (unitSelectionFriendly.activeSelf) {
                unitSelectionFriendly.SetActive(false);
                playerItemMenu.SetActive(true);
            }
        }

        #endregion
    }

    void NextTurn() {
        enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("EnemyUnit"));
        unitSelectionPos = 0;

        //If there are no enemies left, the battle is over and will return to the world scene
        if (enemies.Count <= 0) {
            GameObject playerObj = GameObject.FindGameObjectWithTag("PlayerUnit").gameObject;
            DontDestroyOnLoad(playerObj);
            SceneManager.sceneLoaded += SetupWorld;
            SceneManager.LoadScene("Level 1");
        }

        //Currently, this is set up to where the max party size for the player is 1
        else if (turn == Turn.Player) {
            turn = Turn.Enemy;
            turnPos = 0;
            DoEnemyTurn();
        }
        else {
            turnPos++;
            if (turnPos > enemies.Count - 1) {
                turn = Turn.Player;
                var playerResource = GameObject.FindWithTag("PlayerUnit").GetComponent<Player>().Class.Resource;
                playerResource.Current += playerResource.BaseRestoreRate;
                playerMenu.SetActive(true);
            }
            else {
                DoEnemyTurn();
            }
        }
    }

    void DoEnemyTurn() {
        Enemy e = enemies[turnPos].GetComponent<Enemy>();
        e.Act();
    }

    void SetupWorld(Scene scene, LoadSceneMode mode) {

        //Transfer player stats to the world scene
        GameObject playerUnit = GameObject.FindGameObjectWithTag("PlayerUnit");
        GameObject.FindGameObjectWithTag("Player World").GetComponent<Player>().TransferValues(playerUnit.GetComponent<Player>());
        Destroy(playerUnit);
        GameObject worldData = GameObject.FindGameObjectWithTag("World Data");
        worldData.GetComponent<WorldData>().Reload();
        RemoveLoadingEvent();
        

    }
    void RemoveLoadingEvent() {
        SceneManager.sceneLoaded -= SetupWorld;
    }

    void UnitDeathEnd() {
        Invoke("NextTurn",0.2f);
    }
    void PlayerDeathEnd() {
        Invoke("GameOver", 1f);
    }

    void GameOver() {
        Instantiate(gameOverScreen);
    }

    void CheckUnitStatuses() {
        bool unitIsDying = false;
        if (turn == Turn.Player) {
            foreach(GameObject e in enemies) {
                if (e.GetComponent<Unit>().CurrentHealth <= 0) {
                    e.GetComponent<Unit>().Kill();
                    e.GetComponent<Unit>().killed.AddListener(UnitDeathEnd);
                    unitIsDying = true;
                }
            }
        }
        else {
            foreach(GameObject c in characters) {
                if (c.GetComponent<Unit>().CurrentHealth <= 0) {
                    c.GetComponent<Unit>().Kill();
                    c.GetComponent<Unit>().killed.AddListener(PlayerDeathEnd);
                    unitIsDying = true;
                }
            }
        }
        if (!unitIsDying) {
            Invoke("NextTurn",0.3f);
        }
    }

    void PopulateAbilityMenu(List<Ability> abilities) {
        _abilities = abilities;
        int offset = 0;
        if (abilityTexts != null) {
            foreach (GameObject obj in abilityTexts)
                Destroy(obj.gameObject);
        }
        abilityTexts = new List<GameObject>();
        foreach (Ability ability in abilities) {
            var text = Instantiate(abilityText, Vector3.zero, Quaternion.identity, playerAbilityMenu.transform);
            text.GetComponent<Text>().text = ability.Name;
            RectTransform textRect = text.GetComponent<RectTransform>();
            textRect.localPosition = new Vector3(-80, -110 - offset, 100);
            offset += 20;
            abilityTexts.Add(text);

        }
        UpdateAbilityMenuSelection();
    }
    void UpdateAbilityMenuSelection() {
        RectTransform rect = abilityMenuSelection.GetComponent<RectTransform>();
        RectTransform textRect = abilityTexts[abilityMenuPos].GetComponent<RectTransform>();
        rect.localPosition = new Vector3(textRect.localPosition.x - (textRect.sizeDelta.x / 2) - 8, textRect.localPosition.y + (textRect.sizeDelta.y / 4) + 1, 100);
        abilityDescriptionText.text = _abilities[abilityMenuPos].Description;
    }
    void PopulateItemMenu(Dictionary<Item, int> items) {
        itemMenuPos = 0;
        _items = items;
        int offset = 0;
        if (itemTexts != null) {
            foreach (GameObject obj in itemTexts)
                Destroy(obj.gameObject);
        }
        itemTexts = new List<GameObject>();
        foreach ( KeyValuePair<Item, int> item in items) {
            var text = Instantiate(abilityText, Vector3.zero, Quaternion.identity, playerItemMenu.transform);
            text.GetComponent<Text>().text = item.Key + "     " + item.Value;
            RectTransform textRect = text.GetComponent<RectTransform>();
            textRect.localPosition = new Vector3(-80, -110 - offset, 100);
            offset += 20;
            itemTexts.Add(text);

        }
        UpdateItemMenuSelection();
    }
    void UpdateItemMenuSelection() {
        RectTransform rect = itemMenuSelection.GetComponent<RectTransform>();
        RectTransform textRect = itemTexts[itemMenuPos].GetComponent<RectTransform>();
        rect.localPosition = new Vector3(textRect.localPosition.x - (textRect.sizeDelta.x / 2) - 8, textRect.localPosition.y + (textRect.sizeDelta.y / 4) + 1, 100);
        List<Item> keys = new List<Item>(_items.Keys);
        itemDescriptionText.text = keys[itemMenuPos].Description;
    }
    

    void UpdateMenuSelection() {
        RectTransform rect = playerMenuSelection.GetComponent<RectTransform>();
        rect.localPosition = new Vector3(-72 + playerMenuPos * 48, rect.localPosition.y, rect.localPosition.z);
    }

    void UpdateUnitSelection() {
        RectTransform rect = unitSelection.GetComponent<RectTransform>();
        GameObject enemy = enemies[unitSelectionPos];
        rect.localPosition = new Vector3( enemy.transform.position.x * 32 / 2 - 64, enemy.transform.position.y * 32 / 2, 100);
    }
    void UpdateUnitSelectionFriendly() {
        RectTransform rect = unitSelectionFriendly.GetComponent<RectTransform>();
        GameObject character = characters[unitSelectionFriendlyPos];
        rect.localPosition = new Vector3( character.transform.position.x * 32 / 2 - 32, character.transform.position.y * 32 / 2, 100);
    }
    void UnitSelectionAscend() {
        unitSelectionPos--;
        if (unitSelectionPos < 0)
            unitSelectionPos = enemies.Count - 1;
        UpdateUnitSelection();
    }
    void UnitSelectionDescend() {
        unitSelectionPos++;
        if (unitSelectionPos > enemies.Count - 1)
            unitSelectionPos = 0;
        UpdateUnitSelection();
    }

    public void InflictDamage(Unit target, float damage) {
        target.CurrentHealth -= (int) damage;
        var pos = target.gameObject.transform.position;
        var o = Instantiate(damageText, pos + new Vector3(0.25f,target.gameObject.GetComponent<SpriteRenderer>().bounds.size.y / 2,0), Quaternion.identity);
        var dText = o.GetComponent<DamageText>();
        dText.SetText( (int) damage );
        if (turn == Turn.Player) {
            dText.destroyed.AddListener(CheckUnitStatuses);
        }
        else {
            dText.destroyed.AddListener(CheckUnitStatuses);
        }
    }

    public void Heal(Unit target, float amount) {
        target.CurrentHealth += (int) amount;
        if (target.CurrentHealth > target.MaxHealth)
            target.CurrentHealth = target.MaxHealth;
        var pos = target.gameObject.transform.position;
        var o = Instantiate(damageText, pos + new Vector3(0.25f,target.gameObject.GetComponent<SpriteRenderer>().bounds.size.y / 2,0), Quaternion.identity);
        var dText = o.GetComponent<DamageText>();
        dText.SetText( (int) amount );
        dText.SetColor(Color.green);
        dText.destroyed.AddListener(CheckUnitStatuses);
    }

    private void EndEnemyTurn() {
        Invoke("NextTurn",0.4f);
    }

    public List<GameObject> GetCharacters() {
        return characters;
    }
}
