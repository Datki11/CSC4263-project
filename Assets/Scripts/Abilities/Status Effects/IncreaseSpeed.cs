using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseSpeed : StatusEffect
{
    public float magnitude;

    public IncreaseSpeed() {
        type = StatusEffectType.Speed;
    }
}
