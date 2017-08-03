﻿using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public int height;
    public int width;
    public int maxPointAction;
    public static int pointAction;
    public GameObject gameOver;

    public enum States
    {
        EXPLORATION,
        ENGAGE_ENEMY,
        SELECT,
        MOVE,
        PRE_FIGHT,
        FIGHT,
        ATTACK,
        ABILITY,
        END_MOVE,
        END_TURN,
        WAIT,
        PAUSED,
        GAME_OVER
    }

    public static States currentState;
    public static bool refreshPath;

    TileManager tileManager;
    TurnManager turnManager;
    static GameObject pathfind;

    // Use this for initialization
    void Start () {
        pointAction = maxPointAction;
        tileManager = GetComponent<TileManager>();
        turnManager = GetComponent<TurnManager>();
        pathfind = GameObject.FindGameObjectWithTag("Pathfind");
        InitLevel();
        pathfind.GetComponent<AstarPath>().Scan();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == States.END_TURN && TurnManager.currentTurnState == TurnManager.TurnStates.FINISH)
        {
            pointAction = maxPointAction;
            tileManager.HideGrid();

            GameManager.currentState = GameManager.States.WAIT;
            foreach (GameObject player in TileManager.playerDead)
            {
                player.GetComponent<PlayerController>().ResurrectPlayer();
            }
            TileManager.playerDead.Clear();
            foreach (GameObject player in TileManager.playerInstance)
            {
                player.GetComponent<PlayerController>().playerNumber = player.GetComponent<PlayerController>().originalPlayerNumber;
            }
            TileManager.playerInstance.Sort(delegate (GameObject a, GameObject b) {
                return (a.GetComponent<PlayerController>().playerNumber).CompareTo(b.GetComponent<PlayerController>().playerNumber);
            });
            GameManager.currentState = GameManager.States.EXPLORATION;
        }

        if (currentState == States.SELECT)
        {
            if(TurnManager.currentTurnState == TurnManager.TurnStates.INIT)
            {
                turnManager.GetNextTurn();
                if (TurnManager.currentObjectTurn && TurnManager.currentObjectTurn.tag == "Enemy"
                    && TurnManager.currentObjectTurn.GetComponent<EnemyController>().enemyBehaviour == EnemyController.EnemyType.RANGED &&
                    TileManager.CheckPlayer())
                {
                    tileManager.UpdateGrid(TurnManager.currentObjectTurn, false);
                }
                else
                {
                    tileManager.UpdateGrid(TurnManager.currentObjectTurn, true);
                }
                    
            }
            else if(TurnManager.currentTurnState == TurnManager.TurnStates.EXECUTE)
            {
                if(TurnManager.currentObjectTurn && TurnManager.currentObjectTurn.tag == "Player")
                {
                    tileManager.UpdateGrid(TurnManager.currentObjectTurn, PlayerController.canMove);
                }
                else if (TurnManager.currentObjectTurn && TurnManager.currentObjectTurn.tag == "Enemy")
                {
                    if(TurnManager.currentObjectTurn.GetComponent<EnemyController>().enemyBehaviour == EnemyController.EnemyType.RANGED)
                    {
                        tileManager.UpdateGrid(TurnManager.currentObjectTurn, !EnemyController.rangedCanAttack);
                    }
                    else
                    {
                        tileManager.UpdateGrid(TurnManager.currentObjectTurn, EnemyController.hasMoved);
                    }   
                }
                
            }
        } 

        if(TurnManager.currentObjectTurn && TurnManager.currentObjectTurn.tag == "Player" && Input.GetKeyDown(KeyCode.Space) 
            && (currentState == States.MOVE || currentState == States.FIGHT || currentState == States.ABILITY))
        {
            pointAction--;
            TileManager.ResetGrid();
            StartCoroutine(WaitTurn());
        }

        if (Input.GetMouseButtonDown(0))
        {   
           if(currentState == States.EXPLORATION)
            {
                tileManager.MovePlayer(TileManager.playerInstance.Count - 1);
            }

           if(currentState == States.MOVE)
            {
                if(TurnManager.currentObjectTurn.tag == "Player")
                {
                    tileManager.MovePlayer(TurnManager.currentObjectTurn.GetComponent<PlayerController>().playerNumber);
                }
                
            }
            
           if(currentState == States.FIGHT)
           {
                if(TurnManager.currentObjectTurn && TurnManager.currentObjectTurn.tag == "Player" && TileManager.CheckEnemy())
                {
                    if(!tileManager.AttackEnemy(TurnManager.currentObjectTurn.GetComponent<PlayerController>().playerNumber) && PlayerController.canMove)
                    {
                        TileManager.ResetGrid();
                        tileManager.UpdateGrid(TurnManager.currentObjectTurn, true);
                    }
                }
                else if (TurnManager.currentObjectTurn && TurnManager.currentObjectTurn.tag == "Player" && TileManager.CheckBoss())
                {
                    if (!tileManager.AttackBoss(TurnManager.currentObjectTurn.GetComponent<PlayerController>().playerNumber) && PlayerController.canMove)
                    {
                        TileManager.ResetGrid();
                        tileManager.UpdateGrid(TurnManager.currentObjectTurn, true);
                    }
                }
                else if(TurnManager.currentObjectTurn && TurnManager.currentObjectTurn.tag == "Player" && PlayerController.canMove)
                {
                    TileManager.ResetGrid();
                    tileManager.UpdateGrid(TurnManager.currentObjectTurn, true);
                }
           }

			if(currentState == States.ABILITY)
			{
                if(TurnManager.currentObjectTurn && TurnManager.currentObjectTurn.tag == "Player")
                {
                    (TurnManager.currentObjectTurn.GetComponent(Type.GetType(Ability.activedAbility)) as Ability).UsaAbilita();
                }
                

            }
        }

        if (TurnManager.currentObjectTurn && TurnManager.currentObjectTurn.tag == "Player" && (Input.GetMouseButtonDown(1) && currentState == States.MOVE) || currentState == States.PRE_FIGHT)
        {
            currentState = States.FIGHT;
            if (TurnManager.currentObjectTurn.tag == "Player")
            {
                TileManager.ResetGrid();
                tileManager.UpdateGrid(TurnManager.currentObjectTurn, false);
            }
        }

        if (refreshPath)
        {
            refreshPath = false;
        }

        if (TurnManager.currentObjectTurn && TurnManager.currentObjectTurn.tag == "Enemy" && 
                (currentState == States.MOVE || currentState == States.FIGHT) && TurnManager.currentTurnState == TurnManager.TurnStates.EXECUTE)
        {
            TurnManager.currentTurnState = TurnManager.TurnStates.EXECUTED;
            tileManager.MoveEnemy(TurnManager.currentObjectTurn);
        }
        

        if (currentState == States.ENGAGE_ENEMY)
        {
            turnManager.CreateEnemyUI(TileManager.enemyInstance);
            turnManager.ShowBattleImage();
            tileManager.ShowGrid();
            turnManager.CalculateTurns(TileManager.playerInstance, TileManager.enemyInstance);
            tileManager.PositionBattle();
        }

        if (currentState == States.END_MOVE && TurnManager.currentTurnState == TurnManager.TurnStates.EXECUTED)
        {
            pointAction--;
            TurnManager.currentObjectTurn.GetComponent<AILerp>().target = null;
            TurnManager.currentObjectTurn.GetComponent<AILerp>().canMove = false;
            if (pointAction <=0)
            {
                TileManager.ResetGrid();
                StartCoroutine(WaitTurn());
            }
            else
            {
                TileManager.ResetGrid();
                if (TurnManager.currentObjectTurn.tag == "Player")
                {
                    if (PlayerController.canMove)
                    {
                        StartCoroutine(turnManager.RecalculateTurn(TileManager.playerInstance, TileManager.enemyInstance, States.SELECT, TurnManager.TurnStates.EXECUTE));
                    }
                    else
                    {
                        StartCoroutine(turnManager.RecalculateTurn(TileManager.playerInstance, TileManager.enemyInstance, States.PRE_FIGHT, TurnManager.TurnStates.EXECUTE));
                    }
                }
                else if(TurnManager.currentObjectTurn.tag == "Enemy")
                {
                    StartCoroutine(turnManager.RecalculateTurn(TileManager.playerInstance, TileManager.enemyInstance, States.SELECT, TurnManager.TurnStates.EXECUTE));
                }
            }
            
        }

        if (TileManager.playerInstance.Count <= 0)
        {
            currentState = States.GAME_OVER;
            StartCoroutine(ShowGameOver());
        }
    }


    void InitLevel()
    {
        currentState = States.EXPLORATION;
        tileManager.CreateGrid(width, height);
    }

    IEnumerator ShowGameOver()
    {
        yield return new WaitForSeconds(4f);
        gameOver.SetActive(true);
        StopAllCoroutines();
    }

    IEnumerator WaitTurn()
    {
        StartCoroutine(turnManager.RecalculateTurn(TileManager.playerInstance, TileManager.enemyInstance, States.WAIT, TurnManager.TurnStates.INIT));
        yield return new WaitForSeconds(1f);
        if (TurnManager.currentTurnState != TurnManager.TurnStates.FINISH)
        {
            TurnManager.currentTurn = TurnManager.currentTurn + 1;
            pointAction = maxPointAction;

            if (TurnManager.currentObjectTurn.tag == "Player")
            {
                PlayerController.canMove = true;
            }
            else if (TurnManager.currentObjectTurn.tag == "Enemy")
            {
                EnemyController.hasMoved = true;   
            }
            turnManager.ResetTurnColor();
            currentState = States.SELECT;
        }
    }

    public static void FinishLevel()
    {
        foreach(GameObject player in TileManager.playerInstance)
        {
            player.GetComponent<AILerp>().target = null;
        }
        SceneManager.LoadScene("PowerUpScene");
    }

    public static void RefreshPath()
    {
        pathfind.GetComponent<AstarPath>().Scan();
        //GameManager.refreshPath = true;
    }
}
