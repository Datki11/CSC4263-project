using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonTentacle : Ability
{
    public SummonTentacle()
    {
        Name = "Summon Tentacle";
        Description = "Summons a Tentacle friend";
        Type = TargetType.Self;
        Cost = 0;
    }

    public override void Action(Unit target, Unit caster)
    {
        GameObject.Instantiate(GameObject.Find("Tentacle Enemy"), GameObject.Find("Boss").transform);
    }
}
