using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullEnemy : Enemy
{
    public override void Awake()
    {
        base.Awake();
        Abilities = new List<Ability>();
        Abilities.Add(new Bite());
        MaxHealth = 25;
        CurrentHealth = 25;
        Attack = 5;
        Defense = 3;
        Speed = 6;
        Exp = 17;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Act() {
        base.Act();
    }
}
