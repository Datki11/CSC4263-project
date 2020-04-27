using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class LevelUpMenu : MonoBehaviour
{
    private Player player;
    private int menuPos;
    public Text attackValueText;
    public Text defenseValueText;
    public Text speedValueText;
    public Text statDescription;
    private int attackIncrement;
    private int defenseIncrement;
    private int speedIncrement;
    public GameObject menuTexts;
    public GameObject menuIndicator;
    public Text pointsLeftText;
    private int points;
    public UnityEvent finished;
    
    // Start is called before the first frame update
    void Start()
    {
        points = 3;
        menuPos = 1;
        player = GameObject.FindGameObjectWithTag("PlayerUnit").GetComponent<Player>();
        UpdateMenuSelection();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            menuPos++;
            if (menuPos > menuTexts.transform.childCount - 1)
                menuPos = 1;
            UpdateMenuSelection();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            menuPos--;
            if (menuPos < 1)
                menuPos = menuTexts.transform.childCount - 1;
            UpdateMenuSelection();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && points > 0) {
            if (menuPos == 1) {
                attackIncrement++;
                points--;
            }
            if (menuPos == 2) {
                defenseIncrement++;
                points--;
            }
            if (menuPos == 3) {
                speedIncrement++;
                points--;
            }
            UpdateMenuSelection();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            if (menuPos == 1 && attackIncrement > 0) {
                attackIncrement--;
                points++;
            }
            if (menuPos == 2 && defenseIncrement > 0) {
                defenseIncrement--;
                points++;
            }
            if (menuPos == 3 && speedIncrement > 0) {
                speedIncrement--;
                points++;
            }
            UpdateMenuSelection();
        }
        if (Input.GetKeyDown(KeyCode.Return)) {
            if (menuPos == 4) {
                player.Attack += attackIncrement;
                player.Speed += speedIncrement;
                player.Defense += defenseIncrement;
                finished?.Invoke();
                BattleManager.Instance.BattleEnd();
            }
        }
    }

    private void UpdateMenuSelection() {
        float xOffset = -60f;
        if (menuPos == 1) {
            statDescription.text = "Influences how much damage you inflict on enemies";
        }
        else if (menuPos == 2) {
            statDescription.text = "Reduces the damage you take from enemies";
        }
        else if (menuPos == 3) {
            statDescription.text = "Determines how fast your rage increases in battle";
        }
        else if (menuPos == 4) {
            xOffset = -30f;
            statDescription.text = "Confirm your stat boosts and return to the world";
        }
        int totalAttack, totalDefense, totalSpeed;
        totalAttack = player.Attack + attackIncrement;
        totalDefense = player.Defense + defenseIncrement;
        totalSpeed = player.Speed + speedIncrement;
        attackValueText.text = totalAttack.ToString();
        defenseValueText.text = totalDefense.ToString();
        speedValueText.text = totalSpeed.ToString();
        if (attackIncrement > 0)
            attackValueText.color = Color.green;
        else
            attackValueText.color = Color.white;
        if (defenseIncrement > 0)
            defenseValueText.color = Color.green;
        else
            defenseValueText.color = Color.white;
        if (speedIncrement > 0)
            speedValueText.color = Color.green;
        else
            speedValueText.color = Color.white;
        pointsLeftText.text = "Points left: " + points.ToString();
        RectTransform rect = menuIndicator.GetComponent<RectTransform>();
        RectTransform textRect = menuTexts.transform.GetChild(menuPos).GetComponent<RectTransform>();
        rect.localPosition = new Vector3(textRect.localPosition.x + xOffset, textRect.localPosition.y, rect.localPosition.z);

    }
}
