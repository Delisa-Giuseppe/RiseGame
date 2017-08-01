using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PioggiaDiFuoco : Ability {

	// Use this for initialization
	void Start () 
	{
		this.damage = GetComponent<PlayerController>().magicAttack / 100f * 30f;
		this.cure = 0;
		this.tileRange = 2;
		this.cooldown = 5;
		playerUI = GetComponent<PlayerController>().playerUI;
		playerUI.GetComponentsInChildren<Button> () [1].onClick.AddListener (delegate {
			AttivaAbilita (SelectType.ROMBO);
		});
	}

	public void UsaAbilita()
	{
		print ("Pioggia");
	}

}
