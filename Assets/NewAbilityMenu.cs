using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class NewAbilityMenu : MonoBehaviour
{
    private Player player;
    private int menuPos;
    public Text abilityDescription;
    public GameObject menuTexts;
    public GameObject menuIndicator;
    public GameObject abilityText;
    public UnityEvent finished;
    private int selectionNum = -1;
    
    // Start is called before the first frame update
    void Start()
    {
        menuPos = 2;
        player = GameObject.FindGameObjectWithTag("PlayerUnit").GetComponent<Player>();
        

        float offset = 20f;
        RectTransform origin = menuTexts.GetComponent<RectTransform>();
        foreach(Ability ability in player.Class.UnlearnedAbilities) {
            GameObject textObj = Instantiate(abilityText, menuTexts.transform.position, Quaternion.identity, menuTexts.transform );
            offset -= 18f;
            textObj.GetComponent<Text>().text = ability.Name;
            textObj.GetComponent<RectTransform>().localPosition += new Vector3(0f, offset, 0);
        }

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
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Z)) {
            if (menuPos >= 2) {
                selectionNum = menuPos - 2;
                UpdateMenuSelection();
            }
            else if (menuPos == 1 ) {
                player.Class.Abilities.Add(player.Class.UnlearnedAbilities[selectionNum]);
                player.Class.UnlearnedAbilities.RemoveAt(selectionNum);
                finished?.Invoke();
                BattleManager.Instance.BattleEnd();
            }
        }
    }

    private void UpdateMenuSelection() {
        float xOffset = -74f;
        if (menuPos == 1) {
            xOffset = -30f;
            abilityDescription.text = "Confirm your new ability selection";
        }
        else {
            Ability ability = player.Class.UnlearnedAbilities[menuPos - 2];
            abilityDescription.text = ability.Description;
        }
        RectTransform rect = menuIndicator.GetComponent<RectTransform>();
        RectTransform textRect = menuTexts.transform.GetChild(menuPos).GetComponent<RectTransform>();
        rect.localPosition = new Vector3(textRect.localPosition.x + xOffset, textRect.localPosition.y, rect.localPosition.z);
        
        if (selectionNum != -1) {
            for (int i = 2; i < 2 + player.Class.UnlearnedAbilities.Count; i++) {
                Color color = Color.white;
                if (selectionNum + 2 == i)
                    color = Color.green;
                menuTexts.transform.GetChild(i).gameObject.GetComponent<Text>().color = color;
            }
        }

    }
}
