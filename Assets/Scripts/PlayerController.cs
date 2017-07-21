using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : ObjectController
{

    GameObject playerTile;
    public int playerNumber;
    public int positionArray;

    public GameObject PlayerTile
    {
        get
        {
            return playerTile;
        }

        set
        {
            if(playerTile != null && value != null)
            {
                playerTile.GetComponent<Tile>().isPlayer = false;
                value.GetComponent<Tile>().isPlayer = true;
                playerTile.GetComponent<Tile>().isBusy = false;
                value.GetComponent<Tile>().isBusy = true;
                playerTile.GetComponent<Tile>().isWalkable = true;
                value.GetComponent<Tile>().isWalkable = false;
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
                Physics2D.RaycastAll(transform.position, new Vector2(0, 1), 1f, 1 << LayerMask.NameToLayer("World")),
                Physics2D.RaycastAll(transform.position, new Vector2(0, -1), 1f, 1 << LayerMask.NameToLayer("World")),
                Physics2D.RaycastAll(transform.position, new Vector2(1, 0), 1f, 1 << LayerMask.NameToLayer("World")),
                Physics2D.RaycastAll(transform.position, new Vector2(-1, 0), 1f, 1 << LayerMask.NameToLayer("World"))
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
        target.GetComponent<SpriteRenderer>().color = Color.red;
        OnHit(target);
        if (IsDead(target.GetComponent<ObjectController>()))
        {
            Destroy(target);
            TileManager.enemyInstance.Remove(target);
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
