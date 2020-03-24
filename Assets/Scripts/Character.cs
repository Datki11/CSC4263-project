using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : Unit
{

    #region Public Properties
    public abstract ICharClass Class
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
