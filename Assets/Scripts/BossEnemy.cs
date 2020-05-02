using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy
{
    public GameObject mobToAdd;
    private Animator animator;
    public GameObject tentacle1;
    public GameObject tentacle2;
    public override void Awake()
    {
        base.Awake();
        Abilities = new List<Ability>();
        Ability ability = new Slam();
        ability.attackStart.AddListener(SlamAnimate);
        Abilities.Add(ability);
        Abilities.Add(new BoneSkin());
        ability = new SummonTentacle();
        ability.attackStart.AddListener(AttackAnimate);
        Abilities.Add(ability);
        Items = new Dictionary<Item, int>();
        Items.Add(new Potion(), 2);
        MaxHealth = 60;
        CurrentHealth = 60;
        Attack = 5;
        Defense = 3;
        Speed = 6;
        animator = GetComponent<Animator>();
    }

    public override void Act()
    {
        List<GameObject> characters = BattleManager.Instance.GetCharacters();
        List<GameObject> enemies = BattleManager.Instance.GetEnemies();
        List<Ability> availableAbilities = new List<Ability>();
        int characterNum = Mathf.RoundToInt(Random.Range(0, characters.Count));

        int abilityOrItem = 1; //Only abilities are available
        if (Items.Count > 0 && CurrentHealth <= MaxHealth / 2) //If there are Items available to use AND the current health is less than or equal to half of Max Health...
        {
            abilityOrItem = 2; //Items are also available to use
        }

        int use = Random.Range(0, abilityOrItem); //Randomly selecting if the Boss is using an ability (0) or an Item (1)

        if (use == 0) //Set up and use abilities
        {
            availableAbilities.Add(Abilities[0]); //Add Slam to available Abilities
            if (CurrentHealth <= MaxHealth / 2)
            {
                availableAbilities.Add(Abilities[1]); //Add Bone Skin to available Abilities
            }

            if (enemies.Count <= 3)
            {
                availableAbilities.Add(Abilities[2]); //Add Summon Tentacle to available Abilities
            }

            use = Random.Range(0, availableAbilities.Count); //Pick a random ability out of those available
            //Don't use Summon if there are 3 enemies
            if (availableAbilities[use].Name == "Summon Tentacle" && BattleManager.Instance.GetEnemies().Count >= 3)
                use = 0;
            availableAbilities[use].Action(characters[characterNum].GetComponent<Unit>(), this); //Perform random ability
        }
        else if (use == 1) //Use a random Item
        {
            List<Item> itemKeys = new List<Item>(Items.Keys);//Get the items names available
            use = Random.Range(0, itemKeys.Count); //Select a random item to use
            itemKeys[use].Use(this); //Use the item's ability
            Items[itemKeys[use]] -= 1; //Remove the item from Boss' inventory

            if (Items[itemKeys[use]] <= 0) //Check if there are any of this item type left, if none of that item type are left...
            {
                Items.Remove(itemKeys[use]); //Remove the item from inventory
            }
        }
    }

    public void SlamAnimate() {
        animator.SetTrigger("Slam");
    }
}
