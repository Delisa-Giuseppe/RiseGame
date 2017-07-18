using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : ObjectController
{

    GameObject playerTile;
    public int playerNumber;

    public GameObject PlayerTile
    {
        get
        {
            return playerTile;
        }

        set
        {
            if(playerTile != null)
            {
                playerTile.GetComponent<Tile>().isBusy = false;
                value.GetComponent<Tile>().isBusy = true;
            }
            playerTile = value;
        }
    }

    // Update is called once per frame
    void Update () {
        if(GameManager.currentState == GameManager.States.EXPLORATION)
        {
            List<RaycastHit2D[]> hits = new List<RaycastHit2D[]>(4)
            {
                Physics2D.RaycastAll(transform.position, new Vector2(0, 1), 1f, 1 << LayerMask.NameToLayer("Default")),
                Physics2D.RaycastAll(transform.position, new Vector2(0, -1), 1f, 1 << LayerMask.NameToLayer("Default")),
                Physics2D.RaycastAll(transform.position, new Vector2(1, 0), 1f, 1 << LayerMask.NameToLayer("Default")),
                Physics2D.RaycastAll(transform.position, new Vector2(-1, 0), 1f, 1 << LayerMask.NameToLayer("Default"))
            };


            foreach (RaycastHit2D[] rayCast in hits)
            {
                foreach (RaycastHit2D ray in rayCast)
                {
                    if (ray.collider.tag == "Enemy")
                    {
                        GameManager.currentState = GameManager.States.ENGAGE_ENEMY;
                        break;
                    }
                }
            }
        }
        
    }


    public void PhysicAttack(GameObject target)
    {
        OnHit(target.GetComponent<ObjectController>());
        if (IsDead(target.GetComponent<ObjectController>().health))
        {
            Destroy(target);
            TileManager.enemyInstance[target.GetComponent<EnemyController>().positionArray] = null;
        }
    }
}
