using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public abstract int MaxHealth
	{
		get;
		set;
	}
	public abstract int CurrentHealth
	{
		get;
		set;
	}
    public abstract int Attack
	{
		get;
		set;
	}
	public abstract int Defense
	{
		get;
		set;
	}
	public abstract int Speed
	{
		get;
		set;
	}
}
