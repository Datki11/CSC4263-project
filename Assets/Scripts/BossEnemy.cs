using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy
{
    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
        Abilities = new List<Ability>();
        Abilities.Add(new Slam());
        Abilities.Add(new BoneSkin());
        Items = new Dictionary<Item, int>();
        Items.Add(new Potion(), 2);
        Items.Add(new BigPotion(), 1);
        MaxHealth = 60;
        CurrentHealth = 60;
        Attack = 5;
        Defense = 3;
        Speed = 6;
    }
}
