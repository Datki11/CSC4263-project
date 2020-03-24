using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatEnemy : Enemy
{
    void Awake()
    {
        Abilities = new List<Ability>();
        Abilities.Add(new Bite());
        MaxHealth = 20;
        CurrentHealth = 20;
        Attack = 5;
        Defense = 3;
        Speed = 6;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Act() {
        base.Act();
    }
}
