using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : ObjectController {

    private GameObject enemyTile;
    public int positionArray;
    private List<GameObject> enemyTileNeighbour;


    public void EnemyIA(GameObject[] players, List<GameObject> selectableTile)
    {
        float closerDistance = 0;
        GameObject closerPlayer = null;
        for(int i=0; i<players.Length; i++)
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

        GameObject closerTile = null;
        for (int i = 0; i < selectableTile.Count; i++)
        {
            if(closerTile != null)
            {
                float dist1 = Vector2.Distance(closerTile.transform.position, closerPlayer.transform.position);
                float dist2 = Vector2.Distance(selectableTile[i].transform.position, closerPlayer.transform.position);
                if (dist1 > dist2)
                {
                    closerTile = selectableTile[i];
                }
            }
            else
            {
                closerTile = selectableTile[i];
            }
        }

        EnemyTile = closerTile;
        GetComponent<AILerp>().target = closerTile.transform;
    }

    public GameObject GetTileNearEnemy()
    {
        EnemyTileNeighbour = new List<GameObject>();
        List<RaycastHit2D[]> hits = new List<RaycastHit2D[]>(4)
        {
            Physics2D.RaycastAll(EnemyTile.transform.position, new Vector2(0, 1), 1f, 1 << LayerMask.NameToLayer("GridMap")),
            Physics2D.RaycastAll(EnemyTile.transform.position, new Vector2(0, -1), 1f, 1 << LayerMask.NameToLayer("GridMap")),
            Physics2D.RaycastAll(EnemyTile.transform.position, new Vector2(1, 0), 1f, 1 << LayerMask.NameToLayer("GridMap")),
            Physics2D.RaycastAll(EnemyTile.transform.position, new Vector2(-1, 0), 1f, 1 << LayerMask.NameToLayer("GridMap"))
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
            if (enemyTile != null)
            {
                enemyTile.GetComponent<Tile>().isBusy = false;
                value.GetComponent<Tile>().isBusy = true;
            }
            enemyTile = value;
        }
    }
}
