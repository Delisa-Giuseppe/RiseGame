using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Elementale : Ability {

	private int currentMind;
	private int currentConstitution;

	// Use this for initialization
	void Start ()
	{
		abilityName = "Elementale";
		currentMind = GetComponent<PlayerController> ().mind; 
		currentConstitution = GetComponent<PlayerController> ().constitution; 
		this.damage = 0;
		this.cure = 0;
		this.tileRange = GetComponent<PlayerController> ().moves;
		this.cooldown = 4;
		countCooldown = this.cooldown;
		this.turnDuration = 3;
		countTurnDuration = this.turnDuration;

		playerUI = GetComponent<PlayerController>().playerUI;
		buttonPlayerUI = playerUI.GetComponentsInChildren<Button>()[1];
		buttonPlayerUI.onClick.AddListener(delegate {
			TileManager.ResetGrid ();
			StartCoroutine (Wait(0.5f));
			activedAbility = this.abilityName;


		});
	}

	public override void UsaAbilita()
	{
		

		GetComponent<PlayerController> ().constitution += 2; 
		GetComponent<PlayerController> ().mind += 5;
		GetComponent<PlayerController> ().CalculateStatistics ();

		AddAbilityToTurnDurationList(this);

		StartCoroutine(TileManager.WaitMoves(this.gameObject, GameManager.States.END_MOVE));

	}

	IEnumerator Wait(float delay)
	{
		GameManager.currentState = GameManager.States.ABILITY;
		yield return new WaitForSeconds(delay);

		TileManager.tilesSelectable.Add(GetComponent<PlayerController>().PlayerTile);
		GetComponent<PlayerController>().PlayerTile.GetComponent<SpriteRenderer>().color = Color.green;

		UsaAbilita ();
	}

	public override void ResettaValori()
	{
		AddAbilityToCooldownList (this);
		countTurnDuration = turnDuration;
		turnDurationList.Remove(this);
		PlayerController controller = GetComponent <PlayerController> ();
		controller.mind = currentMind;
		controller.constitution = currentConstitution;
		controller.CalculateStatistics ();
	}


}
