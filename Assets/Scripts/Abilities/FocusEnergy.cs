using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusEnergy : Ability
{
    
    // Start is called before the first frame update
    public FocusEnergy() {
        Name = "Fury";
        Description = "Expends a large amount of rage to inflict a devastating blow on a single enemy";
        Type = TargetType.Self;
        Cost = 30;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Action(Unit target) {
        BattleManager.Instance.InflictDamage(target, Mathf.RoundToInt(Random.Range(22,29)));
    }
}
