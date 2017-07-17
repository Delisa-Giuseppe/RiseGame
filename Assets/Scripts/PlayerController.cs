using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public GameObject playerTile;
    
    public int playerNumber;
    public int strength; // Attributo forza
    public int constitution; // Attributo costituzione
    public int skill; // Attributo destrezza
    public int mind; // Attributo intelligenza

    //Stats of single player
    public int health;
    public int physicAttack;
    public int magicAttack;
    public int moves;
    public int critic;
    public int evasion;


    // Use this for initialization
    void Awake () {
        CalculateStatistics();
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

    private void CalculateStatistics()
    {
        health = 2 * strength + 6 * constitution + 2 * mind;
        magicAttack = 5 * mind;
        physicAttack = 3 * strength + constitution;
        moves = skill;
        critic = skill;
        evasion = skill;
    }

    public void OnDamage(int damage)
    {
        health -= damage; 
    }

}
