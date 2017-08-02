using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CatenaDiFolgore : Ability {

	// Use this for initialization
	void Start ()
	{
        abilityName = "CatenaDiFolgore";
        this.damage = GetComponent<PlayerController>().magicAttack / 100f * 15f;
		this.cure = 0;
		this.tileRange = 2;
		this.cooldown = 4;
		playerUI = GetComponent<PlayerController>().playerUI;
		playerUI.GetComponentsInChildren<Button> () [2].onClick.AddListener (delegate {
			AttivaAbilita (SelectType.ROMBO);
            activedAbility = this.abilityName;
        });
		
	}

}
