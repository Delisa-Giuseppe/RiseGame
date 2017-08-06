using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PassoDiOmbra : Ability {

	// Use this for initialization
	void Start () 
	{
        this.abilityName = "PassoDiOmbra";
        //this.abilityType = AbilityType.TELETRASPORTO;
        this.damage = 0;
		this.cure = 0;
		this.tileRange = 3;
		this.cooldown = 4;
        countCooldown = this.cooldown;
        playerUI = GetComponent<PlayerController>().playerUI;
        buttonPlayerUI = playerUI.GetComponentsInChildren<Button>()[1];
        buttonPlayerUI.onClick.AddListener(delegate {
            AttivaAbilita(SelectType.ROMBO);
            activedAbility = this.abilityName;
        });
	}

	public override void UsaAbilita()
	{
		//GameObject playerTarget = null;
		RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
		if (hit.collider != null && hit.collider.tag == "Tile" && hit.collider.GetComponent<Tile>().isSelected
			&& !hit.collider.GetComponent<Tile>().isEnemy && !hit.collider.GetComponent<Tile>().isPlayer) {
			//playerTarget.transform.position = hit.collider.gameObject.transform.position;
			GetComponent<AILerp>().enabled = false;
			this.gameObject.transform.position = hit.collider.gameObject.transform.position;

			GetComponent<PlayerController>().PlayerTile = hit.collider.gameObject;


			GetComponent<AILerp> ().target = hit.collider.gameObject.transform;
			GetComponent<AILerp> ().targetReached = true;
			GetComponent<AILerp> ().enabled = true;


			AddAbilityToCooldownList(this);


			StartCoroutine(TileManager.WaitMoves(this.gameObject, GameManager.States.END_MOVE));
		}
	}
}
