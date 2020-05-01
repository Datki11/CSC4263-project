using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusPower : Ability
{
    public FocusPower() {
        Name = "Focus Power";
        Description = "Doubles the damage of your next move";
        Type = TargetType.Self;
        Cost = 15;
    }
    

    public override void Action(Unit target, Unit caster) {
        BattleManager.Instance.IncreaseAttack(target, 1f, 99);
    }
}
