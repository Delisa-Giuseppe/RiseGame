using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PallaDiFuoco : Ability {

	// Use this for initialization
	void Start () 
	{
        this.abilityType = AbilityType.SINGOLO;
        abilityName = "PallaDiFuoco";
		this.damage = GetComponent<PlayerController>().magicAttack / 100f * 60f;
		this.cure = 0;
		this.tileRange = 7;
		this.cooldown = 2;
        countCooldown = this.cooldown;
		playerUI = GetComponent<PlayerController>().playerUI;
        buttonPlayerUI = playerUI.GetComponentsInChildren<Button>()[0];
        buttonPlayerUI.onClick.AddListener (delegate {
			AttivaAbilita (SelectType.CROCE);
            activedAbility = this.abilityName;
        });
	}

}
