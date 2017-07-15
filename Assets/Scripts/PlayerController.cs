using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Tile playerTile;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(GameManager.currentState == GameManager.States.EXPLORATION)
        {
            List<RaycastHit2D[]> hits = new List<RaycastHit2D[]>(4)
            {
                Physics2D.RaycastAll(transform.position, new Vector2(0, 1), 1),
                Physics2D.RaycastAll(transform.position, new Vector2(0, -1), 1),
                Physics2D.RaycastAll(transform.position, new Vector2(1, 0), 1),
                Physics2D.RaycastAll(transform.position, new Vector2(-1, 0), 1)
            };


            foreach (RaycastHit2D[] rayCast in hits)
            {
                foreach (RaycastHit2D ray in rayCast)
                {
                    if (ray.collider.tag == "Enemy")
                    {
                        GameManager.currentState = GameManager.States.SELECT;
                        break;
                    }
                }
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<BoxCollider2D>().isTrigger = false;
            GetComponent<AILerp>().target = null;
            collision.GetComponent<BoxCollider2D>().enabled = false;
            collision.GetComponent<BoxCollider2D>().isTrigger = false;
            GameManager.currentState = GameManager.States.SELECT;
        }
    }
}
