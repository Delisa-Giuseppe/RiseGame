using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability : MonoBehaviour
{

    public string abilityName;
    public float damage;
    public float cure;
    public int tileRange;
    public int turnDuration;
    public int cooldown;
    public int countCooldown;
    public static string activedAbility;
    public AbilityType abilityType;
    public static List<Ability> cooldownList; 

    private GameObject UI;
	protected GameObject playerUI;
    protected Button buttonPlayerUI;

    public enum SelectType
    {
        QUADRATO,
        ROMBO,
        CROCE,
        COMPAGNI
    }

    public enum AbilityType
    {
        SINGOLO,
        MULTIPLO,
        MOVIMENTO,
        TELETRASPORTO
    }

    // Use this for initialization
    void Awake()
    {
        UI = GameObject.Find("UI");
        abilityName = "";
        damage = 0;
        cure = 0;
        tileRange = 0;
        turnDuration = 0;
		cooldown = 0;
        countCooldown = 0;
        activedAbility = "";
        cooldownList = new List<Ability>();

    }

	void Update()
	{
		if (GameManager.currentState == GameManager.States.ABILITY && Input.GetMouseButtonDown(1))
		{
			TileManager.ResetGrid();
			GameManager.currentState = GameManager.States.SELECT;
			TurnManager.currentTurnState = TurnManager.TurnStates.EXECUTE;
		}
	}

    protected Vector2[] CalcolaSelezioneQuadrata()
    {
        Vector2[] points = new Vector2[] {
            new Vector2(
                    TileManager.quadInitialPoint[0].x*tileRange,
                    TileManager.quadInitialPoint[0].y*tileRange),
                new Vector2(
                    TileManager.quadInitialPoint[1].x*tileRange,
                    TileManager.quadInitialPoint[1].y*tileRange),
                new Vector2(
                    TileManager.quadInitialPoint[2].x*tileRange,
                    TileManager.quadInitialPoint[2].y*tileRange),
                new Vector2(
                    TileManager.quadInitialPoint[3].x*tileRange,
                    TileManager.quadInitialPoint[3].y*tileRange)
        };

        return points;

    }

    protected Vector2[] CalcolaSelezioneRomboidale()
    {
        Vector2[] points = new Vector2[] {
            new Vector2(
                    TileManager.polygonInitialPoint[0].x*tileRange,
                    TileManager.polygonInitialPoint[0].y*tileRange),
                new Vector2(
                    TileManager.polygonInitialPoint[1].x*tileRange,
                    TileManager.polygonInitialPoint[1].y*tileRange),
                new Vector2(
                    TileManager.polygonInitialPoint[2].x*tileRange,
                    TileManager.polygonInitialPoint[2].y*tileRange),
                new Vector2(
                    TileManager.polygonInitialPoint[3].x*tileRange,
                    TileManager.polygonInitialPoint[3].y*tileRange)
        };

        return points;

    }

    protected Vector2[] CalcolaSelezioneACroce()
    {
        Vector2[] points = new Vector2[] {
            new Vector2(
                    TileManager.quadInitialPoint[0].x*tileRange,
                    TileManager.quadInitialPoint[0].y),
                new Vector2(
                    TileManager.quadInitialPoint[1].x*tileRange,
                    TileManager.quadInitialPoint[1].y),
                new Vector2(
                    TileManager.quadInitialPoint[2].x*tileRange,
                    TileManager.quadInitialPoint[2].y),
                new Vector2(
                    TileManager.quadInitialPoint[3].x*tileRange,
                    TileManager.quadInitialPoint[3].y),
                new Vector2(
                    TileManager.quadInitialPoint[0].x,
                    TileManager.quadInitialPoint[0].y*tileRange),
                new Vector2(
                    TileManager.quadInitialPoint[1].x,
                    TileManager.quadInitialPoint[1].y*tileRange),
                new Vector2(
                    TileManager.quadInitialPoint[2].x,
                    TileManager.quadInitialPoint[2].y*tileRange),
                new Vector2(
                    TileManager.quadInitialPoint[3].x,
                    TileManager.quadInitialPoint[3].y*tileRange)
        };

        return points;

    }

    protected void CalcolaSelezioneCompagni()
    {
        foreach (GameObject player in TileManager.playerInstance)
        {
            TileManager.tilesSelectable.Add(player.GetComponent<PlayerController>().PlayerTile);
            player.GetComponent<PlayerController>().PlayerTile.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

    public void AttivaAbilita(SelectType currentType)
    {
        Vector2[] newPoints = null;
        switch (currentType)
        {
            case SelectType.QUADRATO:
                newPoints = CalcolaSelezioneQuadrata();
                TileManager.ResetGrid();
                TileManager.SetTrigger(this.GetComponent<PlayerController>().PlayerTile, newPoints);
                StartCoroutine(TileManager.WaitMovesAbility(this.gameObject));
                break;
            case SelectType.ROMBO:
                newPoints = CalcolaSelezioneRomboidale();
                TileManager.ResetGrid();
                TileManager.SetTrigger(this.GetComponent<PlayerController>().PlayerTile, newPoints);
                StartCoroutine(TileManager.WaitMovesAbility(this.gameObject));
                break;
            case SelectType.CROCE:
                newPoints = CalcolaSelezioneACroce();
                TileManager.ResetGrid();
                TileManager.SetTrigger(this.GetComponent<PlayerController>().PlayerTile, newPoints);
                StartCoroutine(TileManager.WaitMovesAbility(this.gameObject));
                break;
            case SelectType.COMPAGNI:
                CalcolaSelezioneCompagni();
                break;
        }
        
    }

    //    protected void PhysicAbilityAttack(GameObject[] targets)
    //    {
    //        foreach(GameObject target in targets)
    //        {
    //            UI.GetComponent<UIManager>().ShowPopupDamage((int)damage, target.transform);
    //            Debug.Log("Prima: " + target.GetComponent<ObjectController>().currentHealth);
    //            target.GetComponent<ObjectController>().currentHealth = target.GetComponent<ObjectController>().currentHealth - (int)damage;
    //            Debug.Log("Dopo: " + target.GetComponent<ObjectController>().currentHealth);
    //        }
    //    }

    public virtual void UsaAbilita()
	{
        if(TileManager.CheckEnemy())
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.tag == "Enemy" ||
                    (hit.collider != null && hit.collider.tag == "Tile" && hit.collider.GetComponent<Tile>().isSelected && hit.collider.GetComponent<Tile>().isEnemy))
            {
                //if(abilityType != AbilityType.MULTIPLO)
                //{
                    GameObject enemyTarget = null;
                    foreach (GameObject enemy in TileManager.enemyInstance)
                    {
                        if (enemy.GetComponent<EnemyController>().EnemyTile.transform.position == hit.collider.transform.position)
                        {
                            enemyTarget = enemy;
                            break;
                        }
                    }
                    if(abilityType == AbilityType.SINGOLO)
                    {
                        GetComponent<PlayerController>().PhysicAttack(enemyTarget, "attack", (int)this.damage);

                        AddAbilityToCooldownList();

                        StartCoroutine(TileManager.WaitMoves(this.gameObject, GameManager.States.END_MOVE));
                    }
                    else if(abilityType == AbilityType.MOVIMENTO)
                    {
                        GameObject tileNearEnemy = enemyTarget.GetComponent<EnemyController>().GetTileNearEnemy();
                        GetComponent<AILerp>().target = tileNearEnemy.transform;
                        GetComponent<PlayerController>().PlayerTile = tileNearEnemy;
                        GetComponent<PlayerController>().PhysicAttack(enemyTarget, "attack", (int)this.damage);

                        AddAbilityToCooldownList();

                        StartCoroutine(TileManager.WaitMoves(this.gameObject, GameManager.States.END_MOVE));
                    }
                    else if (abilityType == AbilityType.TELETRASPORTO)
                    {
                        GameObject tileNearEnemy = enemyTarget.GetComponent<EnemyController>().GetTileNearEnemy();
                        GetComponent<AILerp>().enabled = false;
                        this.gameObject.transform.position = tileNearEnemy.transform.position;
                        GetComponent<PlayerController>().PlayerTile = tileNearEnemy;
                        //GetComponent<PlayerController>().PhysicAttack(enemyTarget, "attack", (int)this.damage);

                        AddAbilityToCooldownList();

                        StartCoroutine(TileManager.WaitMoves(this.gameObject, GameManager.States.END_MOVE));
                    }
                //}
                //else if(abilityType == AbilityType.MULTIPLO)
                //{
                //    List<GameObject> enemyTargets = new List<GameObject>();
                //    foreach (GameObject enemy in TileManager.tilesSelectable)
                //    {
                //        if (enemy.GetComponent<Tile>().isEnemy)
                //        {
                //            enemyTargets.Add(enemy);
                //        }
                //    }
                //    foreach(GameObject target in enemyTargets)
                //    {
                //        GetComponent<PlayerController>().PhysicAttack(target, "attack", (int)this.damage);
                //        StartCoroutine(TileManager.WaitMoves(this.gameObject, GameManager.States.END_MOVE, true, target));
                //    }

                //}
                //if ()
                //{

                //}
                //else
                //{
                //    GameObject tileNearEnemy = enemyTarget.GetComponent<EnemyController>().GetTileNearEnemy();
                //    playerInstance[playerNumber].GetComponent<AILerp>().target = tileNearEnemy.transform;
                //    playerInstance[playerNumber].GetComponent<PlayerController>().PlayerTile = tileNearEnemy;
                //    playerInstance[playerNumber].GetComponent<PlayerController>().PhysicAttack(enemyTarget);
                //    StartCoroutine(WaitMoves(playerInstance[playerNumber], GameManager.States.END_MOVE, true, enemyTarget));
                //}    
            }
        }

    }

    protected void AddAbilityToCooldownList()
    {
        Ability ability = GetComponent(Type.GetType(Ability.activedAbility)) as Ability;
        cooldownList.Add(ability);
        ability.buttonPlayerUI.interactable = false;
        ability.buttonPlayerUI.transform.GetChild(0).gameObject.SetActive(true);
        ability.buttonPlayerUI.GetComponentInChildren<Text>().text = ability.countCooldown.ToString();
    }

    public static void CheckCooldownList()
    {

        if(cooldownList.Count > 0)
        {
            for (int i = 0; i<cooldownList.Count; i++)
            {
                cooldownList[i].buttonPlayerUI.GetComponentInChildren<Text>().text = cooldownList[i].countCooldown.ToString();
                cooldownList[i].countCooldown--;

                if (cooldownList[i].countCooldown < 0)
                {
                    cooldownList[i].buttonPlayerUI.transform.GetChild(0).gameObject.SetActive(false);
                    cooldownList[i].buttonPlayerUI.interactable = true;
                    cooldownList[i].countCooldown = cooldownList[i].cooldown;
                    cooldownList.Remove(cooldownList[i]);
                }
            }

            PrintCooldownList();
        }

    }

    // DEBUG FUNCTION TO REMOVE
    public static void PrintCooldownList()
    {
        for (int i = 0; i < cooldownList.Count; i++)
        {
            print(cooldownList[i].abilityName+" "+ cooldownList[i].countCooldown);
        }

    }
}
