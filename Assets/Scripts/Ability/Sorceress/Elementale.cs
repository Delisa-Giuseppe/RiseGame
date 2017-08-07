using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Elementale : Ability {

	private int currentMind;
	private int currentConstitution;

	// Use this for initialization
	void Start ()
	{
		abilityName = "Elementale";
        this.abilityDescription = "La maga scatena il proprio potere per 3 Turni, guadagnando 5 punti intelligenza e 2 punti costituzione";
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

	public override void UsaAbilita()
	{
        GetComponent<PlayerController>().gameObject.transform.GetChild(0).gameObject.SetActive(false);
        GetComponent<PlayerController>().gameObject.transform.GetChild(1).gameObject.SetActive(true);
        GetComponent<PlayerController>().StartFightAnimation();

        GetComponent<PlayerController> ().constitution += 2; 
		GetComponent<PlayerController> ().mind += 5;
		GetComponent<PlayerController> ().CalculateStatistics ();
        this.tileRange = GetComponent<PlayerController>().moves;

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
        this.tileRange = GetComponent<PlayerController>().moves;
        GetComponent<PlayerController>().gameObject.transform.GetChild(0).gameObject.SetActive(true);
        GetComponent<PlayerController>().gameObject.transform.GetChild(1).gameObject.SetActive(false);
    }


}
