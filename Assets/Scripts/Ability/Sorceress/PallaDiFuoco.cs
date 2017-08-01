using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PallaDiFuoco : Ability {

	// Use this for initialization
	void Start () 
	{
		this.damage = GetComponent<PlayerController>().magicAttack / 100f * 60f;
		this.cure = 0;
		this.tileRange = 7;
		this.cooldown = 2;
		playerUI = GetComponent<PlayerController>().playerUI;
		playerUI.GetComponentsInChildren<Button> () [0].onClick.AddListener (delegate {
			AttivaAbilita (SelectType.CROCE);
		});
	}

}
