using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firecracker : Item
{
    public Firecracker() {

        Name = "Firecracker";
        Description = "Deals 15 damage";
        OnlyInBattle = true;
        IsFriendly = false;

    }

    public override void Use(Unit target) {
        BattleManager.Instance.InflictDamage(target, 15);
    }
}
