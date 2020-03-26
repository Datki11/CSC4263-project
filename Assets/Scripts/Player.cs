using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
	#region Public Properties
	
	public Dictionary<Item, int> Items {get; set;}
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
	void Awake()
    {
			MaxHealth = 20;
			CurrentHealth = 20;
			//For testing
			Class = new Berserker();
			Class.Abilities = new List<Ability>();
			Class.Abilities.Add(new Lunge());
			Class.Abilities.Add(new Whirlwind());
			Class.Abilities.Add(new FocusEnergy());

			Items = new Dictionary<Item, int>();
			Items.Add(new Potion(), 3);
			Items.Add(new Firecracker(), 2);
		
    }

	public void TransferValues(Player player) {
		TransferValues( (Unit) player);
		Class.Abilities = player.Class.Abilities;
		Items = player.Items;
	}

    // Update is called once per frame
    void Update()
    {
    }
    #endregion
}
