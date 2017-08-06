using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AttaccoRotante : Ability {

	// Use this for initialization
	void Start () 
	{
		this.abilityName = "AttaccoRotante";
        activedAbility = this.abilityName;
        this.damage = GetComponent<PlayerController>().physicAttack / 100f * 50f;
		this.cure = (5f + GetComponent<PlayerController>().magicAttack / 100f * 20f) * 4;
		this.tileRange = 2;
		this.cooldown = 5;
		countCooldown = this.cooldown;
        playerUI = GetComponent<PlayerController>().playerUI;
        buttonPlayerUI = playerUI.GetComponentsInChildren<Button>()[1];
        buttonPlayerUI.onClick.AddListener(delegate
        {
			GameManager.currentState = GameManager.States.ABILITY;
			activedAbility = this.abilityName;
            AttivaAbilita(SelectType.QUADRATO);
			StartCoroutine (SelectPlayers (1.5f));
        });
	}

	IEnumerator SelectPlayers(float delay)
	{
		yield return new WaitForSeconds(delay);
		UsaAbilita ();
	}

	public override void UsaAbilita()
	{
		if (TileManager.CheckEnemy ()) {
			List<GameObject> enemyTargets = new List<GameObject> ();

			foreach (GameObject tileEnemy in TileManager.tilesSelectable) {
				if (tileEnemy.GetComponent<Tile> ().isEnemy) {
					foreach (GameObject enemy in TileManager.enemyInstance) {
						if (enemy.GetComponent<EnemyController> ().EnemyTile.transform.position == tileEnemy.transform.position) {
							enemyTargets.Add (enemy);
						}
					}
				}

			}

			GetComponent<PlayerController> ().PhysicAttack (enemyTargets, "stomp", (int)this.damage);

			AddAbilityToCooldownList (this);

			StartCoroutine (TileManager.WaitMoves (this.gameObject, GameManager.States.END_MOVE));
		}


	}

}
