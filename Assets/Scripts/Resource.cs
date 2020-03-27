using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Resource : MonoBehaviour
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
