using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Enemy : Unit
{
	public List<Ability> Abilities {
		get;
		set;
	}
	private SpriteRenderer render;
    public override int MaxHealth
	{
		get;
		set;
	}
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
	private int _exp = 5;
	public int Exp {
		get {return _exp;}
		protected set {_exp =  value;}
	}
	private Vector2 _goldRange = new Vector2(2,5);
	public virtual Vector2 GoldRange {
		get {return _goldRange;}
		protected set {_goldRange = value;}
	}


	private float startX;
	public override void Awake() {
		base.Awake();
		statusEffects = new List<StatusEffect>();
	}
	public virtual void Act() {

		//Default implementation is a completely random attack
        List<GameObject> characters = BattleManager.Instance.GetCharacters();
        int characterNum = Mathf.RoundToInt(Random.Range(0, characters.Count));
        int abilityNum = Mathf.RoundToInt(Random.Range(0, Abilities.Count));
        Abilities[abilityNum].Action(characters[characterNum].GetComponent<Unit>(), this);
		AttackAnimate();
	}
	private void AttackAnimate() {
		startX = transform.position.x;
		GoRight();
	}
	private void GoRight() {
		transform.Translate(5 * Time.deltaTime, 0f, 0f);
		if (transform.position.x - startX >= 1) {
			GoLeft();
		}
		else {
			Invoke("GoRight", 0.01f);
		}
	}
	private void GoLeft() {
		transform.Translate(-5 * Time.deltaTime, 0f, 0f);
		if (transform.position.x - startX <= 0) {
			transform.position = new Vector3(startX,transform.position.y,transform.position.z);
		}
		else {
			Invoke("GoLeft", 0.01f);
		}
	}
}
