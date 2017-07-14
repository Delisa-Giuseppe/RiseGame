using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public TileManager tileManager;
    public int height;
    public int width;
    public int moves;

    // Use this for initialization
    void Start () {
        InitLevel();
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            UpdateGrid();
        }
    }

    void InitLevel()
    {
        tileManager.CreateGrid(width, height);
    }

    void UpdateGrid()
    {
        TileManager.tileListCollision.Clear();
        TileManager.moveCount = moves;
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider != null)
        {
            //if (hit.collider.transform.position == GameObject.FindGameObjectWithTag("Player").transform.position)
            //{
            TileManager.SetTrigger(hit.collider.gameObject);
            //}
        }
    }
}
