using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hyper : Ability
{
        public Hyper() {
        Name = "Hyper";
        Description = "Doubles how quickly your rage increases for the next 3 rounds";
        Type = TargetType.Self;
        Cost = 30;
    }

    public override void Action(Unit target, Unit caster) {
        BattleManager.Instance.IncreaseSpeed(caster, 1f, 3);
    }
}
