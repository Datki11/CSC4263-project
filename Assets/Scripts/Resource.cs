using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Resource
{

    public abstract int BaseMax
    {
        get;
        set;
    }

    public abstract int Current
    {
        get;
        set;
    }

    public abstract int BaseRestoreRate
    {
        get;
        set;
    }
}
