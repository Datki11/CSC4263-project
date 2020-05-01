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
        MaxHealth = 60;
        CurrentHealth = 60;
        Attack = 5;
        Defense = 3;
        Speed = 6;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
