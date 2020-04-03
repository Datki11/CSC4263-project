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

    public override void Action(Unit target) {
        BattleManager.Instance.InflictDamage(target, Mathf.RoundToInt( Random.Range(15,18)));
    }
}
