using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PallaDiFuoco : Ability {

	// Use this for initialization
	void Start () 
	{
        this.abilityType = AbilityType.SINGOLO;
        this.abilityName = "PallaDiFuoco";
        this.abilityDescription = "Colpisce tutti i nemici entro 1 casella dal guerriero e li stordisce per 1 turno";
        this.damage = GetComponent<PlayerController>().magicAttack / 100f * 60f;
		this.cure = 0;
		this.tileRange = 8;
		this.cooldown = 1;
        countCooldown = this.cooldown;
		playerUI = GetComponent<PlayerController>().playerUI;
        buttonPlayerUI = playerUI.GetComponentsInChildren<Button>()[0];
        buttonPlayerUI.onClick.AddListener (delegate {
			AttivaAbilita (SelectType.CROCE);
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

}
