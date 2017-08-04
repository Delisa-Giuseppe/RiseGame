using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Elementale : Ability {

	// Use this for initialization
	void Start () 
	{
        abilityName = "Elementale";
        this.damage = 0;
		this.cure = 0;
		this.tileRange = GetComponent<PlayerController>().moves;
		this.cooldown = 7;
		playerUI = GetComponent<PlayerController>().playerUI;
		playerUI.GetComponentsInChildren<Button> () [1].onClick.AddListener (delegate {
			AttivaAbilita (SelectType.QUADRATO);
            activedAbility = this.abilityName;
        });
	}

}
