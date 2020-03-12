using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Player : Character
{
	#region Public Properties
	public override ICharClass Class
	{
		get;
		set;
	}
	public override int MaxHealth
	{
		get;
		set;
	}
	public override int CurrentHealth
	{
		get;
		set;
	}
	public override int Level
	{
		get;
		set;
	}
	public override int CurrentExp
	{
		get;
		set;
	}

	//possibly wrap below stats, maybe health too, in a Stats class
	public override int Attack
	{
		get;
		set;
	}
	public override int Defense
	{
		get;
		set;
	}
	public override int Speed
	{
		get;
		set;
	}
	#endregion

	#region Methods
	public override void LevelUp()
    {
		base.LevelUp();
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
