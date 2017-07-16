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
        IS_MOVING,
        END_MOVE,
        WAIT
    }

    public static States currentState;

    TileManager tileManager;
    TurnManager turnManager;

    // Use this for initialization
    void Start () {
        tileManager = GetComponent<TileManager>();
        turnManager = GetComponent<TurnManager>();

        InitLevel();
	}

    // Update is called once per frame
    void Update()
    {
        if (currentState == States.SELECT && turnManager.actualPhaseTurn != TurnManager.PhaseTurnState.EXECUTE)
        {
            tileManager.UpdateGrid(turnManager.GetNextTurn());
        }

        if (Input.GetMouseButtonDown(0))
        {   
           if(currentState == States.EXPLORATION)
            {
                tileManager.MovePlayer(0);
            }

           if(currentState == States.MOVE)
            {
                if(turnManager.currentObjectTurn.tag == "Player")
                {
                    tileManager.MovePlayer(turnManager.currentObjectTurn.GetComponent<PlayerController>().playerNumber);
                }
            }
            
        }

        if (currentState == States.IS_MOVING)
        {
            StartCoroutine(tileManager.WaitMoves(turnManager.currentObjectTurn));
        }

        if (currentState == States.MOVE && turnManager.currentObjectTurn.tag == "Enemy")
        {
            tileManager.EnemyIA();
        }

        if (currentState == States.ENGAGE_ENEMY)
        {
            tileManager.ShowGrid();
            turnManager.CalculateTurns(TileManager.playerInstance, TileManager.enemyInstance);
            tileManager.PositionBattle();
        }

        if (currentState == States.END_MOVE)
        {
            tileManager.ResetGrid();
            if (turnManager.IsAllTurnFinished())
            {
                currentState = States.EXPLORATION;
            }
            else
            {
                currentState = States.SELECT;
                turnManager.actualPhaseTurn = TurnManager.PhaseTurnState.INIT;
            }
        }
    }

    void InitLevel()
    {
        currentState = States.EXPLORATION;
        tileManager.CreateGrid(width, height);
    }

}
