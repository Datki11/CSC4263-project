using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusEnergy : Ability
{
    
    // Start is called before the first frame update
    public FocusEnergy() {
        Name = "Focus Energy";
        Description = "increase damage and/or critical hit rate for X turns, expend Rage";
        Type = TargetType.Self;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Action(Unit target) {
        //Do something
    }
}
