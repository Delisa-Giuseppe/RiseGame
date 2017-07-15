using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour {

    
    public GameObject gridCell;
    public GameObject player;
    public GameObject target;
    public static List<GameObject> tileListCollision;
    public static int moveCount;

    static GameObject playerInstance;
    static GameObject targetInstance;
    Tile[,] tiles;
    

    public static void SetTrigger(GameObject tileObj)
    {
        if(moveCount > 0)
        {
            moveCount--;
            tileObj.GetComponent<Tile>().isChecked = true;
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
        GameManager.currentState = GameManager.States.MOVE;
    }

    public void MovePlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider != null && hit.collider.tag == "Tile")
        {
            Destroy(targetInstance);
            targetInstance = Instantiate(target, hit.collider.transform);
            playerInstance.GetComponent<AILerp>().target = targetInstance.transform;
        }
    }

    public void UpdateGrid()
    {
        TileManager.tileListCollision.Clear();
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider != null && GameObject.FindGameObjectWithTag("Player") != null &&
            hit.collider.transform.position == GameObject.FindGameObjectWithTag("Player").transform.position)
        {
            TileManager.SetTrigger(hit.collider.gameObject);
        }
    }

    public void ResetGrid()
    {
        foreach (GameObject tileObj in tileListCollision)
        {
            tileObj.GetComponent<Tile>().isChecked = false;
            tileObj.GetComponent<Tile>().isWalkable = false;
            tileObj.GetComponent<BoxCollider2D>().isTrigger = false;
            tileObj.GetComponent<SpriteRenderer>().color = Color.white;

            GameManager.currentState = GameManager.States.SELECT;
        }
    }

    public void CreateGrid(int width, int height)
    {
        tileListCollision = new List<GameObject>();
        tiles = new Tile[width, height];

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
                /** TO REMOVE **/
                //if (x == 1 && y == 0)
                //{
                //    GameObject obstacleInstance = Instantiate(obstacle);
                //    obstacleInstance.transform.parent = grid.transform;
                //    obstacleInstance.transform.position = new Vector3(obstacleInstance.transform.position.x + (sizeX * x) + grid.transform.position.x, obstacleInstance.transform.position.y + (sizeY * y) + grid.transform.position.y);
                //}
                /** END TO REMOVE **/
                GameObject tileInstance = Instantiate(gridCell);
                tileInstance.transform.parent = grid.transform;
                tileInstance.transform.position = new Vector3(tileInstance.transform.position.x + (sizeX * x) + grid.transform.position.x, tileInstance.transform.position.y + (sizeY * y) + grid.transform.position.y);
                tiles[x, y] = tileInstance.GetComponent<Tile>();
                tiles[x, y].Position = tileInstance.transform.position;
                tiles[x, y].ArrayX = x;
                tiles[x, y].ArrayY = y;
                tiles[x, y].isWalkable = true;
                tiles[x, y].TileObject = tileInstance;


                //Instance of the player
                if (x == 2 && y == 2)
                {
                    playerInstance = Instantiate(player);
                    playerInstance.transform.parent = grid.transform;
                    playerInstance.transform.position = new Vector3(grid.transform.position.x, grid.transform.position.y);
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
