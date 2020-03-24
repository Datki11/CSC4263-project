using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Enemy : Unit
{
	public UnityEvent killed;
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
	public override int CurrentHealth
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

	public void Awake() {
		render = GetComponent<SpriteRenderer>();
	}

	public virtual void Act() {

		//Default implementation is a completely random attack
        List<GameObject> characters = BattleManager.Instance.GetCharacters();
        int characterNum = Mathf.RoundToInt(Random.Range(0, characters.Count - 1));
        int abilityNum = Mathf.RoundToInt(Random.Range(0, Abilities.Count - 1));
        Abilities[abilityNum].Action(characters[characterNum].GetComponent<Unit>());
	}

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
