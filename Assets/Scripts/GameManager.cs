using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public int height;
    public int width;
    public int moves;

    public enum EnemyClass
    {
        MELEE,
        RANGED
    }

    public enum States
    {
        EXPLORATION,
        ENGAGE_ENEMY,
        SELECT,
        MOVE,
        END_MOVE
    }

    public static States currentState;

    TileManager tileManager;
    // Use this for initialization
    void Start () {
        tileManager = GetComponent<TileManager>();
        InitLevel();
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(currentState == States.SELECT)
            {
                TileManager.moveCount = moves;
                tileManager.UpdateGrid();
            }
            else if(currentState == States.EXPLORATION || currentState == States.MOVE)
            {
                tileManager.MovePlayer();
            }
            
        }

        if (currentState == States.END_MOVE)
        {
            tileManager.ResetGrid();
        }
    }

    void InitLevel()
    {
        currentState = States.EXPLORATION;
        tileManager.CreateGrid(width, height);
    }

}
