using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CostrizioneCurativa : Ability {

	// Use this for initialization
	void Start () 
	{
		this.damage = GetComponent<PlayerController>().magicAttack / 100f * 50f;
		this.cure = GetComponent<PlayerController>().magicAttack / 100f * 50f;
		this.tileRange = 2;
		this.cooldown = 3;
		playerUI = GetComponent<PlayerController>().playerUI;
		playerUI.GetComponentsInChildren<Button> () [0].onClick.AddListener (delegate {
			AttivaAbilita (SelectType.ROMBO);
		});
		
	}

}
