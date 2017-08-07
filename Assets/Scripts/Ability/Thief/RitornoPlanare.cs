﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RitornoPlanare : Ability {

	private int currentMoves;

	// Use this for initialization
	void Start ()
	{
		this.abilityName = "RitornoPlanare";
        this.abilityDescription = "Per 3 turni il ladro guadagna invisibilità e 2 caselle movimento in più e non può essere colpito; attaccando un bersaglio esce dallo stato di immunità ed invisibilità";
        currentMoves = GetComponent<PlayerController> ().moves; 
		this.damage = 0;
		this.cure = 0;
		this.tileRange = 2;
		this.cooldown = 4;
        countCooldown = this.cooldown;
		this.turnDuration = 2;
		countTurnDuration = this.turnDuration;
        playerUI = GetComponent<PlayerController>().playerUI;
        buttonPlayerUI = playerUI.GetComponentsInChildren<Button>()[2];
        buttonPlayerUI.onClick.AddListener(delegate {
			TileManager.ResetGrid ();
			StartCoroutine (Wait(0.5f));
			activedAbility = this.abilityName;
        });
        EventTrigger trigger = buttonPlayerUI.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((data) => { OnPointerEnterDelegate((PointerEventData)data); });
        trigger.triggers.Add(entry);
        EventTrigger.Entry exit = new EventTrigger.Entry();
        exit.eventID = EventTriggerType.PointerExit;
        exit.callback.AddListener((data) => { OnPointerExitDelegate((PointerEventData)data); });
        trigger.triggers.Add(exit);


    }

	IEnumerator Wait(float delay)
	{
		GameManager.currentState = GameManager.States.ABILITY;
		yield return new WaitForSeconds(delay);

		TileManager.tilesSelectable.Add(GetComponent<PlayerController>().PlayerTile);
		GetComponent<PlayerController>().PlayerTile.GetComponent<SpriteRenderer>().color = Color.green;

		UsaAbilita ();
	}

	public override void UsaAbilita()
	{
        GetComponent<PlayerController>().SetTransparency();
        GetComponent<PlayerController> ().moves += 2; 
		AddAbilityToTurnDurationList(this);
		StartCoroutine(TileManager.WaitMoves(this.gameObject, GameManager.States.END_MOVE));
	}

	public override void ResettaValori()
	{
        GetComponent<PlayerController>().DeleteTransparency();
        AddAbilityToCooldownList (this);
		countTurnDuration = turnDuration;
		turnDurationList.Remove(this);
		PlayerController controller = GetComponent <PlayerController> ();
		controller.moves = currentMoves;
		controller.CalculateStatistics ();
	}

}
