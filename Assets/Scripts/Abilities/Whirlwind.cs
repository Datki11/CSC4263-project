using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whirlwind : Ability
{
    public Whirlwind() {
        Name = "Whirlwind";
        Description = "Attack all enemies, expend Rage, cannot attack next turns";
        Type = TargetType.AllEnemy;
        Cost = 10;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Action(Unit target) {
        BattleManager.Instance.InflictDamage(target, 7);
    }
}
