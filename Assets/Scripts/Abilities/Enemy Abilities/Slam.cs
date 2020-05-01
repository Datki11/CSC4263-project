using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slam : Ability
{
    // Start is called before the first frame update
    public Slam()
    {
        Name = "Slam";
        Description = "An attack that deals physical damage";
        Type = TargetType.Single;
        Cost = 0;
    }

    public override void Action(Unit target, Unit caster) {
        int damageMin = Mathf.Max(1, 17 - caster.Defense);
        int damageMax = Mathf.Max(1, 22 - caster.Defense);

        BattleManager.Instance.InflictDamage(target, caster, Mathf.RoundToInt(Random.Range(damageMin,damageMax)));
    }
}
