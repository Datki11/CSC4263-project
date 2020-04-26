using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Item
{
    public Potion() {

        Name = "Potion";
        Description = "Heals 30 health";
        OnlyInBattle = false;
        IsFriendly = true;

    }

    public override void Use(Unit target) {
        BattleManager.Instance.Heal(target, 30);
    }

    public override void UseThroughPauseMenu(Unit target) {
        target.CurrentHealth += 30;
    }
}
