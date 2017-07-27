using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anima2D;

public class PlayerController : ObjectController
{

    private GameObject playerTile;
    public int playerNumber;
    public SpriteMeshInstance mesh;
    public static bool canMove;

    public enum PlayerType
    {
        MELEE,
        RANGED
    };

    public PlayerType playerBehaviour;


    // Update is called once per frame
    void Update () {

        if(GetComponent<AILerp>().target == null || GetComponent<AILerp>().targetReached)
        {
            anim.SetBool("isWalking", false);
        }
        else
        {
            anim.SetBool("isWalking", true);
        }

        if (GameManager.currentState == GameManager.States.EXPLORATION)
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
                    if (ray.collider.tag == "EnemyGroup")
                    {
                        GameManager.currentState = GameManager.States.WAIT;
                        TileManager.AddEnemy(ray.collider.gameObject);
                        break;
                    }
                }
            }
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

    private void OnDestroy()
    {
        for(int i=0; i<TileManager.playerInstance.Count; i++)
        {
            if(TileManager.playerInstance[i])
            {
                TileManager.playerInstance[i].GetComponent<PlayerController>().playerNumber = i;
            }
        }
    }

    public void PhysicAttack(GameObject target)
    {
        if(target)
        {
            bool rotate = false;
            if(Vector3.Distance(transform.position, target.transform.position) < 0 && transform.eulerAngles.y == 0)
            {
                rotate = true;
            }

            if (Vector3.Distance(transform.position, target.transform.position) > 0 && transform.eulerAngles.y == 180)
            {
                rotate = true;
            }

            if (rotate)
            {
                transform.Rotate(new Vector3(0, 180));
            }
            else
            {
                transform.Rotate(new Vector3(0, 0));
            }

            anim.SetTrigger("attack");

            StartCoroutine(WaitAnimation(target));

        }
    }

    IEnumerator WaitAnimation(GameObject target)
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        target.GetComponent<SpriteRenderer>().color = Color.red;

        OnHit(target);
        if (IsDead(target.GetComponent<ObjectController>()))
        {
            target.GetComponent<EnemyController>().EnemyTile.GetComponent<Tile>().isEnemy = false;
            TileManager.enemyInstance.Remove(target);
            Destroy(target);
        }
        else
        {
            StartCoroutine(ResetColor(target));
        }
    }

    public GameObject PlayerTile
    {
        get
        {
            return playerTile;
        }

        set
        {
            if (playerTile != null && value != null)
            {
                playerTile.GetComponent<Tile>().isPlayer = false;
                playerTile.GetComponent<Tile>().isWalkable = true;
            }

            if (value != null)
            {
                value.GetComponent<Tile>().isPlayer = true;
                value.GetComponent<Tile>().isWalkable = false;
            }

            playerTile = value;
        }
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
