using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whirlwind : Ability
{
    public Whirlwind() {
        Name = "Whirlwind";
        Description = "Attack all enemies by using rage";
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
        BattleManager.Instance.InflictDamageToAllUnits(UnitType.Enemy, Mathf.RoundToInt(Random.Range(7,13)));
        
    }
}
