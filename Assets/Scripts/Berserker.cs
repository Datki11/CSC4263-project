using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berserker : ICharClass
{

	#region Public Properties
	public override List<Ability> Abilities
	{
		get;
		set;
    }

	public override Resource Resource
	{
		get;
		set;
	}

	public override int InitialHealth
    {
		get;
		set;
    }

	public override int InitialAttack
    {
		get;
		set;
    }
	public override int InitialDefense
    {
		get;
		set;
    }
	public override int InitialSpeed
    {
		get;
		set;
    }

	public override int HealthIncrement
    {
		get;
		set;
    }
	public override int AttackIncrement
    {
		get;
		set;
    }
	public override int DefenseIncrement
    {
		get;
		set;
    }
	public override int SpeedIncrement
    {
		get;
		set;
    }
    #endregion

    #region Constructor
    public Berserker()
	{
		Abilities = new List<Ability>
		{
			new Lunge(),
			new Whirlwind(),
			new FocusEnergy(),
			new FocusPower(),
			new Stoneskin(),
			new Unwind()
		};

		Resource = new Rage();

		// InitialHealth = ;
		// InitialAttack = ;
		// InitialDefense = ;
		// InitialSpeed = ;

		// HealthIncrement = ;
		// AttackIncrement = ;
		// DefenseIncrement = ;
		// SpeedIncrement = ;
	}
	#endregion
}
