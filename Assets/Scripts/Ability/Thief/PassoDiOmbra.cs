using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PassoDiOmbra : Ability {

	// Use this for initialization
	void Start () 
	{
        this.abilityName = "PassoDiOmbra";
        this.abilityType = AbilityType.TELETRASPORTO;
        this.damage = 0;
		this.cure = 0;
		this.tileRange = 3;
		this.cooldown = 4;
        countCooldown = this.cooldown;
        playerUI = GetComponent<PlayerController>().playerUI;
        buttonPlayerUI = playerUI.GetComponentsInChildren<Button>()[1];
        buttonPlayerUI.onClick.AddListener(delegate {
            AttivaAbilita(SelectType.ROMBO);
            activedAbility = this.abilityName;
        });
	}
}
