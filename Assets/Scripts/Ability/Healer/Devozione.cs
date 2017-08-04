using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Devozione : Ability {

	// Use this for initialization
	void Start () 
	{
        abilityName = "Devozione";
        this.damage = GetComponent<PlayerController>().magicAttack;
		this.cure = 0;
		this.tileRange = 1;
		this.cooldown = 5;
        countCooldown = this.cooldown;
        playerUI = GetComponent<PlayerController>().playerUI;
        buttonPlayerUI = playerUI.GetComponentsInChildren<Button>()[1];
        buttonPlayerUI.onClick.AddListener(delegate {
            AttivaAbilita(SelectType.CROCE);
            activedAbility = this.abilityName;
        });
    }

}
