using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public int height;
    public int width;

    public enum States
    {
        EXPLORATION,
        ENGAGE_ENEMY,
        SELECT,
        MOVE,
        END_MOVE,
        WAIT
    }

    public static States currentState;

    TileManager tileManager;
    TurnManager turnManager;
    GameObject pathfind;

    // Use this for initialization
    void Start () {
        tileManager = GetComponent<TileManager>();
        turnManager = GetComponent<TurnManager>();
        pathfind = GameObject.FindGameObjectWithTag("Pathfind");
        InitLevel();
	}

    // Update is called once per frame
    void Update()
    {
        pathfind.GetComponent<AstarPath>().Scan();
        if (currentState == States.EXPLORATION)
        {
            tileManager.HideGrid();
        }
        if (currentState == States.SELECT)
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

        if (currentState == States.MOVE && turnManager.currentObjectTurn.tag == "Enemy")
        {
            tileManager.MoveEnemy(turnManager.currentObjectTurn);
        }

        if (currentState == States.ENGAGE_ENEMY)
        {
            tileManager.ShowGrid();
            turnManager.CalculateTurns(TileManager.playerInstance, TileManager.enemyInstance);
            tileManager.PositionBattle();
            //pathfind.GetComponent<AstarPath>().graphs[0].
        }

        if (currentState == States.END_MOVE)
        {
            tileManager.ResetGrid();
            if (turnManager.IsAllTurnFinished())
            {
                StartCoroutine(turnManager.RecalculateTurn(TileManager.playerInstance, TileManager.enemyInstance));
            }
            else
            {
                StartCoroutine(tileManager.WaitMoves(turnManager.currentObjectTurn, States.SELECT, false, null));
            }
        }
    }

    void InitLevel()
    {
        currentState = States.EXPLORATION;
        tileManager.CreateGrid(width, height);
    }

}
