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
	public abstract int CurrentHealth
	{
		get;
		set;
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
}
