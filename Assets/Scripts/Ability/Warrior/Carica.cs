using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Carica : Ability {

	// Use this for initialization
	void Start () 
	{
		this.abilityName = "Carica";
		this.damage = GetComponent<PlayerController>().physicAttack / 100f * 70f;
		this.cure = 0;
		this.tileRange = GetComponent<PlayerController>().moves * 2;
		this.cooldown = 4;
		playerUI = GetComponent<PlayerController>().playerUI;
		playerUI.GetComponentsInChildren<Button> () [0].onClick.AddListener (delegate {
			AttivaAbilita (SelectType.CROCE);
		});
	}

}
