using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ICharClass
{

    #region Public Properties
    public abstract List<Ability> Abilities
    {
		get;
		set;
    }
	public abstract int InitialHealth
    {
		get;
		set;
    }

	//initial values for stats
	public abstract int InitialAttack
	{
		get;
		set;
	}
	public abstract int InitialDefense
	{
		get;
		set;
	}
	public abstract int InitialSpeed
	{
		get;
		set;
	}

	//values to be added to stats on level-up
	public abstract int HealthIncrement
	{
		get;
		set;
	}
	public abstract int AttackIncrement
	{
		get;
		set;
	}
	public abstract int DefenseIncrement
	{
		get;
		set;
	}
	public abstract int SpeedIncrement
	{
		get;
		set;
	}
	#endregion
}
