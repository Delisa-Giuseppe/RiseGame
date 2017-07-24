using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyController : ObjectController {

    public enum EnemyType
    {
        MELEE,
        RANGED
    };

    public GameObject enemyTile;
    private List<GameObject> enemyTileNeighbour;
    public bool canAttack;
    public GameObject playerAttacked;
    public EnemyType enemyBehaviour;
    public int position;

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

        

        bool moveEnemy = true;
        canAttack = false;
        if (selectableTile.Contains(closerPlayer.GetComponent<PlayerController>().PlayerTile))
        {
            canAttack = true;
            playerAttacked = closerPlayer;
        }

        GameObject closerTile = null;

        if((enemyBehaviour == EnemyType.RANGED && canAttack) ||  
            (enemyBehaviour == EnemyType.MELEE && Vector2.Distance(transform.position, closerPlayer.transform.position) < 1.5f))
        {
            moveEnemy = false;
        }

        for (int i = 0; moveEnemy && i < selectableTile.Count; i++)
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

        if (moveEnemy)
        {
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
            if(enemyTile != null && value != null)
            {
                enemyTile.GetComponent<Tile>().isWalkable = true;
                value.GetComponent<Tile>().isWalkable = false;
                enemyTile.GetComponent<Tile>().isEnemy = false;
                value.GetComponent<Tile>().isEnemy = true;
            }
            
            enemyTile = value;
        }
    }

    public void PhysicAttack(GameObject target)
    {
        target.GetComponent<SpriteRenderer>().color = Color.red;
        OnHit(target);
        if (IsDead(target.GetComponent<ObjectController>()))
        {
            Destroy(target);
            TileManager.playerInstance.Remove(target);
        }

        StartCoroutine(ResetColor(target));
    }

    IEnumerator ResetColor(GameObject obj)
    {
        yield return new WaitForSeconds(0.5f);
        if(obj != null)
        {
            obj.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}
