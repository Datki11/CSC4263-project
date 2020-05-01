using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adrenaline : Item
{
    // Start is called before the first frame update
        public Adrenaline() {

        Name = "Adrenaline";
        Description = "Immediately raises your rage by 50";
        OnlyInBattle = true;
        IsFriendly = true;

    }

    public override void Use(Unit target) {
        BattleManager.Instance.IncreaseRage(target, 50);
    }
}
