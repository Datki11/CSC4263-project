using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whirlwind : Ability
{
    public Whirlwind() {
        Name = "Whirlwind";
        Description = "Attack all enemies by using rage";
        Type = TargetType.AllEnemy;
        Cost = 20;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Action(Unit target, Unit caster) {
        BattleManager.Instance.InflictDamageToAllUnits(UnitType.Enemy, caster, Mathf.RoundToInt(Random.Range(2 + caster.Attack, 5 + caster.Attack)));
        
    }
}
