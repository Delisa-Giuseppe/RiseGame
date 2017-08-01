using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public int height;
    public int width;
    public int maxPointAction;
    public static int pointAction;

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
        WAIT,
        PAUSED
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
        if (currentState == States.EXPLORATION && TurnManager.currentTurnState == TurnManager.TurnStates.FINISH)
        {
            pointAction = maxPointAction;
            TurnManager.currentTurnState = TurnManager.TurnStates.WAIT;
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
            GameManager.currentState = GameManager.States.EXPLORATION;
        }

        if (currentState == States.SELECT)
        {
            if(TurnManager.currentTurnState == TurnManager.TurnStates.INIT)
            {
                turnManager.GetNextTurn();
                tileManager.UpdateGrid(TurnManager.currentObjectTurn, true);
            }
            else if(TurnManager.currentTurnState == TurnManager.TurnStates.EXECUTE)
            {
                tileManager.UpdateGrid(TurnManager.currentObjectTurn, PlayerController.canMove);
            }
        } 

        if(TurnManager.currentObjectTurn && TurnManager.currentObjectTurn.tag == "Player" && Input.GetKeyDown(KeyCode.Space) 
            && (currentState == States.MOVE || currentState == States.FIGHT || currentState == States.ABILITY))
        {
            pointAction = maxPointAction;
            TurnManager.currentObjectTurn.GetComponent<AILerp>().canMove = false;
            TileManager.ResetGrid();
            if (TurnManager.currentObjectTurn.tag == "Player")
            {
                PlayerController.canMove = true;
            }
            else if (TurnManager.currentObjectTurn.tag == "Enemy")
            {
                EnemyController.hasMoved = true;
            }
            turnManager.ResetTurnColor();
            TurnManager.currentTurn = TurnManager.currentTurn + 1;
            StartCoroutine(turnManager.RecalculateTurn(TileManager.playerInstance, TileManager.enemyInstance, States.SELECT, TurnManager.TurnStates.INIT));
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
                else if(TurnManager.currentObjectTurn && TurnManager.currentObjectTurn.tag == "Player" && PlayerController.canMove)
                {
                    TileManager.ResetGrid();
                    tileManager.UpdateGrid(TurnManager.currentObjectTurn, true);
                }
           }

			if(currentState == States.ABILITY)
			{
				TurnManager.currentObjectTurn.GetComponent <Ability>().UsaAbilita();
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
                currentState == States.MOVE && TurnManager.currentTurnState == TurnManager.TurnStates.EXECUTE)
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
            if (pointAction <=0)
            {
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
    }


    void InitLevel()
    {
        currentState = States.EXPLORATION;
        tileManager.CreateGrid(width, height);
    }

    IEnumerator WaitTurn()
    {
        StartCoroutine(turnManager.RecalculateTurn(TileManager.playerInstance, TileManager.enemyInstance, States.WAIT, TurnManager.TurnStates.INIT));
        yield return new WaitForSeconds(2f);
        pointAction = maxPointAction;
        TurnManager.currentObjectTurn.GetComponent<AILerp>().canMove = false;
        TileManager.ResetGrid();
        if (TurnManager.currentObjectTurn.tag == "Player")
        {
            PlayerController.canMove = true;
        }
        else if (TurnManager.currentObjectTurn.tag == "Enemy")
        {
            EnemyController.hasMoved = true;
        }
        turnManager.ResetTurnColor();
        TurnManager.currentTurn = TurnManager.currentTurn + 1;
        currentState = States.SELECT;
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
