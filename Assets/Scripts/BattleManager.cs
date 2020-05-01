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
    public GameObject gameWonScreen;
    public GameObject levelUpMenu;
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

    public GameObject world;

    public GameObject playerBattleEndText;

    private static BattleManager _instance;

    //For the prototype
    public bool isFinalBoss = false;

    public static BattleManager Instance { get { return _instance; } }
    
    private enum Turn {
        Player,
        Enemy
    }
    private Turn turn = Turn.Player;
    private int turnPos = 0;
    private int totalExpEarned = 0;
    private int totalGoldEarned = 0;
    private bool battleEnded = false;
    private List<StatusEffect> playerStatusEffects;
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
        playerStatusEffects = new List<StatusEffect>();
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
                    playerMenuPos = 2;
                UpdateMenuSelection();
            }
            else if (unitSelection.activeSelf) {
                UnitSelectionDescend();
            }
        }


        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            if (playerMenu.activeSelf) {
                playerMenuPos++;
                if (playerMenuPos > 2)
                    playerMenuPos = 0;
                UpdateMenuSelection();
            }
            else if (unitSelection.activeSelf) {
                UnitSelectionAscend();
            }   
        }


        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            if (playerAbilityMenu.activeSelf) {
                AbilityMenuDescend();
            }
            else if (unitSelection.activeSelf) {
                UnitSelectionAscend();
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
                AbilityMenuAscend();
            }
            else if (unitSelection.activeSelf) {
                UnitSelectionDescend();
            }
            else if (playerItemMenu.activeSelf) {
                itemMenuPos--;
                if (itemMenuPos < 0)
                    itemMenuPos = itemTexts.Count - 1;
                UpdateItemMenuSelection();
            }  
        }

        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return)) {
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
                else if (playerMenuPos == 2) {
                    AttemptToFlee();
                    playerMenu.SetActive(false);
                }
            }
            else if (playerAbilityMenu.activeSelf) {

                Ability ability = _abilities[abilityMenuPos];
                //Checks to make sure the player has the rage
                if (characters[0].GetComponent<Player>().Class.Resource.Current >= ability.Cost) {
                    
                    //If the player selected an ability that attacks all enemies, don't do unit selection
                    
                    if (ability.Type == TargetType.AllEnemy) {
                        Player player = characters[0].GetComponent<Player>();
                        player.Class.Resource.Current -= ability.Cost;
                        playerAbilityMenu.SetActive(false);
                        ability.Action(null, player);
                    }

                    //If the player selected an ability that affects himself, don't do unit selection
                    else  if (ability.Type == TargetType.Self) {
                        Player player = characters[0].GetComponent<Player>();
                        player.Class.Resource.Current -= ability.Cost;
                        ability.Action(null, player);
                        playerAbilityMenu.SetActive(false);

                    }
                    //Do unit selection
                    else {
                        playerAbilityMenu.SetActive(false);
                        unitSelection.SetActive(true);
                        UpdateUnitSelection();
                    }
                }
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
                    //Depletes the player's resource
                    Player player = characters[0].GetComponent<Player>();
                    player.Class.Resource.Current -= ability.Cost;
                    
                    switch (ability.Type)
                    {
                        case TargetType.Single:
                            ability.Action(enemies[unitSelectionPos].GetComponent<Unit>(), player);
                            break;

                        case TargetType.AllEnemy:
                            foreach (GameObject enemy in enemies.ToArray())
                                ability.Action(enemy.GetComponent<Unit>(), player);
                            break;

                        default:
                            ability.Action(enemies[unitSelectionPos].GetComponent<Unit>(), player);
                            break;
                    }

                    

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
                //Make sure the player actually has any items
                if (_items.Keys.Count > 0) {
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
        }


        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Escape)) {
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
            //For the prototype, game ends if fighting the boss
            if (isFinalBoss) {
                GameWon();
            }
            else {
                
                //Prevents the function from being called twice
                if (battleEnded)
                    return;
                else
                    battleEnded =  true;
                StartCoroutine("ShowBattleEndNotifications");
            }
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
                Player player = GameObject.FindWithTag("PlayerUnit").GetComponent<Player>();
                var playerResource = player.Class.Resource;
                playerResource.Current += player.Speed * 2;
                StatusEffect st = player.statusEffects.Find(x => x.type == StatusEffectType.Speed);
                if (st != null) {
                    playerResource.Current += player.Speed * 2;
                }
                if (playerResource.Current > playerResource.BaseMax)
                    playerResource.Current = playerResource.BaseMax;
                playerMenu.SetActive(true);
                List<StatusEffect> effectsToRemove = new List<StatusEffect>();
                foreach(StatusEffect statusEffect in player.statusEffects) {
                    statusEffect.rounds--;
                    if (statusEffect.rounds <= 0) {
                        effectsToRemove.Add(statusEffect);
                        
                    }
                }
                foreach (StatusEffect statusEffect in effectsToRemove) {
                    player.statusEffects.Remove(statusEffect);
                }
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
        world.SetActive(true);
        GameObject playerUnit = GameObject.FindGameObjectWithTag("PlayerUnit");
        GameObject.FindGameObjectWithTag("Player World").GetComponent<Player>().TransferValues(playerUnit.GetComponent<Player>());
        GameObject.FindGameObjectWithTag("Player World").GetComponent<Player>().CurrentHealth = playerUnit.GetComponent<Player>().CurrentHealth;
        Destroy(playerUnit);
        SceneManager.sceneLoaded -= SetupWorld;
        

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
    void GameWon() {
        Instantiate(gameWonScreen);
    }

    public void BattleEnd() {
        GameObject playerObj = GameObject.FindGameObjectWithTag("PlayerUnit").gameObject;
        DontDestroyOnLoad(playerObj);
        SceneManager.sceneLoaded += SetupWorld;
        SceneManager.LoadScene("Empty Scene");
    }

    private IEnumerator ShowBattleEndNotifications() {
        
        GameObject playerObj = GameObject.FindGameObjectWithTag("PlayerUnit").gameObject;
        Player player = playerObj.GetComponent<Player>();
        for(int i = 0; i < playerObj.transform.childCount; i++) {
            playerObj.transform.GetChild(i).gameObject.SetActive(false);
        }
        List<string> notifications = new List<string>();
        notifications.Add("+" + totalExpEarned.ToString() + " exp" );
        notifications.Add("+" + totalGoldEarned.ToString() + " gold");
        player.CurrentExp += totalExpEarned;
        player.Gold += totalGoldEarned;
        if (player.CurrentExp >= player.ExpToLevelUp) {
            notifications.Add("Level Up");
        }
        foreach(string str in notifications) {
            GameObject textObj = Instantiate(playerBattleEndText, playerObj.transform.position + new Vector3(0,1f,0), Quaternion.identity);
            textObj.GetComponent<BattleEndText>().SetText(str);
            if (str == "Level Up")
                textObj.GetComponent<BattleEndText>().SetColor(Color.green);
            yield return new WaitForSeconds(0.6f);
        }
        yield return new WaitForSeconds(1.25f);
        
        
        if (player.CurrentExp >= player.ExpToLevelUp) {
            player.LevelUp();
            levelUpMenu.SetActive(true);
            
        }
        else 
            BattleEnd();
    }

    void AttemptToFlee() {
        GameObject playerObj = GameObject.FindGameObjectWithTag("PlayerUnit").gameObject;
        //33% chance of failure
        if(Random.Range(0f, 1f) <= 0.33f || isFinalBoss) {
            
            Player player = playerObj.GetComponent<Player>();
            GameObject textObj = Instantiate(playerBattleEndText, playerObj.transform.position + new Vector3(0,2f,0), Quaternion.identity);
            textObj.GetComponent<BattleEndText>().SetText("Failed to escape");
            textObj.GetComponent<BattleEndText>().SetColor(Color.red);
            Invoke("NextTurn", 1.5f);
        }
        else {
            playerObj.GetComponent<Rigidbody2D>().velocity = new Vector2(35f, 0f);
            GameObject textObj = Instantiate(playerBattleEndText, playerObj.transform.position + new Vector3(0,2f,0), Quaternion.identity);
            textObj.GetComponent<BattleEndText>().SetText("Success!");
            textObj.GetComponent<BattleEndText>().SetColor(Color.green);
            Invoke("BattleEnd", 1.8f);
        }


    }

    void CheckUnitStatuses() {
        bool unitIsDying = false;
        if (turn == Turn.Player) {
            foreach(GameObject e in enemies) {
                if (e.GetComponent<Unit>().CurrentHealth <= 0) {
                    totalExpEarned += e.GetComponent<Enemy>().Exp;
                    totalGoldEarned += Mathf.RoundToInt(Random.Range(e.GetComponent<Enemy>().GoldRange.x, e.GetComponent<Enemy>().GoldRange.y));
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
        int pageNum = abilityMenuPos / 4;
        _abilities = abilities;
        int offset = 0;
        if (abilityTexts != null) {
            foreach (GameObject obj in abilityTexts)
                Destroy(obj.gameObject);
        }
        abilityTexts = new List<GameObject>();
        for (int i = pageNum * 4; i < pageNum * 4 + 4 && i < abilities.Count; i++ ) {
            Ability ability = abilities[i];
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
        RectTransform textRect = abilityTexts[abilityMenuPos % 4].GetComponent<RectTransform>();
        rect.localPosition = new Vector3(textRect.localPosition.x - (textRect.sizeDelta.x / 2) - 8, textRect.localPosition.y + (textRect.sizeDelta.y / 4) + 1, 100);
        abilityDescriptionText.text = _abilities[abilityMenuPos].Description + "\n\nCost: " + _abilities[abilityMenuPos].Cost;
    }
    void PopulateItemMenu(Dictionary<Item, int> items) {
        itemMenuPos = 0;
        _items = items;
        int offset = 0;
        if (itemTexts != null) {
            foreach (GameObject obj in itemTexts)
                Destroy(obj.gameObject);
        }
        if (_items.Keys.Count > 0) {
            itemTexts = new List<GameObject>();
            foreach ( KeyValuePair<Item, int> item in items) {
                var text = Instantiate(abilityText, Vector3.zero, Quaternion.identity, playerItemMenu.transform);
                text.GetComponent<Text>().text = item.Key + "     " + item.Value;
                RectTransform textRect = text.GetComponent<RectTransform>();
                textRect.localPosition = new Vector3(-80, -110 - offset, 100);
                offset += 20;
                itemTexts.Add(text);
            }

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
        rect.localPosition = new Vector3(-48 + playerMenuPos * 48, rect.localPosition.y, rect.localPosition.z);
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
    void AbilityMenuDescend() {
        int oldPos = abilityMenuPos;
        abilityMenuPos++;
        if (abilityMenuPos > _abilities.Count - 1) {
            abilityMenuPos = 0;
            PopulateAbilityMenu(_abilities);
        }
        if (abilityMenuPos / 4 > oldPos / 4) {
            PopulateAbilityMenu(_abilities);
        }
        UpdateAbilityMenuSelection();
    }

    void AbilityMenuAscend() {
        int oldPos = abilityMenuPos;
        abilityMenuPos--;
        if (abilityMenuPos / 4 < oldPos / 4) {
            PopulateAbilityMenu(_abilities);
        }
        if (abilityMenuPos < 0) {
            abilityMenuPos = _abilities.Count - 1;
            PopulateAbilityMenu(_abilities);
        }
        UpdateAbilityMenuSelection();
    }

    public void InflictDamage(Unit target, Unit caster, float damage) {

        //Applies status effects
        if (target.statusEffects.Count > 0) {
            StatusEffect statusEffect;

            statusEffect = target.statusEffects.Find(x => x.type == StatusEffectType.ReduceDamage);
            if (statusEffect != null) {
                ReduceDamage reduceDamage = (ReduceDamage) statusEffect;
                damage -= damage * reduceDamage.magnitude;
            }
        }

        if (caster != null) {
            if (caster.statusEffects.Count > 0) {
                StatusEffect statusEffect = caster.statusEffects.Find(x => x.type == StatusEffectType.IncreaseAttackSingleUse);
                if (statusEffect != null) {
                    IncreaseAttack increaseAttack = (IncreaseAttack) statusEffect;
                    damage =(float) damage * (1f + increaseAttack.magnitude);
                    caster.statusEffects.Remove(statusEffect);
                }
            }
        }

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

    public void InflictDamageToAllUnits(UnitType unitType, Unit caster, float damage) {
        
        if (caster != null) {
            if (caster.statusEffects.Count > 0) {
                StatusEffect statusEffect = caster.statusEffects.Find(x => x.type == StatusEffectType.IncreaseAttackSingleUse);
                if (statusEffect != null) {
                    IncreaseAttack increaseAttack = (IncreaseAttack) statusEffect;
                    damage =(float) damage * (1f + increaseAttack.magnitude);
                    caster.statusEffects.Remove(statusEffect);
                }
            }
        }

        if (unitType == UnitType.Enemy) {
            bool eventSet = false;
            foreach(GameObject enemy in enemies) {

                Unit target = (Unit) enemy.GetComponent<Enemy>();

                target.CurrentHealth -= (int) damage;
                var pos = target.gameObject.transform.position;
                var o = Instantiate(damageText, pos + new Vector3(0.25f,target.gameObject.GetComponent<SpriteRenderer>().bounds.size.y / 2,0), Quaternion.identity);
                var dText = o.GetComponent<DamageText>();
                dText.SetText( (int) damage );
                if (!eventSet)
                    dText.destroyed.AddListener(CheckUnitStatuses);
                eventSet = true;
            }
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
    public void HealWithRage() {
        Player target = characters[0].GetComponent<Player>();
        int rage = target.Class.Resource.Current;
        target.Class.Resource.Current = 0;
        int amount = Mathf.RoundToInt((float) rage / 70 * target.MaxHealth);
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

    public void IncreaseDefense(Unit target, float percent, int rounds) {
        GameObject playerObj = GameObject.FindGameObjectWithTag("PlayerUnit").gameObject;
        Player player = playerObj.GetComponent<Player>();
        ReduceDamage statusEffect = (ReduceDamage) player.statusEffects.Find(x => x.type == StatusEffectType.ReduceDamage);
        if (statusEffect != null) {
            statusEffect.rounds = rounds;
            statusEffect.magnitude = percent;
        }
        else {
            statusEffect = new ReduceDamage();
            statusEffect.rounds = rounds;
            statusEffect.magnitude = percent;
            player.statusEffects.Add(statusEffect);
        }
        GameObject textObj = Instantiate(playerBattleEndText, playerObj.transform.position + new Vector3(0,2f,0), Quaternion.identity);
        textObj.GetComponent<BattleEndText>().SetText("+ Defense");
        textObj.GetComponent<BattleEndText>().SetColor(Color.green);
        Invoke("NextTurn", 1.5f);

    }

    public void IncreaseAttack(Unit target, float percent, int rounds) {
        GameObject playerObj = GameObject.FindGameObjectWithTag("PlayerUnit").gameObject;
        Player player = playerObj.GetComponent<Player>();
        IncreaseAttack statusEffect = (IncreaseAttack) player.statusEffects.Find(x => x.type == StatusEffectType.IncreaseAttackSingleUse);
        if (statusEffect != null) {
            statusEffect.rounds = rounds;
            statusEffect.magnitude = percent;
        }
        else {
            statusEffect = new IncreaseAttack();
            statusEffect.rounds = rounds;
            statusEffect.magnitude = percent;
            player.statusEffects.Add(statusEffect);
        }
        GameObject textObj = Instantiate(playerBattleEndText, playerObj.transform.position + new Vector3(0,2f,0), Quaternion.identity);
        textObj.GetComponent<BattleEndText>().SetText("+ Attack");
        textObj.GetComponent<BattleEndText>().SetColor(Color.green);
        Invoke("NextTurn", 1.5f);

    }

    public void IncreaseSpeed(Unit target, float percent, int rounds) {
        GameObject playerObj = GameObject.FindGameObjectWithTag("PlayerUnit").gameObject;
        Player player = playerObj.GetComponent<Player>();
        IncreaseSpeed statusEffect = (IncreaseSpeed) player.statusEffects.Find(x => x.type == StatusEffectType.Speed);
        if (statusEffect != null) {
            statusEffect.rounds = rounds;
            statusEffect.magnitude = percent;
        }
        else {
            statusEffect = new IncreaseSpeed();
            statusEffect.rounds = rounds;
            statusEffect.magnitude = percent;
            player.statusEffects.Add(statusEffect);
        }
        GameObject textObj = Instantiate(playerBattleEndText, playerObj.transform.position + new Vector3(0,2f,0), Quaternion.identity);
        textObj.GetComponent<BattleEndText>().SetText("+ Speed");
        textObj.GetComponent<BattleEndText>().SetColor(Color.green);
        Invoke("NextTurn", 1.5f);

    }
    public void IncreaseRage(Unit _target, float amount) {
        Player target = characters[0].GetComponent<Player>();
        target.Class.Resource.Current += (int) amount;
        if (target.Class.Resource.Current > target.Class.Resource.BaseMax)
            target.Class.Resource.Current = target.Class.Resource.BaseMax;
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
