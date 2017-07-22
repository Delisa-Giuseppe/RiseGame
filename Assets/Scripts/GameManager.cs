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
    public static bool refreshPath;

    TileManager tileManager;
    TurnManager turnManager;
    GameObject pathfind;

    // Use this for initialization
    void Start () {
        tileManager = GetComponent<TileManager>();
        turnManager = GetComponent<TurnManager>();
        pathfind = GameObject.FindGameObjectWithTag("Pathfind");
        InitLevel();
        pathfind.GetComponent<AstarPath>().Scan();
    }

    // Update is called once per frame
    void Update()
    {
        //pathfind.GetComponent<AstarPath>().Scan();

        if (currentState == States.EXPLORATION && TurnManager.currentTurnState == TurnManager.TurnStates.FINISH)
        {
            TurnManager.currentTurnState = TurnManager.TurnStates.WAIT;
            tileManager.HideGrid();
        }
        if (currentState == States.SELECT && (TurnManager.currentTurnState == TurnManager.TurnStates.INIT || TurnManager.currentTurnState == TurnManager.TurnStates.FINISH))
        {
            tileManager.UpdateGrid(turnManager.GetNextTurn());
        }
        if (Input.GetMouseButtonDown(0))
        {   
           if(currentState == States.EXPLORATION)
            {
                tileManager.MovePlayer(TileManager.playerInstance.Count-1);
            }

           if(currentState == States.MOVE)
            {
                if(TurnManager.currentObjectTurn.tag == "Player")
                {
                    TurnManager.currentTurnState = TurnManager.TurnStates.EXECUTED;
                    if(TileManager.playerInstance.Count > 1)
                    {
                        tileManager.MovePlayer(TurnManager.currentObjectTurn.GetComponent<PlayerController>().playerNumber);
                    }
                    else
                    {
                        tileManager.MovePlayer(0);
                    }
                }
                
            }
            
        }

        if(refreshPath)
        {
            pathfind.GetComponent<AstarPath>().Scan();
            refreshPath = false;
        }

        if (currentState == States.MOVE && TurnManager.currentObjectTurn.tag == "Enemy" && TurnManager.currentTurnState == TurnManager.TurnStates.EXECUTE)
        {
            TurnManager.currentTurnState = TurnManager.TurnStates.EXECUTED;
            tileManager.MoveEnemy(TurnManager.currentObjectTurn);
        }

        if (currentState == States.ENGAGE_ENEMY)
        {
            turnManager.ShowBattleImage();
            tileManager.ShowGrid();
            turnManager.CalculateTurns(TileManager.playerInstance, TileManager.enemyInstance);
            tileManager.PositionBattle();
        }

        if (currentState == States.END_MOVE && TurnManager.currentTurnState == TurnManager.TurnStates.EXECUTED)
        {
            tileManager.ResetGrid();
            //TurnManager.currentObjectTurn.GetComponent<AILerp>().canMove = false;
            //if (turnManager.IsAllTurnFinished())
            //{
            StartCoroutine(turnManager.RecalculateTurn(TileManager.playerInstance, TileManager.enemyInstance));
            //}
            //else
            //{
            //    StartCoroutine(tileManager.WaitMoves(turnManager.currentObjectTurn, States.SELECT, false, null));
            //}
            
        }
    }

    void InitLevel()
    {
        currentState = States.EXPLORATION;
        tileManager.CreateGrid(width, height);
    }

    public static void RefreshPath()
    {
        GameManager.refreshPath = true;
    }
}
