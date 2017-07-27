using UnityEngine;

public class GameManager : MonoBehaviour {

    public int height;
    public int width;
    public int maxPointAction;
    private int pointAction;

    public enum States
    {
        EXPLORATION,
        ENGAGE_ENEMY,
        SELECT,
        MOVE,
        PRE_FIGHT,
        FIGHT,
        ATTACK,
        END_MOVE,
        WAIT
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
            TurnManager.currentTurnState = TurnManager.TurnStates.WAIT;
            tileManager.HideGrid();
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
                tileManager.UpdateGrid(TurnManager.currentObjectTurn, true);
            }
        }

        if(TurnManager.currentObjectTurn && TurnManager.currentObjectTurn.tag == "Player" && Input.GetKeyDown(KeyCode.Space) 
            && (currentState == States.MOVE || currentState == States.FIGHT))
        {
            pointAction = maxPointAction;
            tileManager.ResetGrid();
            currentState = States.SELECT;
            TurnManager.currentTurnState = TurnManager.TurnStates.INIT;
            PlayerController.canMove = true;
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
                if(TurnManager.currentObjectTurn && TurnManager.currentObjectTurn.tag == "Player" && tileManager.CheckEnemy())
                {
                    if(!tileManager.AttackEnemy(TurnManager.currentObjectTurn.GetComponent<PlayerController>().playerNumber) && PlayerController.canMove)
                    {
                        tileManager.ResetGrid();
                        tileManager.UpdateGrid(TurnManager.currentObjectTurn, true);
                    }
                }
                else if(TurnManager.currentObjectTurn && TurnManager.currentObjectTurn.tag == "Player" && PlayerController.canMove)
                {
                    tileManager.ResetGrid();
                    tileManager.UpdateGrid(TurnManager.currentObjectTurn, true);
                }
           }
        }

        if (TurnManager.currentObjectTurn && TurnManager.currentObjectTurn.tag == "Player" && (Input.GetMouseButtonDown(1) && currentState == States.MOVE) || currentState == States.PRE_FIGHT)
        {
            currentState = States.FIGHT;
            if (TurnManager.currentObjectTurn.tag == "Player")
            {
                tileManager.ResetGrid();
                tileManager.UpdateGrid(TurnManager.currentObjectTurn, false);
            }
        }

        if (refreshPath)
        {
            
            refreshPath = false;
        }

        if (TurnManager.currentObjectTurn && TurnManager.currentObjectTurn.tag == "Enemy")
        {
            if(currentState == States.MOVE && TurnManager.currentTurnState == TurnManager.TurnStates.EXECUTE)
            {
                TurnManager.currentTurnState = TurnManager.TurnStates.EXECUTED;
                tileManager.MoveEnemy(TurnManager.currentObjectTurn);
            }
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
            if(pointAction <=0)
            {
                pointAction = maxPointAction;
                TurnManager.currentObjectTurn.GetComponent<AILerp>().canMove = false;
                tileManager.ResetGrid();
                if (TurnManager.currentObjectTurn.tag == "Player")
                {
                    PlayerController.canMove = true;
                }
                else if(TurnManager.currentObjectTurn.tag == "Enemy")
                {
                    EnemyController.hasMoved = true;
                }
                StartCoroutine(turnManager.RecalculateTurn(TileManager.playerInstance, TileManager.enemyInstance, States.SELECT, TurnManager.TurnStates.INIT));
                turnManager.ResetTurnColor();
            }
            else
            {
                tileManager.ResetGrid();
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

    public static void RefreshPath()
    {
        pathfind.GetComponent<AstarPath>().Scan();
        //GameManager.refreshPath = true;
    }
}
