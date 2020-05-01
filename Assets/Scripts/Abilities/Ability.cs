using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Ability
{
    public UnityEvent attackStart = new UnityEvent();
    public string Name {get; set;}
    public string Description {get; set;}

    public int Cost {get; set;}
    public TargetType Type;

    Character User;
    Character Target;

    float powerRange;
    public abstract void Action(Unit target, Unit caster);

}
