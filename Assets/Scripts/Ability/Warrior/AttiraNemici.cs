using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AttiraNemici : Ability {

	// Use this for initialization
	void Start () 
	{
		this.abilityName = "AttiraNemici";
		this.damage = 0;
		this.cure = 0;
		this.tileRange = 2;
		this.cooldown = 4;
		playerUI = GetComponent<PlayerController>().playerUI;
		playerUI.GetComponentsInChildren<Button>()[1].onClick.AddListener(delegate { AttivaAbilita(SelectType.ROMBO); });

	}

}
