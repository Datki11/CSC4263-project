﻿using System.Collections;
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

	public
	#endregion

	#region MonoBehaviour
	// Start is called before the first frame update
	void Awake()
    {
			MaxHealth = 40;
			CurrentHealth = 20;

			Level = 1;
			CurrentExp = 0;
			Attack = 8;
			Speed = 3;
			Defense = 6;

			//For testing
			Class = new Berserker();

			Items = new Dictionary<Item, int>();
			Items.Add(new Potion(), 3);
			Items.Add(new Firecracker(), 2);
			//Items.Add(new Firecracker(), 2);
		
    }

	public void TransferValues(Player player) {
		TransferValues( (Unit) player);
		CurrentHealth = player.CurrentHealth;
		Class.Abilities = player.Class.Abilities;
		Items = player.Items;
	}

	public void AddItem (Item item) {
		List<Item> itemKeys = new List<Item>(Items.Keys);
		Item itemInInventory = itemKeys.Find(x => x.Name == item.Name);
		if (itemInInventory != null) {
			Items[itemInInventory] += 1;
		}
		else {
			Items.Add(item, 1);
		}
	}

    // Update is called once per frame
	void Update() {
	}
    #endregion
}
