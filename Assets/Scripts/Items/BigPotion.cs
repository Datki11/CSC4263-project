using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigPotion : Item
{
    public BigPotion() {

        Name = "Big Potion";
        Description = "Heals 60 health";
        OnlyInBattle = false;
        IsFriendly = true;

    }

    public override void Use(Unit target) {
        BattleManager.Instance.Heal(target, 60);
    }

    public override void UseThroughPauseMenu(Unit target) {
        target.CurrentHealth += 60;
    }
}
