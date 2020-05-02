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
	public int ExpToLevelUp {
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

	public int Gold {
		get;
		set;
	}
	#endregion

	#region Methods
	public override void LevelUp()
    {
		Level++;
		MaxHealth += 5;
		CurrentExp -= ExpToLevelUp;
		ExpToLevelUp = Mathf.RoundToInt(ExpToLevelUp * 1.3f);
		ExpToLevelUp -= ExpToLevelUp % 5;
    }
	#endregion

	private Animator animator;

	#region MonoBehaviour
	// Start is called before the first frame update
	public override void Awake()
    {
			animator = GetComponent<Animator>();
			base.Awake();
			MaxHealth = 40;
			CurrentHealth = 40;

			Level = 1;
			CurrentExp = 0;
			ExpToLevelUp = 20;
			Attack = 5;
			Speed = 5;
			Defense = 5;
			Gold = 0;

			//For testing
			Class = new Berserker();

			Items = new Dictionary<Item, int>();
			Items.Add(new Potion(), 3);
			Items.Add(new Firecracker(), 2);
			Items.Add(new Adrenaline(), 2);
			Items.Add(new BigPotion(), 2);

			
			//Items.Add(new Firecracker(), 2);
		
    }
	public void Start() {
		Ability ability = Class.Abilities.Find(x => x.Name =="Lunge");
		ability.attackStart.AddListener(AttackAnimate);
		ability = Class.Abilities.Find(x => x.Name == "Whirlwind");
		ability.attackStart.AddListener(WhirlwindAnimate);
		ability = Class.Abilities.Find(x => x.Name == "Fury");
		ability.attackStart.AddListener(FuryAnimate);
	}

	public void TransferValues(Player player) {
		TransferValues( (Unit) player);
		Level = player.Level;
		CurrentHealth = player.CurrentHealth;
		CurrentExp = player.CurrentExp;
		ExpToLevelUp = player.ExpToLevelUp;
		Class.Abilities = player.Class.Abilities;
		Class.UnlearnedAbilities = player.Class.UnlearnedAbilities;
		Items = player.Items;
		Gold = player.Gold;
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

	private float startX;

	public void AttackAnimate() {
		startX = transform.position.x;
		GoLeft();
	}
	public void FuryAnimate() {
		animator.SetTrigger("Fury");
	}
	public void GoRight() {
		transform.Translate(5 * Time.deltaTime, 0f, 0f);
		if (transform.position.x - startX >= 0) {
			transform.position = new Vector3(startX,transform.position.y,transform.position.z);
		}
		else {
			Invoke("GoRight", 0.01f);
		}
	}
	public void GoLeft() {
		transform.Translate(-5 * Time.deltaTime, 0f, 0f);
		if (transform.position.x - startX <= -1) {
			GoRight();
			
		}
		else {
			Invoke("GoLeft", 0.01f);
		}
	}

	public void WhirlwindAnimate() {
		animator.SetTrigger("Whirlwind");
	}

    #endregion
}
