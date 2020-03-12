using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class Character : MonoBehaviour
{

    #region Public Properties
    public abstract ICharClass Class
	{
		get;
		set;
	}
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
	public abstract int Level
	{
		get;
		set;
	}
	public abstract int CurrentExp
	{
		get;
		set;
	}

	//possibly wrap below stats, maybe health too, in a Stats class
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
    #endregion

    #region Methods
    public virtual void LevelUp()
	{

		Level++;
		CurrentHealth = MaxHealth;
		Attack += Class.AttackIncrement;
		Defense += Class.DefenseIncrement;
		Speed += Class.SpeedIncrement;
	}
	#endregion

	#region MonoBehaviour
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	#endregion
}
