using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour {

    
    public GameObject gridCell;
    public GameObject[] player;
    public GameObject target;
    public GameObject[] enemy;
    //public static List<GameObject> tileListCollider;
    public static int moves;

    public static GameObject[] playerInstance;
    public static GameObject[] enemyInstance;
    static GameObject targetInstance;
    Tile[,] tiles;

    public static List<GameObject> tilesSelectable;

    private Vector2[] quadInitialPoint;
    private Vector2[] polygonInitialPoint;
    private GameObject tileSelected = null;

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
            tile.TileObject.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    public void HideGrid()
    {
        AstarPath.active.graphs[0].GetGridGraph().collision.mask = (LayerMask)Mathf.Pow(2, LayerMask.NameToLayer("World"));
        foreach(GameObject player in playerInstance)
        {
            player.GetComponent<AILerp>().canMove = true;
        }
        foreach (Tile tile in tiles)
        {
            tile.TileObject.GetComponent<SpriteRenderer>().enabled = false;
        }
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
                for(int i=0; i<playerInstance.Length; i++)
                {
                    if(i==0)
                    {
                        playerInstance[i].GetComponent<AILerp>().target = targetInstance.transform;
                    }
                    else
                    {
                        playerInstance[i].GetComponent<AILerp>().target = playerInstance[i-1].transform;
                    }
                }
            }
        }
        else if (GameManager.currentState == GameManager.States.MOVE)
        {
            playerInstance[playerNumber].GetComponent<PlayerController>().PlayerTile.GetComponent<PolygonCollider2D>().SetPath(0, quadInitialPoint);

            Destroy(targetInstance);
            if (hit.collider != null && hit.collider.tag == "Tile" && hit.collider.GetComponent<Tile>().isSelected)
            {
                playerInstance[playerNumber].GetComponent<AILerp>().target = hit.collider.transform;
                playerInstance[playerNumber].GetComponent<PlayerController>().PlayerTile = hit.collider.gameObject;
                StartCoroutine(WaitMoves(playerInstance[playerNumber], GameManager.States.END_MOVE, false , null));
            }
            else
            {
                foreach(GameObject enemy in enemyInstance)
                {
                    if(enemy.GetComponent<EnemyController>().EnemyTile.transform.position == hit.collider.transform.position)
                    {
                        GameObject tileNearEnemy = enemy.GetComponent<EnemyController>().GetTileNearEnemy();
                        playerInstance[playerNumber].GetComponent<AILerp>().target = tileNearEnemy.transform;
                        playerInstance[playerNumber].GetComponent<PlayerController>().PlayerTile = tileNearEnemy;

                        StartCoroutine(WaitMoves(playerInstance[playerNumber], GameManager.States.END_MOVE, true, enemy));

                        break;
                    }
                }

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
            moves = objectTurn.GetComponent<PlayerController>().moves;
            SetTrigger(objectTurn.GetComponent<PlayerController>().PlayerTile);
        }
        else if(objectTurn.tag == "Enemy")
        {
            moves = objectTurn.GetComponent<EnemyController>().moves;
            SetTrigger(objectTurn.GetComponent<EnemyController>().EnemyTile);
        }

        objectTurn.GetComponent<AILerp>().canMove = true;
        GameManager.currentState = GameManager.States.MOVE;
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
        yield return new WaitForSeconds(0.8f);

        if(tilesSelectable.Count == 0)
        {
            yield return null;
        }

        enemy.GetComponent<EnemyController>().EnemyTile.GetComponent<PolygonCollider2D>().SetPath(0, quadInitialPoint);
        enemy.GetComponent<EnemyController>().EnemyIA(playerInstance, tilesSelectable);
        StartCoroutine(WaitMoves(enemy, GameManager.States.END_MOVE, false, null));
    }


    public void PositionBattle()
    {
        StartCoroutine(StartBattle(playerInstance));
        playerInstance[1].transform.parent = null;
        playerInstance[0].GetComponent<AILerp>().target = tiles[1, 3].TileObject.transform;
        playerInstance[0].GetComponent<PlayerController>().PlayerTile = tiles[1, 3].TileObject;
        playerInstance[1].GetComponent<AILerp>().target = tiles[3, 3].TileObject.transform;
        playerInstance[1].GetComponent<PlayerController>().PlayerTile = tiles[3, 3].TileObject;
        enemyInstance[0].GetComponent<EnemyController>().EnemyTile = tiles[6, 3].TileObject;
        enemyInstance[0].GetComponent<AILerp>().target = tiles[6, 3].TileObject.transform;
    }
    public IEnumerator StartBattle(GameObject[] mover)
    { 
        GameManager.currentState = GameManager.States.WAIT;
        yield return new WaitForSeconds(1);

        foreach(GameObject player in mover)
        {
            while (!player.GetComponent<AILerp>().targetReached)
            {
                yield return null;
            }
        }

        GameManager.currentState = GameManager.States.SELECT;
    }


    public IEnumerator WaitMoves(GameObject mover, GameManager.States nextState, bool attack, GameObject enemy)
    {
        GameManager.currentState = GameManager.States.WAIT;
        yield return new WaitForSeconds(1);

        while (!mover.GetComponent<AILerp>().targetReached && mover.GetComponent<AILerp>().canMove)
        {
            yield return null;
        }

        if(attack)
        {
            mover.GetComponent<PlayerController>().PhysicAttack(enemy.gameObject);
        }

        yield return new WaitForSeconds(0.5f);
        GameManager.currentState = nextState;
    }

    public void CreateGrid(int width, int height)
    {
        tiles = new Tile[width, height];

        playerInstance = new GameObject[player.Length];
        enemyInstance = new GameObject[enemy.Length];

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


                if(x==3 && y==1)
                {
                    enemyInstance[0] = Instantiate(enemy[0]);
                    enemyInstance[0].transform.position = tileInstance.transform.position;
                    enemyInstance[0].GetComponent<EnemyController>().EnemyTile = tileInstance;
                    enemyInstance[0].GetComponent<EnemyController>().positionArray = 0;
                }

                if(x==0 && y==0)
                {
                    playerInstance[1] = Instantiate(player[1]);
                    playerInstance[1].transform.position = new Vector3(grid.transform.position.x, grid.transform.position.y);
                    playerInstance[1].GetComponent<PlayerController>().PlayerTile = tileInstance;
                    playerInstance[1].GetComponent<PlayerController>().playerNumber = 1;
                    
                }
                
                if(x==1 && y==0)
                {
                    playerInstance[0] = Instantiate(player[0]);
                    playerInstance[0].transform.position = tileInstance.transform.position;
                    playerInstance[0].GetComponent<PlayerController>().PlayerTile = tileInstance;
                    playerInstance[0].GetComponent<PlayerController>().playerNumber = 0;
                    playerInstance[1].transform.parent = playerInstance[0].transform;
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

}
