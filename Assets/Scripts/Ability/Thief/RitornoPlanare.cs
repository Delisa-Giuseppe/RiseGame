using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RitornoPlanare : Ability {

	// Use this for initialization
	void Start ()
	{
		this.abilityName = "Ritorno Planare";
		this.damage = 0;
		this.cure = 0;
		this.tileRange = 2;
		this.cooldown = 6;
		playerUI = GetComponent<PlayerController>().playerUI;
//		playerUI.GetComponentsInChildren<Button> () [2].onClick.AddListener (delegate {
//			AttivaAbilita (SelectType.CROCE);
//		});
	}

}
