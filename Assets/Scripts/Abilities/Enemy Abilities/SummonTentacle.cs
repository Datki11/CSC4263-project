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
        BossEnemy boss = caster as BossEnemy;
        BattleManager.Instantiate(boss.mobToAdd, caster.transform);
        BattleManager.Instance.EnemyAdded();
        BattleManager.Instance.EndEnemyTurn();
    }
}
