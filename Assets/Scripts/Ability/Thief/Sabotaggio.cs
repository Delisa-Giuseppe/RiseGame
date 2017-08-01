using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Sabotaggio : Ability {

	// Use this for initialization
	void Start () 
	{
		this.abilityName = "Sabotaggio";
		this.damage = 0;
		this.cure = 0;
		this.tileRange = 4;
		this.cooldown = 5;
		playerUI = GetComponent<PlayerController>().playerUI;
		playerUI.GetComponentsInChildren<Button> () [3].onClick.AddListener (delegate {
			AttivaAbilita (SelectType.ROMBO);
		});
	}

}
