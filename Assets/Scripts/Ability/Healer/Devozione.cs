using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Devozione : Ability {

	// Use this for initialization
	void Start () 
	{
        abilityName = "Devozione";
        this.damage = GetComponent<PlayerController>().magicAttack;
		this.cure = 0;
		this.tileRange = 1;
		this.cooldown = 5;
        countCooldown = this.cooldown;
        playerUI = GetComponent<PlayerController>().playerUI;
        buttonPlayerUI = playerUI.GetComponentsInChildren<Button>()[1];
        buttonPlayerUI.onClick.AddListener(delegate {
			TileManager.ResetGrid ();
			StartCoroutine (SelectPlayers(1f));
            activedAbility = this.abilityName;
        });
    }

	public override void UsaAbilita()
	{
		RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
		if (hit.collider != null && hit.collider.tag == "Player" ||
		    (hit.collider != null && hit.collider.tag == "Tile")) {
	
			GameObject playerTarget = null;
			foreach (GameObject player in TileManager.playerDead)
			{
				if (player.GetComponent<PlayerController>().PlayerTile.transform.position == hit.collider.transform.position)
				{
					playerTarget = player;
					break;
				}
			}

			if (playerTarget)
			{
				
				playerTarget.GetComponent<PlayerController> ().ResurrectPlayer ();

				TileManager.playerDead.Remove (playerTarget);
				TurnManager.turns.Add (playerTarget);

				GetComponent<PlayerController>().PhysicAttack(playerTarget, "attack", -playerTarget.GetComponent<PlayerController>().totalHealth / 2);

				AddAbilityToCooldownList(this);

				StartCoroutine(TileManager.WaitMoves(this.gameObject, GameManager.States.END_MOVE));
			}


		}
	}

	IEnumerator SelectPlayers(float delay)
	{
		GameManager.currentState = GameManager.States.ABILITY;
		yield return new WaitForSeconds(delay);

		foreach (GameObject player in TileManager.playerDead)
		{
			TileManager.tilesSelectable.Add(player.GetComponent<PlayerController>().PlayerTile);
			player.GetComponent<PlayerController>().PlayerTile.GetComponent<SpriteRenderer>().color = Color.green;
		}
	}


}
