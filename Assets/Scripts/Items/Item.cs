using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item
{
    public string Name {get; set;}
    public string Description {get; set;}
    public bool OnlyInBattle {get; set;}
    public bool IsFriendly {get; set;}
    public abstract void Use(Unit target);

    public virtual void UseThroughPauseMenu(Unit target) {
        Debug.Log("Method not overriden");
    }
}
