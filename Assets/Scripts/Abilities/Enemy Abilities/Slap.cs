using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slap : Ability
{
  // Start is called before the first frame update
    public Slap() {
        Name = "Slap";
        Description = "An attack that deals physical damage";
        Type = TargetType.Single;
        Cost = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Action(Unit target, Unit caster) {
        int damageMin = Mathf.Max(1, 7 - caster.Defense / 2);
        int damageMax = Mathf.Max(1, 10 - caster.Defense / 2);

        BattleManager.Instance.InflictDamage(target, caster, Mathf.RoundToInt(Random.Range(damageMin,damageMax)));
    }
}
