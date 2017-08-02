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
		this.tileRange = 0;
		this.cooldown = 5;
		playerUI = GetComponent<PlayerController>().playerUI;
        playerUI.GetComponentsInChildren<Button>()[3].onClick.AddListener(delegate
        {
            AttivaAbilita(SelectType.CROCE);
            activedAbility = this.abilityName;
        });

    }

}
