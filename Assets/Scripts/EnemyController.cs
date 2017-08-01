using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anima2D;



public class EnemyController : ObjectController {

    public enum EnemyType
    {
        MELEE,
        RANGED
    };

    private GameObject enemyTile;
    private List<GameObject> enemyTileNeighbour;
    public bool canAttack;
    public GameObject playerAttacked;
    public EnemyType enemyBehaviour;
    public int position;
    public bool canMove;
    public static bool hasMoved = true;

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
    }

    public void EnemyIA(List<GameObject> players, List<GameObject> selectableTile)
    {
        float closerDistance = 0;
        GameObject closerPlayer = null;
        for(int i=0; i<players.Count; i++)
        {
            if(closerDistance != 0)
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
            if ((enemyBehaviour == EnemyType.RANGED) ||
                (enemyBehaviour == EnemyType.MELEE && Vector2.Distance(transform.position, closerPlayer.transform.position) < 1.5f))
            {
                canAttack = true;
                playerAttacked = closerPlayer;
            }
        }

        GameObject closerTile = null;

        for (int i = 0; canMove && i < selectableTile.Count; i++)
        {
            if(closerTile != null)
            {
                if(!selectableTile[i].GetComponent<Tile>().isPlayer && !selectableTile[i].GetComponent<Tile>().isEnemy)
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

        if (!canAttack && canMove)
        {
            hasMoved = false;
            EnemyTile = closerTile;
            //transform.position = closerTile.transform.position;
            GetComponent<AILerp>().target = closerTile.transform;
        }

        //enemyTile.GetComponent<PolygonCollider2D>().enabled = false;

    }

    public GameObject GetTileNearEnemy()
    {
        EnemyTileNeighbour = new List<GameObject>();
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
                    EnemyTileNeighbour.Add(ray.collider.gameObject);
                }
            }
        }

        GameObject tileSelected = null;
        foreach(GameObject tile in EnemyTileNeighbour)
        {
            if(tile.GetComponent<Tile>().isWalkable)
            {
                tileSelected = tile;
                break;
            }
        }

        return tileSelected;
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

    public void StopFightAnimation()
    {
        anim.SetBool("isFighting", false);
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

	IEnumerator WaitAnimation(GameObject target, int damage)
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

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
