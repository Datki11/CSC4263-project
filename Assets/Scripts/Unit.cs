using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Unit : MonoBehaviour
{
    public abstract int MaxHealth
	{
		get;
		set;
	}
	private int _health;
	public virtual int CurrentHealth
	{
		get {
			return _health;
		}
		set {
			_health = value;
			if (_health < 0)
				_health = 0;
			if (_health > MaxHealth)
				_health = MaxHealth;
		}
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
	public UnityEvent killed;

	public void Kill() {
		KillAnimate();
	}
	private void KillAnimate() {
		float alpha = GetComponent<SpriteRenderer>().color.a - 2 * Time.deltaTime;
		GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f, alpha);
		if (alpha > 0) {
			Invoke("KillAnimate",0.01f);
		}
		else {
			gameObject.tag = "dead";
			killed.Invoke();
			Destroy(gameObject);
		}
	}

	public virtual void TransferValues(Unit unit) {
		MaxHealth = unit.MaxHealth;
		CurrentHealth = unit.CurrentHealth;
		Attack = unit.Attack;
		Defense = unit.Defense;
		Speed = unit.Speed;

	}
}
