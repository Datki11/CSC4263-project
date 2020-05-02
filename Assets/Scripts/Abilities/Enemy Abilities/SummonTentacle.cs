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
        if (boss.tentacle1 == null)
        {
            tentacle = BattleManager.Instantiate(boss.mobToAdd, boss.transform.position, Quaternion.identity);
            boss.tentacle1 = tentacle;
            tentacle.transform.position += new Vector3(10.0f, 0f, 0f);
            
        }
        else if (boss.tentacle2 == null)
        {
            tentacle = BattleManager.Instantiate(boss.mobToAdd, boss.transform.position, Quaternion.identity);
            boss.tentacle2 = tentacle;
            
            tentacle.transform.position += new Vector3(7.0f, -7.0f, 0f);
        }
        BattleManager.Instance.EnemyAdded();
        attackStart?.Invoke();
        
    }
}
