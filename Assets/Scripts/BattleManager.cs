using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public GameObject playerMenu;
    public GameObject playerMenuSelection;
    public GameObject playerAbilityMenu;
    public GameObject abilityText;
    public GameObject abilityMenuSelection;
    public GameObject unitSelection;
    public Text abilityDescriptionText;
    private int playerMenuPos = 0;
    private int unitSelectionPos = 0;
    private List<GameObject> characters;
    private List<GameObject> enemies;
    private List<Ability> _abilities;
    private int abilityMenuPos = 0;
    private List<GameObject> abilityTexts;
    
    private enum Turn {
        Player,
        Enemy
    }
    private Turn turn = Turn.Player;
    void Awake()
    {
        characters = new List<GameObject>(GameObject.FindGameObjectsWithTag("PlayerUnit"));
        enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("EnemyUnit"));
        NextTurn();
    }

    // Update is called once per frame
    void Update()
    {
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
        }
        if (Input.GetKeyDown(KeyCode.Return)) {
            if (playerMenu.activeSelf) {
                if (playerMenuPos == 0) {
                    playerMenu.SetActive(false);
                    playerAbilityMenu.SetActive(true);
                    PopulateAbilityMenu(characters[0].GetComponent<Character>().Class.Abilities);
                }
            }
            else if (playerAbilityMenu.activeSelf) {
                playerAbilityMenu.SetActive(false);
                unitSelection.SetActive(true);
                UpdateUnitSelection();
            }
        }
    }

    void NextTurn() {

    }

    void PopulateAbilityMenu(List<Ability> abilities) {
        _abilities = abilities;
        int offset = 0;
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

    void UpdateMenuSelection() {
        RectTransform rect = playerMenuSelection.GetComponent<RectTransform>();
        rect.localPosition = new Vector3(-72 + playerMenuPos * 48, rect.localPosition.y, rect.localPosition.z);
    }
    void UpdateAbilityMenuSelection() {
        RectTransform rect = abilityMenuSelection.GetComponent<RectTransform>();
        RectTransform textRect = abilityTexts[abilityMenuPos].GetComponent<RectTransform>();
        rect.localPosition = new Vector3(textRect.localPosition.x - (textRect.sizeDelta.x / 2) - 8, textRect.localPosition.y + (textRect.sizeDelta.y / 4) + 1, 100);
        abilityDescriptionText.text = _abilities[abilityMenuPos].Description;
    }
    void UpdateUnitSelection() {
        RectTransform rect = unitSelection.GetComponent<RectTransform>();
        GameObject enemy = enemies[unitSelectionPos];
        rect.localPosition = new Vector3( enemy.transform.position.x * 32 / 2 - 64, enemy.transform.position.y * 32 / 2, 100);
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
}
