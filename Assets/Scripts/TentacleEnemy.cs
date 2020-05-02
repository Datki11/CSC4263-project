using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleEnemy : Enemy
{
    public override void Awake()
    {
        base.Awake();
        Abilities = new List<Ability>();
        Abilities.Add(new Slap());
        MaxHealth = 10;
        CurrentHealth = 10;
        Attack = 5;
        Defense = 3;
        Speed = 6;
        Exp = 7;
        GoldRange = new Vector2(5, 7);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Act() {
        base.Act();
    }
}
