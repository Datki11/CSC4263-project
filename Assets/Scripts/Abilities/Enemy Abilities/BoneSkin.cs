using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneSkin : Ability
{
    public BoneSkin()
    {
        Name = "BoneSkin";
        Description = "Reduces damage from player attacks for the next 3 turns";
        Type = TargetType.Self;
        Cost = 30;
    }

    public override void Action(Unit target, Unit caster)
    {
        BattleManager.Instance.IncreaseDefense(caster, 0.5f, 3);
    }
}
