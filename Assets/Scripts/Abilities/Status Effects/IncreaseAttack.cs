using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseAttack : StatusEffect
{
    public float magnitude;
    public IncreaseAttack() {
        type = StatusEffectType.IncreaseAttackSingleUse;
    }
}
