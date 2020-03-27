using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rage : Resource
{
    public override int BaseMax
    {
        get;
        set;
    }

    public override int Current
    {
        get;
        set;
    }

    public override int BaseRestoreRate
    {
        get;
        set;
    }
}