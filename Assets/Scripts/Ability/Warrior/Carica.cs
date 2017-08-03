using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Carica : Ability {

    // Use this for initialization
    void Start () 
	{
        this.abilityType = AbilityType.MOVIMENTO;
		this.abilityName = "Carica";
		this.damage = GetComponent<PlayerController>().physicAttack / 100f * 70f;
		this.cure = 0;
		this.tileRange = 8;
		this.cooldown = 4;
        countCooldown = this.cooldown;
        playerUI = GetComponent<PlayerController>().playerUI;
        buttonPlayerUI = playerUI.GetComponentsInChildren<Button>()[0];
        buttonPlayerUI.onClick.AddListener (delegate {
			AttivaAbilita (SelectType.CROCE);
            activedAbility = this.abilityName;
        });
	}

}
