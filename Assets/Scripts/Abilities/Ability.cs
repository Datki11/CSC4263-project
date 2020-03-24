using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability
{
    public string Name {get; set;}
    public string Description {get; set;}

    Character User;
    Character Target;

    float powerRange;
    public abstract void Action(Unit target);

}
