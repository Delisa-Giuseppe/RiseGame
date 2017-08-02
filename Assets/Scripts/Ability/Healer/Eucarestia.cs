using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Eucarestia : Ability {

	// Use this for initialization
	void Start () 
	{
		this.abilityName = "Eucarestia";
		this.damage = 0;
		this.cure = GetComponent<PlayerController>().currentHealth / 100f * 30f;
		this.tileRange = 2;
		this.cooldown = 4;
		playerUI = GetComponent<PlayerController>().playerUI;
        playerUI.GetComponentsInChildren<Button>()[1].onClick.AddListener(delegate
        {
            AttivaAbilita(SelectType.CROCE);
            activedAbility = this.abilityName;
        });

    }

}
