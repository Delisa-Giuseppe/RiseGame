using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CostrizioneCurativa : Ability {

    // Use this for initialization
    void Start () 
	{
        abilityName = "CostrizioneCurativa";
        this.damage = GetComponent<PlayerController>().magicAttack / 100f * 50f;
		this.cure = GetComponent<PlayerController>().magicAttack / 100f * 50f;
		this.tileRange = 2;
		this.cooldown = 3;
        countCooldown = this.cooldown;
        playerUI = GetComponent<PlayerController>().playerUI;
        buttonPlayerUI = playerUI.GetComponentsInChildren<Button>()[0];
        buttonPlayerUI.onClick.AddListener (delegate {
			AttivaAbilita (SelectType.ROMBO);
            activedAbility = this.abilityName;
        });
		
	}

    public override void UsaAbilita()
    {
        if (TileManager.CheckEnemy())
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.tag == "Enemy" ||
                    (hit.collider != null && hit.collider.tag == "Tile" && hit.collider.GetComponent<Tile>().isSelected && hit.collider.GetComponent<Tile>().isEnemy))
            {

                GameObject enemyTarget = null;
                foreach (GameObject enemy in TileManager.enemyInstance)
                {
                    if (enemy.GetComponent<EnemyController>().EnemyTile.transform.position == hit.collider.transform.position)
                    {
                        enemyTarget = enemy;
                        break;
                    }
                }
                GetComponent<PlayerController>().PhysicAttack(enemyTarget, "attack", (int)this.damage);
                TileManager.ResetGrid();
                StartCoroutine(SelectPlayers(2f));
            }
        }
        else
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.tag == "Player" ||
                    (hit.collider != null && hit.collider.tag == "Tile" && hit.collider.GetComponent<Tile>().isPlayer))
            {
                GameObject playerTarget = null;
                foreach (GameObject player in TileManager.playerInstance)
                {
                    if (player.GetComponent<PlayerController>().PlayerTile.transform.position == hit.collider.transform.position)
                    {
                        playerTarget = player;
                        break;
                    }
                }
                GetComponent<PlayerController>().PhysicAttack(playerTarget, "attack", -(int)this.damage);


                AddAbilityToCooldownList();

                StartCoroutine(TileManager.WaitMoves(this.gameObject, GameManager.States.END_MOVE));

            }
        }
                
            
    }

    IEnumerator SelectPlayers(float delay)
    {
        yield return new WaitForSeconds(delay);
        AttivaAbilita(SelectType.COMPAGNI);
    }

}
