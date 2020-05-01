using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lunge : Ability
{
    
    public Lunge() {
        Name = "Lunge";
        Description = "attacks a single enemy";
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

    public override void Action(Unit target, Unit caster) {
        BattleManager.Instance.InflictDamage(target, caster, Mathf.RoundToInt(Random.Range(5 + caster.Attack / 2, 9 + caster.Attack / 2)));
        attackStart?.Invoke();
    }
}
