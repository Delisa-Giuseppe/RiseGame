﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anima2D;



public class EnemyController : ObjectController {

    public enum EnemyType
    {
        MELEE,
        RANGED,
        BOSS
    };

    public GameObject enemyTile;
    private List<GameObject> enemyTileNeighbour;
    private static Vector2[] bossPoint;
    public bool canAttack;
    public bool canMagicAttack;
    public List<GameObject> playerAttacked;
    public static List<GameObject> tilesAttackable;
    public EnemyType enemyBehaviour;
    public int position;
    public bool canMove;
    public static bool hasMoved;
    public Color nemesyColor;
    public static GameObject bossTileSelected;
    public static bool isMovable = false;

    private void Start()
    {
        bossPoint = new Vector2[] {
            new Vector2(-1.64f, 1.74f),
            new Vector2(-1.64f, -0.58f),
            new Vector2(2.58f, -0.58f),
            new Vector2(2.58f, 1.74f)
        };

        tilesAttackable = new List<GameObject>();
    }

    private void Update()
    {
        if(anim)
        {
            if (GetComponent<AILerp>().target == null || GetComponent<AILerp>().targetReached)
            {
                anim.SetBool("isWalking", false);
            }
            else
            {
                anim.SetBool("isWalking", true);
            }
        }

        if(gameObject.name.Contains("Nemesy"))
        {
            foreach (SpriteMeshInstance mesh in GetComponentsInChildren<SpriteMeshInstance>())
            {
                mesh.color = nemesyColor;
            }
        }
    }

    public void EnemyIA(List<GameObject> players, List<GameObject> selectableTile, GameManager.States previousState)
    {
        float closerDistance = 0;
        GameObject closerPlayer = null;
        canMagicAttack = false;
        playerAttacked.Clear();

        if (enemyBehaviour == EnemyType.BOSS && GameManager.pointAction == 2)
        {
            foreach(GameObject player in TileManager.playerInstance)
            {
                playerAttacked.Add(player);
            }

            canMagicAttack = true;
        }
        else
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (closerDistance != 0)
                {
                    if (closerDistance > Vector2.Distance(transform.position, players[i].transform.position))
                    {
                        closerDistance = Vector2.Distance(transform.position, players[i].transform.position);
                        closerPlayer = players[i];
                    }
                }
                else
                {
                    closerDistance = Vector2.Distance(transform.position, players[i].transform.position);
                    closerPlayer = players[i];
                }
            }

            canMove = hasMoved;
            canAttack = false;
            if (selectableTile.Contains(closerPlayer.GetComponent<PlayerController>().PlayerTile))
            {
                playerAttacked = new List<GameObject>();
                if ((enemyBehaviour == EnemyType.RANGED) ||
                    (enemyBehaviour == EnemyType.MELEE && Vector2.Distance(transform.position, closerPlayer.transform.position) < 1.5f))
                {
                    canAttack = true;
                    playerAttacked.Add(closerPlayer);
                }
                else if (enemyBehaviour == EnemyType.BOSS)
                {
                    if (Vector2.Distance(transform.position, closerPlayer.transform.position) < 1.5f)
                    {
                        canAttack = true;
                        playerAttacked.Add(closerPlayer);
                        List<RaycastHit2D> playerHits = new List<RaycastHit2D>(2)
                    {
                        Physics2D.Raycast(closerPlayer.transform.position, Vector2.up, 1f, 1 << LayerMask.NameToLayer("GridMap")),
                        Physics2D.Raycast(closerPlayer.transform.position, Vector2.down, 1f, 1 << LayerMask.NameToLayer("GridMap"))
                    };

                        foreach (RaycastHit2D tile in playerHits)
                        {
                            if (tile.collider.tag == "Tile" && tile.collider.GetComponent<Tile>().isPlayer)
                            {
                                foreach (GameObject player in TileManager.playerInstance)
                                {
                                    if (player.GetComponent<PlayerController>().PlayerTile.transform.position == tile.collider.transform.position)
                                    {
                                        playerAttacked.Add(player);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            GameObject closerTile = null;

            for (int i = 0; canMove && i < selectableTile.Count; i++)
            {
                if (closerTile != null)
                {
                    if (!selectableTile[i].GetComponent<Tile>().isPlayer && !selectableTile[i].GetComponent<Tile>().isEnemy)
                    {
                        float dist1 = Vector2.Distance(closerTile.transform.position, closerPlayer.transform.position);
                        float dist2 = Vector2.Distance(selectableTile[i].transform.position, closerPlayer.transform.position);
                        if (dist1 > dist2)
                        {
                            closerTile = selectableTile[i];
                        }
                    }
                }
                else
                {
                    closerTile = selectableTile[i];
                }
            }

            if (canMove && previousState == GameManager.States.MOVE)
            {
                hasMoved = false;
                EnemyTile = closerTile;
                GetComponent<AILerp>().target = closerTile.transform;
            }
        }

    }

	public List<GameObject> GetTileNearEnemy()
    {
		List<GameObject> tileNeighbour = new List<GameObject>();
        List<RaycastHit2D[]> hits = new List<RaycastHit2D[]>(4)
        {
            Physics2D.RaycastAll(EnemyTile.transform.position, new Vector2(0, 1), 1f, 1 << LayerMask.NameToLayer("GridBattle")),
            Physics2D.RaycastAll(EnemyTile.transform.position, new Vector2(0, -1), 1f, 1 << LayerMask.NameToLayer("GridBattle")),
            Physics2D.RaycastAll(EnemyTile.transform.position, new Vector2(1, 0), 1f, 1 << LayerMask.NameToLayer("GridBattle")),
            Physics2D.RaycastAll(EnemyTile.transform.position, new Vector2(-1, 0), 1f, 1 << LayerMask.NameToLayer("GridBattle"))
        };

        foreach (RaycastHit2D[] rayCast in hits)
        {
            foreach (RaycastHit2D ray in rayCast)
            {
                if (ray.collider != null && ray.collider.tag == "Tile" && ray.collider.transform != EnemyTile.transform)
                {
					tileNeighbour.Add(ray.collider.gameObject);
                }
            }
        }

//        GameObject tileSelected = null;
//        foreach(GameObject tile in EnemyTileNeighbour)
//        {
//            if(tile.GetComponent<Tile>().isWalkable)
//            {
//                tileSelected = tile;
//                break;
//            }
//        }

		return tileNeighbour;
    }

    public void SetTrigger()
    {
        StartCoroutine(ResetTrigger());

        bossTileSelected = enemyTile;
        enemyTile.layer = LayerMask.NameToLayer("GridBattle");
        enemyTile.GetComponent<Tile>().isChecked = true;
        enemyTile.GetComponent<Tile>().isAttackable = true;
        enemyTile.GetComponent<PolygonCollider2D>().SetPath(0, bossPoint);
        enemyTile.GetComponent<PolygonCollider2D>().isTrigger = true;

    }

    public static IEnumerator ResetTrigger()
    {
        while (tilesAttackable.Count == 0)
        {
            yield return null;
        }

        isMovable = true;
        bossTileSelected.GetComponent<PolygonCollider2D>().isTrigger = false;
        bossTileSelected.GetComponent<PolygonCollider2D>().SetPath(0, TileManager.quadInitialPoint);
    }

    public static void ResetBossGrid()
    {
        bossTileSelected.GetComponent<Tile>().isChecked = false;
        bossTileSelected.GetComponent<Tile>().isAttackable = false;
        bossTileSelected.layer = LayerMask.NameToLayer("GridMap");
        foreach (GameObject tileObj in tilesAttackable)
        {
            tileObj.layer = LayerMask.NameToLayer("GridMap");
            tileObj.GetComponent<Tile>().isChecked = false;
            tileObj.GetComponent<Tile>().isAttackable = false;
        }
        tilesAttackable.Clear();
    }

    public List<GameObject> EnemyTileNeighbour
    {
        get
        {
            return enemyTileNeighbour;
        }

        set
        {
            enemyTileNeighbour = value;
        }
    }

    public GameObject EnemyTile
    {
        get
        {
            return enemyTile;
        }

        set
        {
            if(enemyTile != null)
            {
                enemyTile.GetComponent<Tile>().isWalkable = true;
                enemyTile.GetComponent<Tile>().isEnemy = false;
            }
            if(value != null)
            {
                value.GetComponent<Tile>().isWalkable = false;
                value.GetComponent<Tile>().isEnemy = true;
            }
            
            enemyTile = value;
        }
    }

    private void OnMouseOver()
    {
        Tile tile = enemyTile.GetComponent<Tile>();
        if(TurnManager.currentObjectTurn && TurnManager.currentObjectTurn.tag == "Player" && tile.isEnemy && tile.isSelected 
            && GameManager.currentState == GameManager.States.FIGHT)
        {
            tile.SetImageSprite(enemyTile.GetComponent<Tile>().borderFullBattle);
        }
    }

    private void OnMouseExit()
    {
        Tile tile = enemyTile.GetComponent<Tile>();
        if (TurnManager.currentObjectTurn && TurnManager.currentObjectTurn.tag == "Player" && tile.isEnemy && tile.isSelected 
            && GameManager.currentState == GameManager.States.FIGHT)
        {
            tile.SetImageSprite(enemyTile.GetComponent<Tile>().borderEmpty);
        }
    }

    public void StartFightAnimation()
    {
        anim.SetBool("isFighting", true);
    }

	public void PhysicAttack(GameObject target, string animationName ,int damage)
    {
        if (target)
        {
            if (transform.position.x > target.transform.position.x)
            {
                transform.eulerAngles = new Vector3(0f, 180f);
            }

            if (transform.position.x < target.transform.position.x)
            {
                transform.eulerAngles = new Vector3(0f, 0f);
            }

            anim.SetTrigger(animationName);

            StartCoroutine(WaitAnimation(target, damage));
        }
    }

    public void PhysicAttack(List<GameObject> target, string animationName, int damage)
    {
        if (target.Count > 0)
        {
            if (transform.position.x > target[0].transform.position.x)
            {
                transform.eulerAngles = new Vector3(0f, 0f);
            }

            if (transform.position.x < target[0].transform.position.x)
            {
                transform.eulerAngles = new Vector3(0f, 180f);
            }
            anim.SetTrigger(animationName);
            StartCoroutine(WaitAnimation(target, damage));
        }
    }

    public void MagicAttack(List<GameObject> target, string animationName, int damage)
    {
        if (target.Count > 0)
        {
            if (transform.position.x > target[0].transform.position.x)
            {
                transform.eulerAngles = new Vector3(0f, 0f);
            }

            if (transform.position.x < target[0].transform.position.x)
            {
                transform.eulerAngles = new Vector3(0f, 180f);
            }
            anim.SetTrigger(animationName);
            StartCoroutine(WaitAnimation(target, damage));
        }
    }

    IEnumerator WaitAnimation(GameObject target, int damage)
    {
        yield return new WaitForSeconds(0.7f);

        foreach (SpriteMeshInstance mesh in target.GetComponentsInChildren<SpriteMeshInstance>())
        {
            mesh.color = Color.red;
        }

        OnHit(target, damage);
        if (IsDead(target.GetComponent<ObjectController>()))
        {
            target.GetComponent<PlayerController>().PlayerTile.GetComponent<Tile>().isPlayer = false;
            TileManager.playerInstance.Remove(target);
            TileManager.playerDead.Add(target);
            TurnManager.turns[TurnManager.turns.IndexOf(target)] = null;
            TurnManager.refreshTurn = true;
            StartCoroutine(ResetColor(target));
            target.GetComponentInChildren<Animator>().SetTrigger("isDead");
            target.GetComponentInChildren<Animator>().SetBool("isFighting", false);
            target.GetComponentInChildren<Animator>().SetBool("isWalking", false);
            for (int i = 0; i < TileManager.playerInstance.Count; i++)
            {
                if (TileManager.playerInstance[i])
                {
                    TileManager.playerInstance[i].GetComponent<PlayerController>().playerNumber = i;
                }
            }
            //Destroy(target);
        }
        else
        {
            TurnManager.refreshTurn = true;
            StartCoroutine(ResetColor(target));
        }
    }

    IEnumerator WaitAnimation(List<GameObject> targets, int damage)
    {
        if (TurnManager.currentObjectTurn && TurnManager.currentObjectTurn.name == "Dragon")
        {
            DragonController.instanceFlame = true;
        }

        yield return new WaitForSeconds(1f);
        

        foreach (GameObject target in targets)
        {
            foreach (SpriteMeshInstance mesh in target.GetComponentsInChildren<SpriteMeshInstance>())
            {
                mesh.color = Color.red;
            }

            OnHit(target, damage);

            if (IsDead(target.GetComponent<ObjectController>()))
            {
                target.GetComponent<PlayerController>().PlayerTile.GetComponent<Tile>().isPlayer = false;
                TileManager.playerInstance.Remove(target);
                TileManager.playerDead.Add(target);
                TurnManager.turns[TurnManager.turns.IndexOf(target)] = null;
                TurnManager.refreshTurn = true;
                StartCoroutine(ResetColor(target));
                target.GetComponentInChildren<Animator>().SetTrigger("isDead");
                target.GetComponentInChildren<Animator>().SetBool("isFighting", false);
                target.GetComponentInChildren<Animator>().SetBool("isWalking", false);
                for (int i = 0; i < TileManager.playerInstance.Count; i++)
                {
                    if (TileManager.playerInstance[i])
                    {
                        TileManager.playerInstance[i].GetComponent<PlayerController>().playerNumber = i;
                    }
                }
                //Destroy(target);
            }
            else
            {
                TurnManager.refreshTurn = true;
                StartCoroutine(ResetColor(target));
            }
        }
    }

    IEnumerator ResetColor(GameObject obj)
    {
        yield return new WaitForSeconds(0.5f);
        if(obj != null)
        {
            foreach (SpriteMeshInstance mesh in obj.GetComponentsInChildren<SpriteMeshInstance>())
            {
                mesh.color = Color.white;
            }
        }
    }
}
