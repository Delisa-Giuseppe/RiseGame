using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public Tile enemyTile;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<BoxCollider2D>().isTrigger = false;
            collision.GetComponent<BoxCollider2D>().enabled = false;
            collision.GetComponent<BoxCollider2D>().isTrigger = false;
            collision.GetComponent<AILerp>().target = null;
            GameManager.currentState = GameManager.States.SELECT;
        }
    }
}
