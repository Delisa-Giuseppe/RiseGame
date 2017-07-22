﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour {

    
    public GameObject gridCell;
    public GameObject[] player;
    public GameObject target;
    public GameObject[] enemy;
    //public static List<GameObject> tileListCollider;
    public static int moves;

    public static List<GameObject> playerInstance;
    public static List<GameObject> enemyInstance;
    static GameObject targetInstance;
    static Tile[,] tiles;

    public static List<GameObject> tilesSelectable;

    private Vector2[] quadInitialPoint;
    private Vector2[] polygonInitialPoint;
    private GameObject tileSelected = null;
    private GameObject previousTile = null;

    public TileManager()
    {
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

        //if (TileManager.tilesSelectable.Count > 0 && tileSelected != "" && tile.name != tileSelected)
        //{
        //    GameObject oldTile = GameObject.Find(tileSelected);
        //    ResetTiles(oldTile);
        //}

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

            //tilesSelectable.Add(tile);
            tile.GetComponent<PolygonCollider2D>().SetPath(0, newPoints);
            tile.GetComponent<PolygonCollider2D>().isTrigger = true;

        }
    }

    //private void ResetTiles(GameObject oldTile)
    //{
    //    Debug.Log(oldTile.name);

    //    oldTile.GetComponent<Tile>().isChecked = false;
    //    oldTile.GetComponent<PolygonCollider2D>().SetPath(0, quadInitialPoint);

    //    foreach (GameObject tile in TileManager.tilesSelectable)
    //    {
    //        tile.GetComponent<SpriteRenderer>().color = Color.white;
    //    }
    //    TileManager.tilesSelectable.Clear();

    //}

    //public void SetTrigger(GameObject tileObj)
    //{

    //    tileObj.GetComponent<Tile>().isChecked = true;
    //    tileObj.GetComponent<Tile>().isWalkable = true;
    //    float moves = moveCount;

    //    if(moveCount <= 3)
    //    {
    //        moves = moveCount * 0.5f;
    //    }

    //    List<RaycastHit2D[]> hits = new List<RaycastHit2D[]>(4)
    //    {
    //        Physics2D.RaycastAll(tileObj.GetComponent<Tile>().Position, new Vector2(0, 1), moves, 1 << LayerMask.NameToLayer("GridMap")),
    //        Physics2D.RaycastAll(tileObj.GetComponent<Tile>().Position, new Vector2(0, -1), moves, 1 << LayerMask.NameToLayer("GridMap")),
    //        Physics2D.RaycastAll(tileObj.GetComponent<Tile>().Position, new Vector2(1, 0), moves, 1 << LayerMask.NameToLayer("GridMap")),
    //        Physics2D.RaycastAll(tileObj.GetComponent<Tile>().Position, new Vector2(-1, 0), moves, 1 << LayerMask.NameToLayer("GridMap"))
    //    };

    //    for(int i=0; i < hits.Capacity; i++)
    //    {
    //        for(int x=0; x < hits[i].Length; x++)
    //        {
    //            if(hits[i][x].collider.transform.position != tileObj.transform.position)
    //            {
    //                tileListCollider.Add(hits[i][x].collider.gameObject);
    //            }
    //        }
    //    }
    //    int size = tileListCollider.Count;
    //    for (int i=0; i < size; i++)
    //    {
    //        GetNeighbour(tileListCollider[i]);
    //    }

    //    foreach (GameObject tileCollider in tileListCollider)
    //    {
    //        tileCollider.GetComponent<SpriteRenderer>().color = Color.red;
    //    }

    //}


    //public void GetNeighbour(GameObject tileObj)
    //{
    //    tileObj.GetComponent<Tile>().isChecked = true;
    //    tileObj.GetComponent<Tile>().isWalkable = true;
    //    tileListCollider.Add(tileObj);
    //    List<RaycastHit2D[]> hits = new List<RaycastHit2D[]>(4)
    //        {
    //            Physics2D.RaycastAll(tileObj.GetComponent<Tile>().Position, new Vector2(0, 1), 1f , 1 << LayerMask.NameToLayer("GridMap")),
    //            Physics2D.RaycastAll(tileObj.GetComponent<Tile>().Position, new Vector2(0, -1), 1f, 1 << LayerMask.NameToLayer("GridMap")),
    //            Physics2D.RaycastAll(tileObj.GetComponent<Tile>().Position, new Vector2(1, 0), 1f, 1 << LayerMask.NameToLayer("GridMap")),
    //            Physics2D.RaycastAll(tileObj.GetComponent<Tile>().Position, new Vector2(-1, 0), 1f, 1 << LayerMask.NameToLayer("GridMap"))
    //        };

    //    for (int i = 0; i < hits.Capacity; i++)
    //    {
    //        for (int x = 0; x < hits[i].Length; x++)
    //        {
    //            if (hits[i][x].collider.transform.position != tileObj.transform.position)
    //            {
    //                hits[i][x].collider.GetComponent<Tile>().isChecked = true;
    //                hits[i][x].collider.GetComponent<Tile>().isWalkable = true;
    //                tileListCollider.Add(hits[i][x].collider.gameObject);
    //            }
    //        }
    //    }
    //}

    public void ShowGrid()
    {
        foreach(Tile tile in tiles)
        {
            tile.TileObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 1f);
        }
    }

    public void HideGrid()
    {
        AstarPath.active.graphs[0].GetGridGraph().collision.mask = AstarPath.active.graphs[0].GetGridGraph().collision.mask - (LayerMask) Mathf.Pow(2, LayerMask.NameToLayer("GridMap"));
        foreach(GameObject player in playerInstance)
        {
            player.GetComponent<AILerp>().canMove = true;
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
            if (hit.collider != null)
            {
                Destroy(targetInstance);
                targetInstance = Instantiate(target, hit.collider.transform);

                if(previousTile != null)
                {
                    if (previousTile.tag == "Tile")
                    {
                        previousTile.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0f);
                    }
                    else if (previousTile.tag == "Enemy")
                    {
                        hit.collider.GetComponent<EnemyController>().EnemyTile.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0f);
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
                                if (tile && tile.collider.gameObject.transform.position != position && tile.collider.tag == "Tile")
                                {
                                    playerInstance[i].GetComponent<AILerp>().target = tile.transform;
                                    playerInstance[i].GetComponent<PlayerController>().PlayerTile = tile.collider.gameObject;
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
                        if(hit.collider.gameObject.tag == "Tile")
                        {
                            playerInstance[i].GetComponent<PlayerController>().PlayerTile = hit.collider.gameObject;
                        }
                    }
                }

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
                StartCoroutine(WaitMoves(playerInstance[playerNumber], GameManager.States.END_MOVE, false , null));
            }
            else if(hit.collider != null && hit.collider.tag == "Enemy" ||
                (hit.collider.tag == "Tile" && hit.collider.GetComponent<Tile>().isSelected && hit.collider.GetComponent<Tile>().isEnemy))
            {
                GameObject enemyTarget = null;
                foreach(GameObject enemy in enemyInstance)
                {
                    if(enemy.GetComponent<EnemyController>().EnemyTile.transform.position == hit.collider.transform.position)
                    {
                        enemyTarget = enemy;
                        break;
                    }
                }
                if (Vector2.Distance(playerInstance[playerNumber].transform.position, enemyTarget.transform.position) > 1.5f)
                {
                    GameObject tileNearEnemy = enemyTarget.GetComponent<EnemyController>().GetTileNearEnemy();
                    playerInstance[playerNumber].GetComponent<AILerp>().target = tileNearEnemy.transform;
                    playerInstance[playerNumber].GetComponent<PlayerController>().PlayerTile = tileNearEnemy;
                }
                
                StartCoroutine(WaitMoves(playerInstance[playerNumber], GameManager.States.END_MOVE, true, enemyTarget));

            }
        }
    }

    public void UpdateGrid(GameObject objectTurn)
    {
        Destroy(targetInstance);
        tilesSelectable.Clear();

        //objectTurn.GetComponent<Seeker>().startEndModifier.mask = LayerMask.NameToLayer("GridBattle");

        if (objectTurn.tag == "Player")
        {
            objectTurn.GetComponent<AILerp>().target = null;
            moves = objectTurn.GetComponent<PlayerController>().moves;
            SetTrigger(objectTurn.GetComponent<PlayerController>().PlayerTile);
            
        }
        else if(objectTurn.tag == "Enemy")
        {
            moves = objectTurn.GetComponent<EnemyController>().moves;
            SetTrigger(objectTurn.GetComponent<EnemyController>().EnemyTile);
        }

        objectTurn.GetComponent<AILerp>().canMove = true;
        StartCoroutine(WaitMoves(objectTurn, GameManager.States.MOVE, false, null));
        //GameManager.currentState = GameManager.States.MOVE;
    }

    public void ResetGrid()
    {
        tileSelected.GetComponent<Tile>().isChecked = false;
        tileSelected.layer = LayerMask.NameToLayer("GridMap");
        foreach (GameObject tileObj in tilesSelectable)
        {
            tileObj.layer = LayerMask.NameToLayer("GridMap");
            tileObj.GetComponent<PolygonCollider2D>().SetPath(0, quadInitialPoint);
            tileObj.GetComponent<Tile>().isChecked = false;
            tileObj.GetComponent<Tile>().isSelected = false;
            //tileObj.GetComponent<Tile>().isWalkable = false;
            tileObj.GetComponent<SpriteRenderer>().color = Color.white;
        }
        tilesSelectable.Clear();
    }

    public void MoveEnemy(GameObject enemy)
    {
        StartCoroutine(WaitListTile(enemy));
    }

    IEnumerator WaitListTile(GameObject enemy)
    {
        yield return new WaitForSeconds(1f);

        if(tilesSelectable.Count == 0)
        {
            yield return null;
        }

        enemy.GetComponent<EnemyController>().EnemyTile.GetComponent<PolygonCollider2D>().SetPath(0, quadInitialPoint);
        enemy.GetComponent<EnemyController>().EnemyIA(playerInstance, tilesSelectable);
        if(enemy.GetComponent<EnemyController>().canAttack)
        {
            StartCoroutine(WaitMoves(enemy, GameManager.States.END_MOVE, true, enemy.GetComponent<EnemyController>().playerAttacked));
        }
        else
        {
            StartCoroutine(WaitMoves(enemy, GameManager.States.END_MOVE, false, null));
        }
    }


    public void PositionBattle()
    {
        StartCoroutine(StartBattle());

        //foreach(GameObject player in playerInstance)
        //{
        //    player.GetComponent<AILerp>().target = tiles[1, 3].TileObject.transform;
        //    player.GetComponent<PlayerController>().PlayerTile = tiles[1, 3].TileObject;
        //}

        //playerInstance[0].GetComponent<AILerp>().target = tiles[1, 3].TileObject.transform;
        //playerInstance[0].GetComponent<PlayerController>().PlayerTile = tiles[1, 3].TileObject;
        //playerInstance[1].GetComponent<AILerp>().target = tiles[3, 3].TileObject.transform;
        //playerInstance[1].GetComponent<PlayerController>().PlayerTile = tiles[3, 3].TileObject;
        //playerInstance[1].GetComponent<Seeker>().startEndModifier.exactEndPoint = Pathfinding.StartEndModifier.Exactness.Original;
        //enemyInstance[0].GetComponent<EnemyController>().EnemyTile = tiles[6, 3].TileObject;
        //enemyInstance[0].GetComponent<AILerp>().target = tiles[6, 3].TileObject.transform;
        //enemyInstance[1].GetComponent<EnemyController>().EnemyTile = tiles[8, 3].TileObject;
        //enemyInstance[1].GetComponent<AILerp>().target = tiles[8, 3].TileObject.transform;
    }
    public IEnumerator StartBattle()
    { 
        GameManager.currentState = GameManager.States.WAIT;
        yield return new WaitForSeconds(1.5f);

        foreach(GameObject player in playerInstance)
        {
            while (!player.GetComponent<AILerp>().targetReached)
            {
                yield return null;
            }
        }

        yield return new WaitForSeconds(0.8f);

        AstarPath.active.graphs[0].GetGridGraph().collision.mask = AstarPath.active.graphs[0].GetGridGraph().collision.mask + (LayerMask)Mathf.Pow(2, LayerMask.NameToLayer("GridMap"));
        GameManager.currentState = GameManager.States.SELECT;
    }


    public IEnumerator WaitMoves(GameObject mover, GameManager.States nextState, bool attack, GameObject enemy)
    {
        GameManager.currentState = GameManager.States.WAIT;
        if (nextState == GameManager.States.MOVE && mover.tag == "Player")
        {
            yield return new WaitForSeconds(0.5f);
            mover.GetComponent<PlayerController>().PlayerTile.GetComponent<PolygonCollider2D>().SetPath(0, quadInitialPoint);
        }
        else
        {
            yield return new WaitForSeconds(1);
        }

        while (!mover.GetComponent<AILerp>().targetReached && mover.GetComponent<AILerp>().canMove)
        {
            yield return null;
        }

        if(attack && mover.tag == "Player")
        {
            mover.GetComponent<PlayerController>().PhysicAttack(enemy.gameObject);
        } else if(attack && mover.tag == "Enemy")
        {
            yield return new WaitForSeconds(1f);
            mover.GetComponent<EnemyController>().PhysicAttack(enemy.gameObject);
        }

        yield return new WaitForSeconds(0.5f);
        GameManager.refreshPath = true;
        GameManager.currentState = nextState;
    }

    public static void AddEnemy(GameObject enemyGroup)
    {
        for (int i = 0; i < enemyGroup.transform.childCount; i++)
        {
            GameObject enemy = enemyGroup.transform.GetChild(i).gameObject;
            enemy.GetComponent<SpriteRenderer>().enabled = true;
            enemy.GetComponent<BoxCollider2D>().enabled = true;
            enemyInstance.Add(enemy);
        }
    }

    public void CreateGrid(int width, int height)
    {
        tiles = new Tile[width, height];

        playerInstance = new List<GameObject>();
        enemyInstance = new List<GameObject>();

        //Create the parent game object
        GameObject grid = new GameObject("Grid");

        //Get the tile size
        float sizeX = gridCell.GetComponent<SpriteRenderer>().bounds.size.x;
        float sizeY = gridCell.GetComponent<SpriteRenderer>().bounds.size.y;

        //Get the lowest bound camera position for the start position of the tile
        Bounds cameraBounds = OrthographicBounds(Camera.main);
        //grid.transform.position = new Vector3(cameraBounds.min.x + sizeX / 2, cameraBounds.min.y + sizeY / 2);

        GameObject pathfind = GameObject.FindGameObjectWithTag("Pathfind");
        grid.transform.position = new Vector3(pathfind.transform.position.x + sizeX / 2, pathfind.transform.position.y + sizeY / 2);

        /** TO REMOVE **/
        int rndX = Mathf.FloorToInt(Random.Range(0, width));
        int rndY = Mathf.FloorToInt(Random.Range(0, height));
        /** END TO REMOVE **/
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


                //if(x==10 && y==1)
                //{
                //    enemyInst = Instantiate(enemy[0]);
                //    enemyInst.transform.position = tileInstance.transform.position;
                //    enemyInst.GetComponent<EnemyController>().EnemyTile = tileInstance;
                    
                //}

                //if (x == 11 && y == 1)
                //{
                //    enemyInst1 = Instantiate(enemy[1]);
                //    enemyInst1.transform.position = tileInstance.transform.position;
                //    enemyInst1.GetComponent<EnemyController>().EnemyTile = tileInstance;
                //}


                if (x==0 && y==4)
                {
                    player4 = Instantiate(player[3]);
                    player4.transform.position = tileInstance.transform.position;
                    player4.GetComponent<PlayerController>().PlayerTile = tileInstance;
                    player4.GetComponent<PlayerController>().playerNumber = 3;
                }
                
                if(x==1 && y==4)
                {

                    player3 = Instantiate(player[2]);
                    player3.transform.position = tileInstance.transform.position;
                    player3.GetComponent<PlayerController>().PlayerTile = tileInstance;
                    player3.GetComponent<PlayerController>().playerNumber = 2;
                }

                if (x == 2 && y == 4)
                {

                    player2 = Instantiate(player[1]);
                    player2.transform.position = tileInstance.transform.position;
                    player2.GetComponent<PlayerController>().PlayerTile = tileInstance;
                    player2.GetComponent<PlayerController>().playerNumber = 1;
                }

                if (x == 3 && y == 4)
                {

                    player1 = Instantiate(player[0]);
                    player1.transform.position = tileInstance.transform.position;
                    player1.GetComponent<PlayerController>().PlayerTile = tileInstance;
                    player1.GetComponent<PlayerController>().playerNumber = 0;
                }
            }
        }
        playerInstance.Add(player1);
        playerInstance.Add(player2);
        playerInstance.Add(player3);
        playerInstance.Add(player4);
        
        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            RaycastHit2D tile = Physics2D.Raycast(enemy.transform.position, new Vector2(0, 1), 1f, 1 << LayerMask.NameToLayer("GridMap"));

            if (tile.collider.tag == "Tile")
            {
                enemy.GetComponent<EnemyController>().EnemyTile = tile.collider.gameObject;
            }

        }
        
        //enemyInstance.Add(enemyInst);
        //enemyInstance.Add(enemyInst1);
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

}
