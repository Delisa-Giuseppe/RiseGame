using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CenereAlVento : Ability {

	// Use this for initialization
	void Start () 
	{
		this.damage = 0;
		this.cure = 0;
		this.tileRange = 3;
		this.cooldown = 6;
		playerUI = GetComponent<PlayerController>().playerUI;
		playerUI.GetComponentsInChildren<Button> () [2].onClick.AddListener (delegate {
			AttivaAbilita (SelectType.QUADRATO);
		});
	}

}
