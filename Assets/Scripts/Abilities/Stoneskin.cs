using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stoneskin : Ability
{
    public Stoneskin() {
        Name = "Stoneskin";
        Description = "Reduces damage from enemy attacks for the next 3 turns";
        Type = TargetType.Self;
        Cost = 30;
    }
    

    public override void Action(Unit target, Unit caster) {
        BattleManager.Instance.IncreaseDefense(target, 0.5f, 3);
    }
}
