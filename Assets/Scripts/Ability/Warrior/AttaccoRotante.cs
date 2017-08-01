using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AttaccoRotante : Ability {

	// Use this for initialization
	void Start () 
	{
		this.abilityName = "Attacco Rotante";
		this.damage = GetComponent<PlayerController>().physicAttack / 100f * 50f;
		this.cure = (5f + GetComponent<PlayerController>().magicAttack / 100f * 20f) * 4;
		this.tileRange = 2;
		this.cooldown = 5;
		playerUI = GetComponent<PlayerController>().playerUI;
		playerUI.GetComponentsInChildren<Button>()[3].onClick.AddListener(delegate { AttivaAbilita(SelectType.QUADRATO); });

	}

}
