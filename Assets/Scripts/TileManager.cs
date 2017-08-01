using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour {

    
    public GameObject gridCell;
    public GameObject[] player;
    public GameObject target;
    public GameObject[] borders;
    public static int moves;

    public static List<GameObject> playerInstance;
    public static List<GameObject> enemyInstance;
    static GameObject targetInstance;
    static Tile[,] tiles;

    public static List<GameObject> tilesSelectable;

    public static Vector2[] quadInitialPoint;
    public static Vector2[] polygonInitialPoint;
    public static GameObject tileSelected = null;
    private GameObject previousTile = null;
    private static Vector3 playerBattlePosition = Vector3.zero;
    private GameObject UI;

    private void Awake()
    {
        UI = GameObject.Find("UI");
        tilesSelectable = new List<GameObject>();

        quadInitialPoint = new Vector2[] {
            new Vector2(-0.58f, 0.58f),
            new Vector2(-0.58f, -0.58f),
            new Vector2(0.58f, -0.58f),
            new Vector2(0.58f, 0.58f)
        };

        polygonInitialPoint = new Vector2[] {
            new Vector2(-1.18f, 0f),
            new Vector2(0f, -1.18f),
            new Vector2(1.18f, 0f),
            new Vector2(0f, 1.18f)
        };
    }

    public void SetTrigger(GameObject tile)
    {

        if (!tile.GetComponent<Tile>().isChecked)
        {
            tileSelected = tile;
            tile.layer = LayerMask.NameToLayer("GridBattle");
            tile.GetComponent<Tile>().isChecked = true;

            Vector2[] newPoints = {
                new Vector2(
                    polygonInitialPoint[0].x*moves,
                    polygonInitialPoint[0].y*moves),
                new Vector2(
                    polygonInitialPoint[1].x*moves,
                    polygonInitialPoint[1].y*moves),
                new Vector2(
                    polygonInitialPoint[2].x*moves,
                    polygonInitialPoint[2].y*moves),
                new Vector2(
                    polygonInitialPoint[3].x*moves,
                    polygonInitialPoint[3].y*moves)
            };

            tile.GetComponent<PolygonCollider2D>().SetPath(0, newPoints);
            tile.GetComponent<PolygonCollider2D>().isTrigger = true;
        }
    }

    public static void SetTrigger(GameObject tile, Vector2[] newPoints)
    {

        tileSelected = tile;
        tile.layer = LayerMask.NameToLayer("GridBattle");
        tile.GetComponent<Tile>().isChecked = true;
        if (newPoints.Length == 4)
        {
            tile.GetComponent<PolygonCollider2D>().SetPath(0, newPoints);
            tile.GetComponent<PolygonCollider2D>().isTrigger = true;
        } else if(newPoints.Length == 8)
        {
            Vector2[] horizontalPoints = new Vector2[4];
            for(int i = 0; i<4; i++)
            {
                horizontalPoints[i] = newPoints[i];
            }
            Vector2[] verticalPoints = new Vector2[4];
            for (int i = 0; i < 4; i++)
            {
                verticalPoints[i] = newPoints[i+4];
            }
            tile.GetComponent<PolygonCollider2D>().pathCount = 2;
            tile.GetComponent<PolygonCollider2D>().SetPath(0, horizontalPoints);
            tile.GetComponent<PolygonCollider2D>().SetPath(1, verticalPoints);
            tile.GetComponent<PolygonCollider2D>().isTrigger = true;
        }

    }
    
    public void ShowGrid()
    {
        foreach(Tile tile in tiles)
        {
            tile.GetComponent<Tile>().ResetSpriteImage();
            tile.TileObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0.2f);
        }
    }

    public void HideGrid()
    {
        AstarPath.active.graphs[0].GetGridGraph().collision.mask = AstarPath.active.graphs[0].GetGridGraph().collision.mask - (LayerMask) Mathf.Pow(2, LayerMask.NameToLayer("GridMap"));
        foreach (GameObject player in playerInstance)
        {
            player.GetComponent<AILerp>().canMove = true;
            player.GetComponent<PlayerController>().StopFightAnimation();
        }
        foreach (Tile tile in tiles)
        {
            tile.TileObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0f);
        }
        GameManager.RefreshPath();
    }

    public void MovePlayer(int playerNumber)
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (GameManager.currentState == GameManager.States.EXPLORATION)
        {
            if (hit.collider != null && hit.collider.tag == "Tile" && !hit.collider.GetComponent<Tile>().isObstacle 
                || (hit.collider != null && hit.collider.tag == "Totem"))
            {
                Destroy(targetInstance);
                targetInstance = Instantiate(target);
                targetInstance.transform.position = hit.collider.transform.position;

                if (previousTile != null)
                {
                    if (previousTile.tag == "Tile")
                    {
                        previousTile.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0f);
                    }
                    else if (previousTile.tag == "Enemy")
                    {
                        previousTile.GetComponent<EnemyController>().EnemyTile.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0f);
                    }
                }

                if(hit.collider.tag == "Tile")
                {
                    previousTile = hit.collider.gameObject;
                    hit.collider.GetComponent<SpriteRenderer>().color = Color.blue;
                }
                else if (hit.collider.tag == "Enemy")
                {
                    previousTile = hit.collider.gameObject;
                    hit.collider.GetComponent<EnemyController>().EnemyTile.GetComponent<SpriteRenderer>().color = Color.blue;
                }

                List<RaycastHit2D[]> nearTiles = null;
                Vector3 position = targetInstance.transform.position;

                for (int i=0; i<playerInstance.Count; i++)
                {

                    if(i!=0)
                    {
                        nearTiles = new List<RaycastHit2D[]>(4)
                        {
                            Physics2D.RaycastAll(position, new Vector2(-1, 0), 1f, 1 << LayerMask.NameToLayer("GridMap")),
                            Physics2D.RaycastAll(position, new Vector2(1, 0), 1f, 1 << LayerMask.NameToLayer("GridMap")),
                            Physics2D.RaycastAll(position, new Vector2(0, 1), 1f, 1 << LayerMask.NameToLayer("GridMap")),
                            Physics2D.RaycastAll(position, new Vector2(0, -1), 1f, 1 << LayerMask.NameToLayer("GridMap"))
                        };

                        foreach (RaycastHit2D[] nearTile in nearTiles)
                        {
                            bool found = false;
                            foreach (RaycastHit2D tile in nearTile)
                            {
                                if (tile && tile.collider.gameObject.transform.position != position 
                                    && tile.collider.tag == "Tile" && !tile.collider.GetComponent<Tile>().isObstacle)
                                {
                                    playerInstance[i].GetComponent<AILerp>().target = tile.transform;
                                    position = tile.transform.position;
                                    found = true;
                                    break;
                                }
                            }
                            if(found)
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        playerInstance[i].GetComponent<AILerp>().target = targetInstance.transform;
                    }
                }

                StartCoroutine(RotatePlayer());

            }

            Camera.main.GetComponent<CameraManager>().player = playerInstance[playerNumber];
        }
        else if (GameManager.currentState == GameManager.States.MOVE)
        {
            Destroy(targetInstance);
            if (hit.collider != null && hit.collider.tag == "Tile" && hit.collider.GetComponent<Tile>().isSelected && !hit.collider.GetComponent<Tile>().isEnemy)
            {
                playerInstance[playerNumber].GetComponent<AILerp>().target = hit.collider.transform;
                playerInstance[playerNumber].GetComponent<PlayerController>().PlayerTile = hit.collider.gameObject;
                PlayerController.canMove = false;
                StartCoroutine(WaitMoves(playerInstance[playerNumber], GameManager.States.END_MOVE, false , null));
            }
        }
    }

    IEnumerator RotatePlayer()
    {
        yield return new WaitForSeconds(0.35f);
        foreach (GameObject player in playerInstance)
        {
            if (targetInstance.transform.position.x < player.transform.position.x &&
                player.transform.eulerAngles.y == 0f)
            {
                player.transform.eulerAngles = new Vector3(0f, 180f);
            }

            if (targetInstance.transform.position.x > player.transform.position.x &&
                player.transform.eulerAngles.y == 180f)
            {
                player.transform.eulerAngles = new Vector3(0f, 0f);
            }
        }
    }

    public static bool CheckEnemy()
    {
        foreach(GameObject enemy in enemyInstance)
        {
            if (tilesSelectable.Contains(enemy.GetComponent<EnemyController>().EnemyTile))
            {
                return true;
            }
        }
        return false;
    }

    public bool AttackEnemy(int playerNumber)
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null && hit.collider.tag == "Enemy" ||
                (hit.collider != null && hit.collider.tag == "Tile" && hit.collider.GetComponent<Tile>().isSelected && hit.collider.GetComponent<Tile>().isEnemy))
        {
            GameObject enemyTarget = null;
            foreach (GameObject enemy in enemyInstance)
            {
                if (enemy.GetComponent<EnemyController>().EnemyTile.transform.position == hit.collider.transform.position)
                {
                    enemyTarget = enemy;
                    break;
                }
            }
            if (playerInstance[playerNumber].GetComponent<PlayerController>().playerBehaviour == PlayerController.PlayerType.RANGED
                && tilesSelectable.Contains(enemyTarget.GetComponent<EnemyController>().EnemyTile) || Vector2.Distance(playerInstance[playerNumber].transform.position, enemyTarget.transform.position) < 1.5f)
            {
                Debug.Log("Ciao");
                playerInstance[playerNumber].GetComponent<PlayerController>().PhysicAttack(enemyTarget);
                StartCoroutine(WaitMoves(playerInstance[playerNumber], GameManager.States.END_MOVE, true, enemyTarget));
            }
            //else
            //{
            //    GameObject tileNearEnemy = enemyTarget.GetComponent<EnemyController>().GetTileNearEnemy();
            //    playerInstance[playerNumber].GetComponent<AILerp>().target = tileNearEnemy.transform;
            //    playerInstance[playerNumber].GetComponent<PlayerController>().PlayerTile = tileNearEnemy;
            //    playerInstance[playerNumber].GetComponent<PlayerController>().PhysicAttack(enemyTarget);
            //    StartCoroutine(WaitMoves(playerInstance[playerNumber], GameManager.States.END_MOVE, true, enemyTarget));
            //}

            return true;
        }
        else
        {
            return false;
        }
    }

    public void UpdateGrid(GameObject objectTurn, bool move)
    {
        Destroy(targetInstance);
        tilesSelectable.Clear();

        if (move)
        {
            moves = objectTurn.GetComponent<ObjectController>().moves;
            if (objectTurn.tag == "Player")
            {
                objectTurn.GetComponent<AILerp>().target = null;
                Tile.tileColor = objectTurn.GetComponent<PlayerController>().colorTile;
                SetTrigger(objectTurn.GetComponent<PlayerController>().PlayerTile);

            }
            else if (objectTurn.tag == "Enemy")
            {
                Tile.tileColor = objectTurn.GetComponent<EnemyController>().colorTile;
                SetTrigger(objectTurn.GetComponent<EnemyController>().EnemyTile);
            }

            objectTurn.GetComponent<AILerp>().canMove = true;
            StartCoroutine(WaitMoves(objectTurn, GameManager.States.MOVE, false, null));
        }
        else
        {
            moves = objectTurn.GetComponent<ObjectController>().combatMoves;
            if (objectTurn.tag == "Player")
            {
                objectTurn.GetComponent<AILerp>().target = null;
                Tile.tileColor = Color.red;
                SetTrigger(objectTurn.GetComponent<PlayerController>().PlayerTile);

            }
            else if (objectTurn.tag == "Enemy")
            {
                Tile.tileColor = objectTurn.GetComponent<EnemyController>().colorTile;
                SetTrigger(objectTurn.GetComponent<EnemyController>().EnemyTile);
            }

            objectTurn.GetComponent<AILerp>().canMove = true;
            StartCoroutine(WaitMoves(objectTurn, GameManager.States.FIGHT, false, null));
        }
        
    }

    public static void ResetGrid()
    {
        tileSelected.GetComponent<Tile>().isChecked = false;
        tileSelected.layer = LayerMask.NameToLayer("GridMap");
        foreach (GameObject tileObj in tilesSelectable)
        {
            tileObj.layer = LayerMask.NameToLayer("GridMap");
            tileObj.GetComponent<PolygonCollider2D>().SetPath(0, quadInitialPoint);
            tileObj.GetComponent<Tile>().isChecked = false;
            tileObj.GetComponent<Tile>().isSelected = false;
            tileObj.GetComponent<Tile>().ResetSpriteImage();
            tileObj.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0.2f);
        }
        tilesSelectable.Clear();
    }

    public void MoveEnemy(GameObject enemy)
    {
        StartCoroutine(WaitListTile(enemy));
    }

    public void PositionBattle()
    {
        StartCoroutine(StartBattle());

        Vector3 startPointMelee = new Vector3(playerBattlePosition.x, playerBattlePosition.y);
        Vector3 startPointRanged = new Vector3(playerBattlePosition.x, playerBattlePosition.y);
        bool positive = false;
        Vector3[] positions = new Vector3[playerInstance.Count];

        for (int i=0; i<playerInstance.Count; i++)
        {
            playerInstance[i].GetComponent<PlayerController>().StartFightAnimation();
            if (playerInstance[i].GetComponent<PlayerController>().playerBehaviour == PlayerController.PlayerType.MELEE)
            {
                startPointMelee = new Vector3(startPointMelee.x - (1.2f * 2), startPointMelee.y);
                positions[i] = startPointMelee;
            }
            else if(playerInstance[i].GetComponent<PlayerController>().playerBehaviour == PlayerController.PlayerType.RANGED)
            {
                if(positive)
                {
                    startPointRanged = new Vector3(playerBattlePosition.x - (1.2f * 3), playerBattlePosition.y - (1.2f * 2));
                    positions[i] = startPointRanged;
                }
                else
                {
                    startPointRanged = new Vector3(playerBattlePosition.x - (1.2f * 3), playerBattlePosition.y + (1.2f * 2));
                    positions[i] = startPointRanged;
                    positive = true;
                }
                
            }

            foreach (Tile cell in tiles)
            {
                if (cell.transform.position == positions[i])
                {
                    playerInstance[i].GetComponent<AILerp>().target = cell.TileObject.transform;
                    playerInstance[i].GetComponent<PlayerController>().PlayerTile = cell.TileObject;
                    break;
                }
            }
        }

    }

    public static void AddEnemy(GameObject enemyGroup)
    {
        enemyGroup.GetComponent<BoxCollider2D>().enabled = false;
        for (int i = 0; i < enemyGroup.transform.childCount; i++)
        {
            GameObject enemy = enemyGroup.transform.GetChild(i).gameObject;
            if(i==0)
            {
                playerBattlePosition = new Vector3(enemy.transform.position.x, -0.52f);
            }
            enemy.GetComponent<BoxCollider2D>().enabled = true;
            enemy.GetComponent<EnemyController>().position = i;
            enemy.GetComponent<EnemyController>().StartFightAnimation();
            enemyInstance.Add(enemy);
        }
        
        GameManager.currentState = GameManager.States.ENGAGE_ENEMY;
    }

    public void CreateGrid(int width, int height)
    {
        tiles = new Tile[width, height];

        if(playerInstance == null || playerInstance.Count <=0)
        {
            playerInstance = new List<GameObject>();
        }
        
        enemyInstance = new List<GameObject>();

        //Create the parent game object
        GameObject grid = new GameObject("Grid");

        //Get the tile size
        float sizeX = gridCell.GetComponent<SpriteRenderer>().bounds.size.x;
        float sizeY = gridCell.GetComponent<SpriteRenderer>().bounds.size.y;

        GameObject pathfind = GameObject.FindGameObjectWithTag("Pathfind");
        grid.transform.position = new Vector3(pathfind.transform.position.x + sizeX / 2, pathfind.transform.position.y + sizeY / 2);

        GameObject player1 = null;
        GameObject player2 = null;
        GameObject player3 = null;
        GameObject player4 = null;

        //Populate with grid cell
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                
                GameObject tileInstance = Instantiate(gridCell);
                tileInstance.transform.parent = grid.transform;
                tileInstance.transform.position = new Vector3(tileInstance.transform.position.x + (sizeX * x) + grid.transform.position.x, tileInstance.transform.position.y + (sizeY * y) + grid.transform.position.y);
                tiles[x, y] = tileInstance.GetComponent<Tile>();
                tiles[x, y].Position = tileInstance.transform.position;
                tiles[x, y].ArrayX = x;
                tiles[x, y].ArrayY = y;
                tiles[x, y].TileObject = tileInstance;

                if(y==0 || y == height-1)
                {
                    tiles[x, y].isWalkable = false;
                    tiles[x, y].isObstacle = true;
                    int rnd = Random.Range(0, borders.Length);
                    if(y==0)
                    {
                        Instantiate(borders[rnd], tileInstance.transform).GetComponent<SpriteRenderer>().sortingOrder = 99;
                    }
                    else
                    {
                        Instantiate(borders[rnd], tileInstance.transform);
                    }
                    
                }

                if (y!=0 && y != height-1 && x ==0 || x == width-1)
                {
                    tiles[x, y].isWalkable = false;
                    tiles[x, y].isObstacle = true;
                    int rnd = Random.Range(0, borders.Length);
                    Instantiate(borders[rnd], tileInstance.transform);
                }

                if (x==1 && y==4)
                {
                    player4 = Instantiate(player[3]);
                    player4.transform.position = tileInstance.transform.position;
                    player4.GetComponent<PlayerController>().PlayerTile = tileInstance;
                    player4.GetComponent<PlayerController>().playerNumber = 3;
                    player4.GetComponent<PlayerController>().playerUI = Instantiate(player4.GetComponent<PlayerController>().playerUI, UI.transform);
                }
                
                if(x==2 && y==4)
                {

                    player3 = Instantiate(player[2]);
                    player3.transform.position = tileInstance.transform.position;
                    player3.GetComponent<PlayerController>().PlayerTile = tileInstance;
                    player3.GetComponent<PlayerController>().playerNumber = 2;
                    player3.GetComponent<PlayerController>().playerUI = Instantiate(player3.GetComponent<PlayerController>().playerUI, UI.transform);
                }

                if (x==3 && y==4)
                {

                    player2 = Instantiate(player[1]);
                    player2.transform.position = tileInstance.transform.position;
                    player2.GetComponent<PlayerController>().PlayerTile = tileInstance;
                    player2.GetComponent<PlayerController>().playerNumber = 1;
                    player2.GetComponent<PlayerController>().playerUI = Instantiate(player2.GetComponent<PlayerController>().playerUI, UI.transform);
                }

                if (x==4 && y==4)
                {

                    player1 = Instantiate(player[0]);
                    player1.transform.position = tileInstance.transform.position;
                    player1.GetComponent<PlayerController>().PlayerTile = tileInstance;
                    player1.GetComponent<PlayerController>().playerNumber = 0;
                    player1.GetComponent<PlayerController>().playerUI = Instantiate(player1.GetComponent<PlayerController>().playerUI, UI.transform);
                }
            }
        }

        if(playerInstance.Count > 0)
        {
            bool p2 = false;
            bool p3 = false;
            bool p4 = false;

            foreach (GameObject player in playerInstance)
            {
                if(player.name == player1.name)
                {
                    player1.GetComponent<ObjectController>().mind = player.GetComponent<ObjectController>().mind;
                    player1.GetComponent<ObjectController>().constitution = player.GetComponent<ObjectController>().constitution;
                    player1.GetComponent<ObjectController>().skill = player.GetComponent<ObjectController>().skill;
                    player1.GetComponent<ObjectController>().strength = player.GetComponent<ObjectController>().strength;
                    player1.GetComponent<ObjectController>().CalculateStatistics();
                    Destroy(player);
                }
                else if (player.name == player2.name)
                {
                    player2.GetComponent<ObjectController>().mind = player.GetComponent<ObjectController>().mind;
                    player2.GetComponent<ObjectController>().constitution = player.GetComponent<ObjectController>().constitution;
                    player2.GetComponent<ObjectController>().skill = player.GetComponent<ObjectController>().skill;
                    player2.GetComponent<ObjectController>().strength = player.GetComponent<ObjectController>().strength;
                    player2.GetComponent<ObjectController>().CalculateStatistics();
                    Destroy(player);
                    p2 = true;
                }
                else if (player.name == player3.name)
                {
                    player3.GetComponent<ObjectController>().mind = player.GetComponent<ObjectController>().mind;
                    player3.GetComponent<ObjectController>().constitution = player.GetComponent<ObjectController>().constitution;
                    player3.GetComponent<ObjectController>().skill = player.GetComponent<ObjectController>().skill;
                    player3.GetComponent<ObjectController>().strength = player.GetComponent<ObjectController>().strength;
                    player3.GetComponent<ObjectController>().CalculateStatistics();
                    Destroy(player);
                    p3 = true;
                }
                else if (player.name == player4.name)
                {
                    player4.GetComponent<ObjectController>().mind = player.GetComponent<ObjectController>().mind;
                    player4.GetComponent<ObjectController>().constitution = player.GetComponent<ObjectController>().constitution;
                    player4.GetComponent<ObjectController>().skill = player.GetComponent<ObjectController>().skill;
                    player4.GetComponent<ObjectController>().strength = player.GetComponent<ObjectController>().strength;
                    player4.GetComponent<ObjectController>().CalculateStatistics();
                    Destroy(player);
                    p4 = true;
                }
            }
            playerInstance.Clear();
            playerInstance.Add(player1);
            if(p2)
            {
                playerInstance.Add(player2);
            }
            else
            {
                Destroy(player2.GetComponent<PlayerController>().playerUI);
                UI.GetComponent<UIManager>().DisablePlayerHUD(player2.GetComponent<PlayerController>().playerNumber);
                Destroy(player2);
            }
            if(p3)
            {
                playerInstance.Add(player3);
            }
            else
            {
                Destroy(player3.GetComponent<PlayerController>().playerUI);
                UI.GetComponent<UIManager>().DisablePlayerHUD(player3.GetComponent<PlayerController>().playerNumber);
                Destroy(player3);
            }
            if(p4)
            {
                playerInstance.Add(player4);
            }
            else
            {
                Destroy(player4.GetComponent<PlayerController>().playerUI);
                UI.GetComponent<UIManager>().DisablePlayerHUD(player4.GetComponent<PlayerController>().playerNumber);
                Destroy(player4);
            }
        }
        else
        {
            playerInstance.Add(player1);
            playerInstance.Add(player2);
            playerInstance.Add(player3);
            playerInstance.Add(player4);
        }

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            RaycastHit2D tile = Physics2D.Raycast(enemy.transform.position, new Vector2(0, 1), 0.5f, 1 << LayerMask.NameToLayer("GridMap"));

            if (tile.collider.tag == "Tile")
            {
                enemy.transform.position = tile.collider.gameObject.transform.position;
                enemy.GetComponent<EnemyController>().EnemyTile = tile.collider.gameObject;
            }

        }

        foreach (GameObject obstacle in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            List<RaycastHit2D> hits = new List<RaycastHit2D>(4)
            {
                Physics2D.Raycast(obstacle.transform.position, new Vector2(0, 1), 1f, 1 << LayerMask.NameToLayer("GridMap")),
                Physics2D.Raycast(obstacle.transform.position, new Vector2(0, -1), 1f, 1 << LayerMask.NameToLayer("GridMap")),
                Physics2D.Raycast(obstacle.transform.position, new Vector2(1, 0), 1f, 1 << LayerMask.NameToLayer("GridMap")),
                Physics2D.Raycast(obstacle.transform.position, new Vector2(-1, 0), 1f, 1 << LayerMask.NameToLayer("GridMap"))
            };

            foreach(RaycastHit2D tile in hits)
            {
                if (tile && tile.collider.tag == "Tile")
                {
                    tile.collider.GetComponent<Tile>().isObstacle = true;
                }
            }
        }
    }

    // Get the camera bounds
    public Bounds OrthographicBounds(Camera camera)
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = camera.orthographicSize * 2;
        Bounds bounds = new Bounds(
            camera.transform.position,
            new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
        return bounds;
    }

    public static IEnumerator WaitMovesAbility(GameObject mover)
    {
        GameManager.currentState = GameManager.States.ABILITY;

        while (tilesSelectable.Count == 0)
        {
            yield return null;
        }

        tileSelected.GetComponent<PolygonCollider2D>().isTrigger = false;

        if (mover.tag == "Player")
        {
            Tile.movable = false;
            yield return new WaitForSeconds(0.5f);
            mover.GetComponent<PlayerController>().PlayerTile.GetComponent<PolygonCollider2D>().pathCount = 1;
            mover.GetComponent<PlayerController>().PlayerTile.GetComponent<PolygonCollider2D>().SetPath(0, quadInitialPoint);
        }
    }


    public IEnumerator WaitMoves(GameObject mover, GameManager.States nextState, bool attack, GameObject enemy)
    {
        GameManager.currentState = GameManager.States.WAIT;

        while (tilesSelectable.Count == 0)
        {
            yield return null;
        }

        tileSelected.GetComponent<PolygonCollider2D>().isTrigger = false;

        if ((nextState == GameManager.States.MOVE || nextState == GameManager.States.FIGHT) && mover.tag == "Player")
        {
            Tile.movable = false;
            yield return new WaitForSeconds(0.5f);
            mover.GetComponent<PlayerController>().PlayerTile.GetComponent<PolygonCollider2D>().pathCount = 1;
            mover.GetComponent<PlayerController>().PlayerTile.GetComponent<PolygonCollider2D>().SetPath(0, quadInitialPoint);
        }
        else
        {
            yield return new WaitForSeconds(1);
        }

        if(mover.GetComponent<AILerp>().target != null)
        {
            while (!mover.GetComponent<AILerp>().targetReached) //&& mover.GetComponent<AILerp>().canMove)
            {
                yield return null;
            }

        }

        yield return new WaitForSeconds(0.5f);

        if (nextState == GameManager.States.MOVE && mover.tag == "Player")
        {
            Tile.movable = true;
        }

        if (mover.tag == "Player")
        {
            TurnManager.currentTurnState = TurnManager.TurnStates.EXECUTED;
            GameManager.RefreshPath();
        }

        GameManager.currentState = nextState;
    }

    private IEnumerator WaitListTile(GameObject enemy)
    {
        GameManager.currentState = GameManager.States.WAIT;

        yield return new WaitForSeconds(0.5f);

        while (tilesSelectable.Count == 0)
        {
            yield return null;
        }

        tileSelected.GetComponent<PolygonCollider2D>().isTrigger = false;
        GameManager.RefreshPath();
        enemy.GetComponent<EnemyController>().EnemyTile.GetComponent<PolygonCollider2D>().SetPath(0, quadInitialPoint);
        enemy.GetComponent<EnemyController>().EnemyIA(playerInstance, tilesSelectable);

        if (enemy.GetComponent<EnemyController>().canAttack)
        {
            StartCoroutine(WaitMoves(enemy, GameManager.States.END_MOVE, true, enemy.GetComponent<EnemyController>().playerAttacked));
            enemy.GetComponent<EnemyController>().PhysicAttack(enemy.GetComponent<EnemyController>().playerAttacked);
        }
        else
        {
            StartCoroutine(WaitMoves(enemy, GameManager.States.END_MOVE, false, null));
        }
    }

    public IEnumerator StartBattle()
    {
        GameManager.currentState = GameManager.States.WAIT;
        yield return new WaitForSeconds(1.5f);

        foreach (GameObject player in playerInstance)
        {
            while (!player.GetComponent<AILerp>().targetReached)
            {
                yield return null;
            }
        }

        yield return new WaitForSeconds(0.8f);

        AstarPath.active.graphs[0].GetGridGraph().collision.mask = AstarPath.active.graphs[0].GetGridGraph().collision.mask + (LayerMask)Mathf.Pow(2, LayerMask.NameToLayer("GridMap"));

        foreach (GameObject player in playerInstance)
        {
            player.GetComponent<AILerp>().target = null;
        }

        GameManager.currentState = GameManager.States.SELECT;
    }
}
