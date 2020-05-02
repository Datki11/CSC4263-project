using UnityEngine;

public class Gold : Item
{

    public Gold() {

        Name = "Gold";
        Description = "Contains gold";
        OnlyInBattle = false;
        IsFriendly = true;
    }

    public override void Use(Unit target)
    {
        
    }

    public override void UseThroughPauseMenu(Unit target)
    {
        
    }
}