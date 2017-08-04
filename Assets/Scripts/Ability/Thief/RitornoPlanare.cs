using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RitornoPlanare : Ability {

	// Use this for initialization
	void Start ()
	{
		this.abilityName = "RitornoPlanare";
		this.damage = 0;
		this.cure = 0;
		this.tileRange = 2;
		this.cooldown = 6;
        countCooldown = this.cooldown;
        playerUI = GetComponent<PlayerController>().playerUI;
        buttonPlayerUI = playerUI.GetComponentsInChildren<Button>()[2];
        buttonPlayerUI.onClick.AddListener(delegate {
            AttivaAbilita(SelectType.ROMBO);
            activedAbility = this.abilityName;
        });
	}

}
