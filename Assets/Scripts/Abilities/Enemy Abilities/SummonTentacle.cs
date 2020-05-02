using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonTentacle : Ability
{
    public GameObject tentacle;
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
        List<GameObject> enemies = BattleManager.Instance.GetEnemies();
        if (enemies.Count <= 2)
        {
            tentacle = BattleManager.Instantiate(boss.mobToAdd, boss.transform);
            tentacle.transform.position += new Vector3(5.0f, -3.0f, -200.0f);
        }
        else
        {
            tentacle = BattleManager.Instantiate(boss.mobToAdd, boss.transform);
            tentacle.transform.position += new Vector3(10.0f, 3.0f, -200.0f);
        }
        BattleManager.Instance.EnemyAdded();
        BattleManager.Instance.EndEnemyTurn();
    }
}
