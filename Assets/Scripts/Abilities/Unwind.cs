using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unwind : Ability
{
    public Unwind() {
        Name = "Unwind";
        Description = "Expends all your rage to heal yourself.";
        Type = TargetType.Self;
        Cost = 0;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Action(Unit target, Unit caster) {
        BattleManager.Instance.HealWithRage();
    }
}
