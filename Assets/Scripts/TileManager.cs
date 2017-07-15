using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour {

    
    public GameObject gridCell;
    public GameObject[] player;
    public GameObject target;
    public GameObject[] enemy;
    public static List<GameObject> tileListCollision;
    public static int moveCount;

    public static GameObject[] playerInstance;
    public static GameObject[] enemyInstance;
    static GameObject targetInstance;
    Tile[,] tiles;
    

    public static void SetTrigger(GameObject tileObj)
    {
        if(moveCount > 0)
        {
            moveCount--;
            tileObj.GetComponent<Tile>().isChecked = true;
            Debug.Log("POSTION TILE UNO : " + tileObj.GetComponent<Tile>().ArrayX + "  " + tileObj.GetComponent<Tile>().ArrayY);
            tileObj.GetComponent<Tile>().isWalkable = true;
            tileObj.GetComponent<BoxCollider2D>().isTrigger = true;
        }
    }

    public static void SetTrigger(List<GameObject> tileObjs)
    {
        if (moveCount > 0)
        {
            moveCount--;
            foreach (GameObject tileObj in tileObjs)
            {
                tileObj.GetComponent<Tile>().isChecked = true;
                tileObj.GetComponent<Tile>().isWalkable = true;
                tileObj.GetComponent<BoxCollider2D>().isTrigger = true;
            }
        }
        else
        {
            foreach (GameObject tileObj in tileObjs)
            {
                tileObj.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
    }

    public void ShowGrid()
    {
        foreach(Tile tile in tiles)
        {
            tile.TileObject.GetComponent<SpriteRenderer>().enabled = true;
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
            if (hit.collider != null && hit.collider.tag == "Tile" && hit.collider.GetComponent<Tile>().isWalkable)
            {
                Destroy(targetInstance);
                targetInstance = Instantiate(target, hit.collider.transform);
                playerInstance[playerNumber].GetComponent<AILerp>().enabled = true;
                playerInstance[playerNumber].GetComponent<AILerp>().target = targetInstance.transform;
                playerInstance[playerNumber].GetComponent<PlayerController>().playerTile = hit.collider.gameObject;
                GameManager.currentState = GameManager.States.END_MOVE;
            }
        }
    }

    public void UpdateGrid(GameObject objectTurn)
    {
        Destroy(targetInstance);
        TileManager.tileListCollision.Clear();

        if(objectTurn.tag == "Player")
        {
            moveCount = objectTurn.GetComponent<PlayerController>().pointAction;
            SetTrigger(objectTurn.GetComponent<PlayerController>().playerTile);
        }
        else if(objectTurn.tag == "Enemy")
        {

        }

        GameManager.currentState = GameManager.States.MOVE;
    }

    public void ResetGrid()
    {
        foreach (GameObject tileObj in tileListCollision)
        {
            tileObj.GetComponent<Tile>().isChecked = false;
            tileObj.GetComponent<Tile>().isWalkable = false;
            tileObj.GetComponent<BoxCollider2D>().isTrigger = false;
            tileObj.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public void PositionBattle()
    {
        foreach(GameObject player in playerInstance)
        {
            player.GetComponent<AILerp>().enabled = false;
            player.GetComponent<AILerp>().target = null;
        }

        playerInstance[1].transform.parent = null;
        playerInstance[0].transform.position = tiles[3, 3].Position;
        playerInstance[0].GetComponent<PlayerController>().playerTile = tiles[3, 3].TileObject;
        playerInstance[1].transform.position = tiles[1, 3].Position;
        playerInstance[1].GetComponent<PlayerController>().playerTile = tiles[1, 3].TileObject;
        enemyInstance[0].transform.position = tiles[6, 3].Position;

        GameManager.currentState = GameManager.States.SELECT;
    }

    public void CreateGrid(int width, int height)
    {
        tileListCollision = new List<GameObject>();
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
                }

                if(x==0 && y==0)
                {
                    playerInstance[1] = Instantiate(player[1]);
                    playerInstance[1].transform.position = new Vector3(grid.transform.position.x, grid.transform.position.y);
                    playerInstance[1].GetComponent<PlayerController>().playerTile = tileInstance;
                    playerInstance[1].GetComponent<PlayerController>().playerNumber = 1;
                    
                }
                
                if(x==1 && y==0)
                {
                    playerInstance[0] = Instantiate(player[0]);
                    playerInstance[0].transform.position = tileInstance.transform.position;
                    playerInstance[0].GetComponent<PlayerController>().playerTile = tileInstance;
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
