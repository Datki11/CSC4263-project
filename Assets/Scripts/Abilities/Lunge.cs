using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lunge : Ability
{
    public Lunge() {
        Name = "Lunge";
        Description = "attack first on single opponent, expend Rage";
        Type = TargetType.Single;
        Cost = 0;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Action(Unit target) {
        BattleManager.Instance.InflictDamage(target, Mathf.RoundToInt(Random.Range(12,16)));
    }
}
