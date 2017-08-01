using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AttaccoPotenziato : Ability {

    

	// Use this for initialization
	void Start ()
    {
        this.abilityName = "Attacco Potenziato";
        this.damage = GetComponent<PlayerController>().physicAttack / 100f * 125f;
        this.cure = 0;
        this.tileRange = 1;
        this.cooldown = 4;
		playerUI = GetComponent<PlayerController>().playerUI;
		playerUI.GetComponentsInChildren<Button>()[2].onClick.AddListener(delegate { AttivaAbilita(SelectType.ROMBO); });

    }

    void UsaAbilita()
    {

    }
}
