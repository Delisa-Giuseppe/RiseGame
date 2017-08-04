using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anima2D;

public class PlayerController : ObjectController
{

    private GameObject playerTile;
    public static string nextScene;
    public int playerNumber;
    public static bool canMove;
    public GameObject playerUI;
    public int originalPlayerNumber;

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
                    if (ray.collider.tag == "EnemyGroup" || ray.collider.tag == "Nemesy" || ray.collider.tag == "Boss")
                    {
                        GameManager.currentState = GameManager.States.WAIT;
                        TileManager.AddEnemy(ray.collider.gameObject);
                        break;
                    }

                    if(ray.collider.tag == "EndLevel")
                    {
                        foreach(GameObject player in TileManager.playerInstance)
                        {
                            DontDestroyOnLoad(player);
                        }
                        string[] sceneName = ray.collider.name.Split('_');
                        nextScene = sceneName[1];
                        GameManager.FinishLevel();
                    }

                    if(ray.collider.tag == "Totem")
                    {
                        ray.collider.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                        ray.collider.GetComponent<TotemController>().OnCutscene();
                        GameManager.currentState = GameManager.States.WAIT;
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

	public void PhysicAttack(GameObject target, string animationName, int damage)
    {
        if(target)
        {
            if(transform.position.x > target.transform.position.x)
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
        yield return new WaitForSeconds(0.6f);

        foreach (SpriteMeshInstance mesh in target.GetComponentsInChildren<SpriteMeshInstance>())
        {
            mesh.color = Color.red;
        }

        OnHit(target, damage);
        if (IsDead(target.GetComponent<ObjectController>()))
        {
            target.GetComponent<EnemyController>().EnemyTile.GetComponent<Tile>().isEnemy = false;
            TileManager.enemyInstance.Remove(target);
            TurnManager.turns[TurnManager.turns.IndexOf(target)] = null;
            TurnManager.refreshTurn = true;
            StartCoroutine(ResetColor(target));
            target.GetComponentInChildren<Animator>().SetTrigger("isDead");
            //Destroy(target);
        }
        else
        {
            TurnManager.refreshTurn = true;
            StartCoroutine(ResetColor(target));
        }
    }

    public void ResurrectPlayer()
    {
        if (currentHealth <= 0)
        {
            currentHealth = totalHealth / 2;
            this.gameObject.GetComponent<AILerp>().canMove = true;
            UI.GetComponent<UIManager>().SetPlayerHealthBar(this.gameObject);
            TileManager.playerInstance.Add(this.gameObject);
            StartCoroutine(PlayResurrect());
        }

    }

    IEnumerator PlayResurrect()
    {
        yield return new WaitForSeconds(2f);
        anim.SetTrigger("isResurrect");
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
            foreach (SpriteMeshInstance mesh in obj.GetComponentsInChildren<SpriteMeshInstance>())
            {
                mesh.color = Color.white;
            }
        }
    }
}
